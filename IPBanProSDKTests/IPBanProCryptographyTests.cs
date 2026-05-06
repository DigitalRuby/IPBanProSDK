using System;
using System.IO;
using System.Text;

using DigitalRuby.IPBanCore;
using DigitalRuby.IPBanProSDK;

using NUnit.Framework;

namespace DigitalRuby.IPBanProSDKTests;

[TestFixture]
public class IPBanProCryptographyTests
{
    [Test]
    public void GenerateKeyPair_ProducesValidPair()
    {
        IPBanProCryptography.GenerateKeyPair(out string privateKey, out string publicKey);
        Assert.That(privateKey, Is.Not.Null.And.Not.Empty);
        Assert.That(publicKey, Is.Not.Null.And.Not.Empty);

        // public key matches what we'd derive from the private key
        var derivedPublic = IPBanProCryptography.GetPublicKeyFromPrivateKey(privateKey);
        Assert.That(derivedPublic, Is.EqualTo(publicKey));
    }

    [Test]
    public void GenerateKeyPair_NoValidate_StillProducesPair()
    {
        IPBanProCryptography.GenerateKeyPair(out string privateKey, out string publicKey, 256, validate: false);
        Assert.That(privateKey, Is.Not.Null.And.Not.Empty);
        Assert.That(publicKey, Is.Not.Null.And.Not.Empty);
    }

    [Test]
    public void GetPublicKeyFromPrivateKey_AcceptsBytes()
    {
        IPBanProCryptography.GenerateKeyPair(out string privateKey, out string publicKey);
        byte[] bytes = Convert.FromBase64String(privateKey);
        Assert.That(IPBanProCryptography.GetPublicKeyFromPrivateKey(bytes), Is.EqualTo(publicKey));
    }

    [Test]
    public void GetPublicKeyFromPrivateKey_BadInput_ReturnsNull()
    {
        // not base64, throws inside which gets logged and returns null
        Assert.That(IPBanProCryptography.GetPublicKeyFromPrivateKey("$$$not-base64$$$"), Is.Null);
    }

    [Test]
    public void ComputeAndVerifySignature_RoundTripStringKey()
    {
        IPBanProCryptography.GenerateKeyPair(out string priv, out string pub);
        string sig = IPBanProCryptography.ComputeSignature("hello world", priv);
        Assert.That(sig, Is.Not.Null);
        Assert.That(IPBanProCryptography.VerifySignature("hello world", pub, sig), Is.True);
        Assert.That(IPBanProCryptography.VerifySignature("hello earth", pub, sig), Is.False);
    }

    [Test]
    public void ComputeAndVerifySignature_RoundTripByteKey()
    {
        IPBanProCryptography.GenerateKeyPair(out string priv, out string pub);
        byte[] privBytes = Convert.FromBase64String(priv);
        byte[] pubBytes = Convert.FromBase64String(pub);
        byte[] message = Encoding.UTF8.GetBytes("hello world");
        string sig = IPBanProCryptography.ComputeSignature(message, privBytes);
        Assert.That(sig, Is.Not.Null);
        Assert.That(IPBanProCryptography.VerifySignature(message, pubBytes, sig), Is.True);
    }

    [Test]
    public void ComputeSignature_NullPrivateKeyReturnsNull()
    {
        // empty byte[] private key triggers the null/empty short-circuit
        var sig = IPBanProCryptography.ComputeSignature("hello", Array.Empty<byte>());
        Assert.That(sig, Is.Null);
    }

    [Test]
    public void VerifySignature_BadSignatureReturnsFalse()
    {
        IPBanProCryptography.GenerateKeyPair(out _, out string pub);
        Assert.That(IPBanProCryptography.VerifySignature("msg", pub, "$$$not-base64$$$"), Is.False);
    }

