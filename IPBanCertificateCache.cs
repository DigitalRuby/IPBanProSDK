#nullable enable

using System;
using System.IO;
using System.Net.Security;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

using DigitalRuby.IPBanCore;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;

namespace DigitalRuby.IPBanProSDK;

/// <summary>
/// Certificate cache
/// </summary>
public interface ICertificateCache
{
    X509Certificate2 GetOrLoad(string publicKeyFile, string? privateKeyFile, SecureString? password = null);

    /// <summary>
    /// Validate client certificate against configuration
    /// </summary>
    /// <param name="presentedCert">Presented client certificate (never null when framework invokes delegate)</param>
    /// <param name="chain">TLS chain (may not yet be built with our policy)</param>
    /// <param name="errors">Policy errors</param>
    /// <returns>True if the certificate is accepted</returns>
    bool ValidateClientCertificate(X509Certificate2 presentedCert, X509Chain chain, SslPolicyErrors errors);

    /// <summary>
    /// Whether client certificate validation should be performed
    /// </summary>
    bool ShouldValidateClientCertificate { get; }
}

/// <inheritdoc />
public sealed class IPBanCertificateCache : ICertificateCache
{
    private static readonly TimeSpan oneHour = TimeSpan.FromHours(1.0);
    private static readonly TimeSpan oneDay = TimeSpan.FromDays(1.0);

    private readonly IMemoryCache memoryCache;
    private readonly string clientCertificatePath;
    private readonly string clientCertificatePrivateKeyPath;
    private readonly SecureString? clientCertificatePassword = null;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="memoryCache">Memory cache</param>
    /// <param name="configuration">Config</param>
    public IPBanCertificateCache(IMemoryCache memoryCache, IConfiguration configuration)
    {
        this.memoryCache = memoryCache;
        clientCertificatePath = configuration["Ssl:CertificateClientFile"] ?? string.Empty;
        clientCertificatePrivateKeyPath = configuration["Ssl:CertificateClientPrivateKeyFile"] ?? string.Empty;
        string? password = configuration["Ssl:CertificateClientPassword"];
        if (!string.IsNullOrWhiteSpace(password))
        {
            clientCertificatePassword = password.ToSecureString();
        }
    }

    /// <inheritdoc />
    public X509Certificate2 GetOrLoad(string publicKeyFile, string? privateKeyFile, SecureString? password = null)
    {
        string hash = $"Cert_{publicKeyFile}_{privateKeyFile}";
        lock (this)
        {
            return memoryCache.GetOrCreate(hash, entry =>
            {
                entry.Size = 8192;
                entry.AbsoluteExpirationRelativeToNow = oneDay;
                return LoadCertificateInternal(publicKeyFile, privateKeyFile, password);
            })!;
        }
    }

    /// <inheritdoc />
    public bool ValidateClientCertificate(X509Certificate2 presentedCert, X509Chain chain, SslPolicyErrors errors)
    {
        if (!ShouldValidateClientCertificate)
        {
            return true;
        }
        else if (presentedCert is null)
        {
            Logger.Warn("Client certificate validation failed: no certificate provided");
            return false;
        }

        var cacheKey = $"{nameof(IPBanCertificateCache)}_{nameof(ValidateClientCertificate)}_{presentedCert.Thumbprint}";
        var cacheOpt = new MemoryCacheEntryOptions
        {
            Size = 8,
            AbsoluteExpirationRelativeToNow = oneHour
        };

        try
        {
            var valid = memoryCache.Get<bool?>(cacheKey);
            if (valid is not null)
            {
                return valid.Value;
            }

            var clientCertificate = GetOrLoad(clientCertificatePath, clientCertificatePrivateKeyPath, clientCertificatePassword);
            chain.ChainPolicy.ExtraStore.Add(clientCertificate);
            chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllowUnknownCertificateAuthority;
            var result = chain.Build(clientCertificate);
            memoryCache.Set<bool?>(cacheKey, result, cacheOpt);
            return result;
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Unexpected error validating client certificate");
            return false;
        }
    }

    /// <inheritdoc />
    public bool ShouldValidateClientCertificate => File.Exists(clientCertificatePath);

