/*

IPBanPro SDK - https://ipban.com | https://github.com/DigitalRuby/IPBanProSDK
IPBan and IPBan Pro Copyright(c) 2012 Digital Ruby, LLC
support@ipban.com

The MIT License(MIT)

Copyright(c) 2012 Digital Ruby, LLC

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

*/

using DigitalRuby.IPBanCore;

using Microsoft.Extensions.Logging;

using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DigitalRuby.IPBanProSDK
{
    /// <summary>
    /// Cryptopgraphy helper methods
    /// </summary>
    public static class IPBanProCryptography
    {
        private static readonly X9ECParameters curve = SecNamedCurves.GetByName("secp256r1");
        private static readonly ECDomainParameters domain = new(curve.Curve, curve.G, curve.N, curve.H);
        private const string signingAlgorithm = "SHA-512withECDSA";

        /// <summary>
        /// Generate lots of api keys
        /// </summary>
        /// <param name="outputCsvFile">Output csv file</param>
        /// <param name="count">Count of keys to generate</param>
        /// <param name="changeSlashToHyphen">Whether to replace slashes to hyphens, needed for azure or other storage systems where slash is an invalid char</param>
        public static void GenerateApiKeys(string outputCsvFile, int count, bool changeSlashToHyphen = true)
        {
            using StreamWriter writer = File.CreateText(outputCsvFile);
            writer.WriteLine("PartitionKey,RowKey,CustomerId,TrustLevel,Notes");
            Parallel.ForEach(new int[count], (ignore) =>
            {
                GenerateKeyPair(out string privateKey, out string publicKey);
                if (changeSlashToHyphen)
                {
                    privateKey = privateKey.Replace('/', '-');
                    publicKey = publicKey.Replace('/', '-');
                }
                lock (writer)
                {
                    writer.WriteLine("{0},{1},,0,", publicKey, privateKey);
                }
            });
        }

        /// <summary>
        /// Generate a private key using ECDSA and secp256r1
        /// </summary>
        /// <param name="privateKeyBase64">Private key in base64</param>
        /// <param name="publicKeyBase64">Public key in base64</param>
        /// <param name="strength">Strength of key in bits, 256 is the standard, 384 or 521 are also possible</param>
        /// <param name="validate">Whether to validate the keys</param>
        /// <returns>Private key string in base64.</returns>
        /// <exception cref="System.IO.InvalidDataException">Validate is true and the generated key is not valid</exception>
        public static void GenerateKeyPair(out string privateKeyBase64, out string publicKeyBase64, int strength = 256, bool validate = true)
        {
            ECKeyPairGenerator keyGenerator = new("ECDSA");
            keyGenerator.Init(new KeyGenerationParameters(new SecureRandom(), strength));
            AsymmetricCipherKeyPair pair = keyGenerator.GenerateKeyPair();
            ECPrivateKeyParameters privateKey = pair.Private as ECPrivateKeyParameters;
            ECPublicKeyParameters publicKey = pair.Public as ECPublicKeyParameters;
            byte[] privateKeyBytes = privateKey.D.ToByteArray();
            byte[] publicKeyBytes = publicKey.Q.GetEncoded(true);
            if (validate)
            {
                ValidateKeyPair(privateKeyBytes, publicKeyBytes);
            }
            publicKeyBase64 = Convert.ToBase64String(publicKeyBytes);
            privateKeyBase64 = Convert.ToBase64String(privateKeyBytes);
        }

        /// <summary>
        /// Validate a key pair
        /// </summary>
        /// <param name="privateKeyBytes">Private key bytes</param>
        /// <param name="publicKeyBytes">Public key bytes</param>
        /// <exception cref="System.IO.InvalidDataException">Key pair is invalid</exception>
        public static void ValidateKeyPair(byte[] privateKeyBytes, byte[] publicKeyBytes)
        {
            string privateKeyString = Convert.ToBase64String(privateKeyBytes);
            string publicKeyString = Convert.ToBase64String(publicKeyBytes);
            string computedPublicKeyString = GetPublicKeyFromPrivateKey(privateKeyBytes);
            string computedPublicKeyString2 = GetPublicKeyFromPrivateKey(privateKeyString);
            if (publicKeyString != computedPublicKeyString || computedPublicKeyString != computedPublicKeyString2)
            {
                throw new InvalidDataException("Key generation failure, public key is not valid");
            }

            // ensure we can sign a message
            string signature1 = ComputeSignature("test", privateKeyString);
            string signature2 = ComputeSignature("test", privateKeyBytes);

            if (!VerifySignature("test", publicKeyString, signature1) ||
                !VerifySignature("test", publicKeyBytes, signature1) ||
                !VerifySignature("test", publicKeyString, signature2) ||
                !VerifySignature("test", publicKeyBytes, signature2))
            {
                throw new InvalidDataException("Key generation failure, public key is not valid");
            }

            if (VerifySignature("test", publicKeyString, "asdf") ||
                VerifySignature("atest", publicKeyString, signature1) ||
                VerifySignature("atest", publicKeyString, signature2))
            {
                throw new InvalidDataException("Signature generation is broken");
            }
        }

        /// <summary>
        /// Get a public key in base64 encoding from a private key. The public key can be shared freely.
        /// </summary>
        /// <param name="privateKey">Private key. Keep it secret, keep it safe. Must be string, SecureString or byte[].</param>
        /// <param name="logger">Logger for any errors</param>
        /// <returns>Public key in base64 encoding or null if exception.</returns>
        public static string GetPublicKeyFromPrivateKey(object privateKey, ILogger logger = null)
        {
            try
            {
                byte[] privateKeyBytes = IPBanProSDKExtensionMethods.BytesFromObject(privateKey, true);
                BigInteger d = new(1, privateKeyBytes);
                Org.BouncyCastle.Math.EC.ECPoint q = domain.G.Multiply(d);
                ECPublicKeyParameters publicKey = new("ECDSA", q, domain);
                return Convert.ToBase64String(publicKey.Q.GetEncoded(true));
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Failed to get public key from private key");
                return null;
            }
        }

        /// <summary>
        /// Compute a signature for a message using a private key. This signature is base64 encoded and can be verified with just the public key.
        /// </summary>
        /// <param name="message">Message. Must be string, SecureString, or byte[].</param>
        /// <param name="privateKey">Private key. Keep it secret, keep it safe. Must be string, SecureString or byte[].</param>
        /// <param name="logger">Logger</param>
        /// <returns>Signature in base64 encoding or null if failure.</returns>
        public static string ComputeSignature(object message, object privateKey, ILogger logger = null)
        {
            try
            {
                byte[] messageBytes = IPBanProSDKExtensionMethods.BytesFromObject(message, false);
                byte[] privateKeyBytes = IPBanProSDKExtensionMethods.BytesFromObject(privateKey, true);
                if (privateKeyBytes is null || privateKeyBytes.Length == 0)
                {
                    return null;
                }
                ECPrivateKeyParameters keyParameters = new("ECDSA", new BigInteger(1, privateKeyBytes), domain);
                ISigner signer = SignerUtilities.GetSigner(signingAlgorithm);
                signer.Init(true, keyParameters);
                signer.BlockUpdate(messageBytes, 0, messageBytes.Length);
                byte[] signature = signer.GenerateSignature();
                string signatureString = Convert.ToBase64String(signature);
                //string publicKey = GetPublicKeyFromPrivateKey(privateKeyBytes);
                //bool verify = VerifySignature(messageBytes, publicKey, signatureString);
                return signatureString;
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Failed to compute signature");
                return null;
            }
        }

        /// <summary>
        /// Verify a signature created with ComputeSignature using a public key.
        /// </summary>
        /// <param name="message">Message to verify. Must be string, SecureString or byte[].</param>
        /// <param name="publicKey">Public key in base64 encoding. May be shared freely.</param>
        /// <param name="signature">Signature, in base64 encoding.</param>
        /// <param name="logger">Logger</param>
        /// <returns>True if signature is valid, false if not</returns>
        public static bool VerifySignature(object message, object publicKey, string signature, ILogger logger = null)
        {
            try
            {
                byte[] messageBytes = IPBanProSDKExtensionMethods.BytesFromObject(message, false);
                byte[] publicKeyBytes = IPBanProSDKExtensionMethods.BytesFromObject(publicKey, true);
                Org.BouncyCastle.Math.EC.ECPoint q = curve.Curve.DecodePoint(publicKeyBytes);
                ECPublicKeyParameters keyParameters = new("ECDSA", q, domain);
                ISigner signer = SignerUtilities.GetSigner(signingAlgorithm);
                signer.Init(false, keyParameters);
                signer.BlockUpdate(messageBytes, 0, messageBytes.Length);
                byte[] signatureBytes = Convert.FromBase64String(signature);
                return signer.VerifySignature(signatureBytes);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Failed to verify signature");
                return false;
            }
        }

        /// <summary>
        /// Sign a message with SHA1 hash
        /// </summary>
        /// <param name="message">Message to sign</param>
        /// <param name="key">Private key bytes</param>
        /// <returns>Signature in base64</returns>
        public static string HmacSha1Sign(string message, byte[] key)
        {
            using HMACSHA1 sha = new(key);
            return Convert.ToBase64String(sha.ComputeHash(message.ToBytesUTF8()));
        }

        /// <summary>
        /// Sign a message with SHA256 hash
        /// </summary>
        /// <param name="message">Message to sign</param>
        /// <param name="key">Private key bytes</param>
        /// <returns>Signature in base64</returns>
        public static string HmacSha256Sign(string message, byte[] key)
        {
            using HMACSHA256 sha = new(key);
            return Convert.ToBase64String(sha.ComputeHash(message.ToBytesUTF8()));
        }
    }
}