    [Test]
    public void HmacSha1Sign_KnownVector()
    {
        // known: HMAC-SHA1("Hi There", key=0x0b * 20) = b617318655057264e28bc0b6fb378c8ef146be00 (RFC 2202)
        byte[] key = new byte[20];
        for (int i = 0; i < key.Length; i++) key[i] = 0x0b;
        string sig = IPBanProCryptography.HmacSha1Sign("Hi There", key);
        Assert.That(Convert.FromBase64String(sig), Is.EqualTo(Convert.FromHexString("b617318655057264e28bc0b6fb378c8ef146be00")));
    }

    [Test]
    public void HmacSha256Sign_KnownVector()
    {
        // RFC 4231 test case 1
        byte[] key = new byte[20];
        for (int i = 0; i < key.Length; i++) key[i] = 0x0b;
        string sig = IPBanProCryptography.HmacSha256Sign("Hi There", key);
        Assert.That(Convert.FromBase64String(sig), Is.EqualTo(Convert.FromHexString("b0344c61d8db38535ca8afceaf0bf12b881dc200c9833da726e9376c2e32cff7")));
    }

    [Test]
    public void HashKey_StableAndUrlSafe()
    {
        string h1 = IPBanProCryptography.HashKey("hello");
        string h2 = IPBanProCryptography.HashKey("hello");
        Assert.That(h1, Is.EqualTo(h2));
        Assert.That(h1, Does.Not.Contain("/"));
        Assert.That(h1, Does.Not.Contain("+"));
        Assert.That(h1, Does.Not.Contain("="));
    }

    [Test]
    public void HashKey_NullCoercesToEmpty()
    {
        Assert.That(IPBanProCryptography.HashKey(null), Is.EqualTo(IPBanProCryptography.HashKey(string.Empty)));
    }

    [Test]
    public void GenerateApiKeys_WritesCsv()
    {
        string path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".csv");
        try
        {
            IPBanProCryptography.GenerateApiKeys(path, 5);
            string[] lines = File.ReadAllLines(path);
            Assert.That(lines.Length, Is.EqualTo(6)); // header + 5 rows
            Assert.That(lines[0], Is.EqualTo("PartitionKey,RowKey,CustomerId,TrustLevel,Notes"));
            foreach (var line in lines[1..])
            {
                // generated keys have slashes replaced by hyphens by default
                Assert.That(line, Does.Not.Contain("/"));
            }
        }
        finally
        {
            if (File.Exists(path)) File.Delete(path);
        }
    }

    [Test]
    public void GenerateApiKeys_KeepSlashes()
    {
        string path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".csv");
        try
        {
            IPBanProCryptography.GenerateApiKeys(path, 2, changeSlashToHyphen: false);
            string[] lines = File.ReadAllLines(path);
            Assert.That(lines.Length, Is.EqualTo(3));
        }
        finally
        {
            if (File.Exists(path)) File.Delete(path);
        }
    }

    [Test]
    public void ValidateKeyPair_BadPair_Throws()
    {
        IPBanProCryptography.GenerateKeyPair(out string p1, out _);
        IPBanProCryptography.GenerateKeyPair(out _, out string p2);
        Assert.Throws<InvalidDataException>(() =>
            IPBanProCryptography.ValidateKeyPair(Convert.FromBase64String(p1), Convert.FromBase64String(p2)));
    }

    [Test]
    public void ValidateKeyPair_ValidPair_DoesNotThrow()
    {
        // exercises the full sign-and-verify happy path that GenerateKeyPair already triggers internally
        IPBanProCryptography.GenerateKeyPair(out string priv, out string pub);
        Assert.DoesNotThrow(() =>
            IPBanProCryptography.ValidateKeyPair(Convert.FromBase64String(priv), Convert.FromBase64String(pub)));
    }

    [Test]
    public void ComputeSignature_UnsupportedKeyType_ReturnsNull()
    {
        // BytesFromObject (internal) throws ArgumentException when handed an int; ComputeSignature's
        // catch swallows it and returns null. Same for GetPublicKeyFromPrivateKey.
        Assert.That(IPBanProCryptography.GetPublicKeyFromPrivateKey(123), Is.Null);
        Assert.That(IPBanProCryptography.ComputeSignature("msg", 123), Is.Null);
    }
}