    private static X509Certificate2 LoadCertificateInternal(string publicKeyFile, string? privateKeyFile, SecureString? password)
    {
        if (!File.Exists(publicKeyFile))
        {
            throw new FileNotFoundException("Unable to find certificates at " + publicKeyFile);
        }
        else if (!string.IsNullOrWhiteSpace(privateKeyFile) && !File.Exists(privateKeyFile))
        {
            throw new FileNotFoundException("Unable to find private key at " + privateKeyFile);
        }

        X509Certificate2? result = null;

        // this should throw if loading the certificate fails
        ExtensionMethods.Retry(() =>
        {
            if (publicKeyFile.EndsWith(".pfx", StringComparison.OrdinalIgnoreCase) ||
                publicKeyFile.EndsWith(".p12", StringComparison.OrdinalIgnoreCase))
            {
                result = X509CertificateLoader.LoadPkcs12FromFile(publicKeyFile, password?.ToUnsecureString());
            }

            try
            {
                if (password is null)
                {
                    result = X509Certificate2.CreateFromPemFile(publicKeyFile, privateKeyFile);
                }
                else
                {
                    result = X509Certificate2.CreateFromEncryptedPemFile(publicKeyFile, password?.ToUnsecureString(), privateKeyFile);
                }
            }
            catch
            {
                result = LoadCertificateInternalLegacy(publicKeyFile, privateKeyFile, password);
            }
            Logger.Info("Loaded new server certificate from files: {0}, {1}", publicKeyFile, privateKeyFile);
        });

        // but just in case, check it
        if (result is null)
        {
            throw new IOException("Unexpected unknown error loading certificate at " + publicKeyFile);
        }

        return result;
    }

    private static X509Certificate2 LoadCertificateInternalLegacy(string publicKeyFile, string? privateKeyFile, SecureString? password)
    {
        Org.BouncyCastle.X509.X509Certificate? bouncyCert;
        var publicKeyText = File.ReadAllText(publicKeyFile);
        var publicKeyReader = new StringReader(publicKeyText);
        var publicKeyPemReader = new Org.BouncyCastle.OpenSsl.PemReader(publicKeyReader);
        var certObj = publicKeyPemReader.ReadObject();
        if (certObj is Org.BouncyCastle.X509.X509Certificate x509)
        {
            bouncyCert = x509;
        }
        else if (certObj is X509CertificateEntry entry)
        {
            bouncyCert = entry.Certificate;
        }
        else
        {
            throw new InvalidOperationException("Unsupported certificate format.");
        }

        // If no private key provided, return public-only certificate
        if (string.IsNullOrWhiteSpace(privateKeyFile))
        {
            return new X509Certificate2(DotNetUtilities.ToX509Certificate(bouncyCert));
        }

        // Read private key (supports legacy RSA PEM and PKCS#8)
        var privateKeyText = File.ReadAllText(privateKeyFile);
        var privateKeyReader = new StringReader(privateKeyText);
        var privateKeyPemReader = password is not null ? new Org.BouncyCastle.OpenSsl.PemReader(privateKeyReader, new PasswordFinder(password)) :
            new Org.BouncyCastle.OpenSsl.PemReader(privateKeyReader);
        var keyObj = privateKeyPemReader.ReadObject();
        var privateKey = keyObj switch
        {
            AsymmetricCipherKeyPair keyPair => keyPair.Private,
            AsymmetricKeyParameter keyParam => keyParam,
            _ => throw new InvalidOperationException("Unsupported private key format."),
        };

        // Combine public certificate with private key
        var store = new Pkcs12StoreBuilder().Build();
        var certEntry = new X509CertificateEntry(bouncyCert);
        store.SetKeyEntry("main", new AsymmetricKeyEntry(privateKey), [certEntry]);

        using var ms = new MemoryStream();
        var pwd = password?.ToUnsecureString() ?? string.Empty;
        store.Save(ms, pwd.ToCharArray(), new SecureRandom());
        return X509CertificateLoader.LoadPkcs12(ms.ToArray(), pwd);
    }

    private class PasswordFinder(SecureString password) : IPasswordFinder
    {
        public char[] GetPassword() => password.ToUnsecureString().ToCharArray();
    }
}

#nullable restore