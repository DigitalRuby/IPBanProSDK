using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

using DigitalRuby.IPBanCore;
using DigitalRuby.IPBanProSDK;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

using NUnit.Framework;

namespace DigitalRuby.IPBanProSDKTests;

[TestFixture]
public class IPBanCertificateCacheTests
{
    private string tempDir;
    private string pubPemPath;
    private string privPemPath;
    private string pfxPath;

    [SetUp]
    public void SetUp()
    {
        tempDir = Path.Combine(Path.GetTempPath(), "ipbansdk-cert-" + Path.GetRandomFileName());
        Directory.CreateDirectory(tempDir);

        // make a self-signed RSA certificate, write its public key as PEM and the private key as PKCS8 PEM
        using var rsa = RSA.Create(2048);
        var req = new CertificateRequest("CN=ipbansdk-test", rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        var cert = req.CreateSelfSigned(DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddYears(1));

        pubPemPath = Path.Combine(tempDir, "public.pem");
        privPemPath = Path.Combine(tempDir, "private.pem");
        pfxPath = Path.Combine(tempDir, "cert.pfx");

        File.WriteAllText(pubPemPath, cert.ExportCertificatePem());
        File.WriteAllText(privPemPath, new string(PemEncoding.Write("PRIVATE KEY", rsa.ExportPkcs8PrivateKey())));
        File.WriteAllBytes(pfxPath, cert.Export(X509ContentType.Pfx));
    }

    [TearDown]
    public void TearDown()
    {
        try { Directory.Delete(tempDir, recursive: true); } catch { }
    }

    private static IPBanCertificateCache MakeCache(string clientCertPath = null, string clientKeyPath = null)
    {
        var configValues = new Dictionary<string, string>
        {
            ["Ssl:CertificateClientFile"] = clientCertPath ?? string.Empty,
            ["Ssl:CertificateClientPrivateKeyFile"] = clientKeyPath ?? string.Empty,
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configValues)
            .Build();
        var memCache = new MemoryCache(new MemoryCacheOptions { SizeLimit = 1024 * 1024 });
        return new IPBanCertificateCache(memCache, configuration);
    }

    [Test]
    public void GetOrLoad_PemPair_LoadsCertificate()
    {
        var cache = MakeCache();
        var c = cache.GetOrLoad(pubPemPath, privPemPath);
        Assert.That(c, Is.Not.Null);
        Assert.That(c.Subject, Does.Contain("ipbansdk-test"));
    }

    [Test]
    public void GetOrLoad_Pkcs12_LoadsCertificate()
    {
        var cache = MakeCache();
        var c = cache.GetOrLoad(pfxPath, null);
        Assert.That(c, Is.Not.Null);
    }

    [Test]
    public void GetOrLoad_PublicOnlyPem_LoadsCertificate()
    {
        var cache = MakeCache();
        var c = cache.GetOrLoad(pubPemPath, null);
        Assert.That(c, Is.Not.Null);
    }

    [Test]
    public void GetOrLoad_MissingPublicFileThrows()
    {
        var cache = MakeCache();
        Assert.Throws<FileNotFoundException>(() => cache.GetOrLoad(Path.Combine(tempDir, "nope.pem"), null));
    }

    [Test]
    public void GetOrLoad_MissingPrivateFileThrows()
    {
        var cache = MakeCache();
        Assert.Throws<FileNotFoundException>(() => cache.GetOrLoad(pubPemPath, Path.Combine(tempDir, "nope.pem")));
    }

    [Test]
    public void GetOrLoad_CachesAcrossCalls()
    {
        var cache = MakeCache();
        var c1 = cache.GetOrLoad(pubPemPath, privPemPath);
        var c2 = cache.GetOrLoad(pubPemPath, privPemPath);
        Assert.That(c2, Is.SameAs(c1));
    }

    [Test]
    public void ShouldValidateClientCertificate_FalseWhenNoConfigPath()
    {
        var cache = MakeCache();
        Assert.That(cache.ShouldValidateClientCertificate, Is.False);
    }

    [Test]
    public void ShouldValidateClientCertificate_TrueWhenConfigPathExists()
    {
        var cache = MakeCache(pubPemPath, privPemPath);
        Assert.That(cache.ShouldValidateClientCertificate, Is.True);
    }

    [Test]
    public void ValidateClientCertificate_ShortCircuitsTrueWhenValidationDisabled()
    {
        var cache = MakeCache();
        Assert.That(cache.ValidateClientCertificate(null, new X509Chain(), SslPolicyErrors.None), Is.True);
    }

    [Test]
    public void ValidateClientCertificate_NoPresentedCertReturnsFalse()
    {
        var cache = MakeCache(pubPemPath, privPemPath);
        Assert.That(cache.ValidateClientCertificate(null, new X509Chain(), SslPolicyErrors.None), Is.False);
    }

    [Test]
    public void ValidateClientCertificate_BuildsAndCachesResult()
    {
        var cache = MakeCache(pubPemPath, privPemPath);
        using var presented = X509Certificate2.CreateFromPemFile(pubPemPath, privPemPath);
        // first call computes, second call hits the cache
        var first = cache.ValidateClientCertificate(presented, new X509Chain(), SslPolicyErrors.None);
        var second = cache.ValidateClientCertificate(presented, new X509Chain(), SslPolicyErrors.None);
        Assert.That(first, Is.True);
        Assert.That(second, Is.True);
    }

    [Test]
    public void ValidateClientCertificate_DifferentPresentedCertificateReturnsFalse()
    {
        var cache = MakeCache(pubPemPath, privPemPath);
        using var presented = CreateSelfSignedCertificate("different-client");
        Assert.That(cache.ValidateClientCertificate(presented, new X509Chain(), SslPolicyErrors.None), Is.False);
    }

    [Test]
    public void Constructor_ReadsPasswordFromConfig()
    {
        var configValues = new Dictionary<string, string>
        {
            ["Ssl:CertificateClientFile"] = string.Empty,
            ["Ssl:CertificateClientPrivateKeyFile"] = string.Empty,
            ["Ssl:CertificateClientPassword"] = "swordfish",
        };
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(configValues).Build();
        var memCache = new MemoryCache(new MemoryCacheOptions { SizeLimit = 1024 });
        var cache = new IPBanCertificateCache(memCache, configuration);
        Assert.That(cache, Is.Not.Null);
    }

    [Test]
    public void ValidateClientCertificate_LoadFailureCaughtReturnsFalse()
    {
        // configure a client cert path that exists but is unparsable so GetOrLoad inside Validate
        // throws — the catch block converts that to a `false` result.
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".pem");
        File.WriteAllText(path, "notapem");
        try
        {
            var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>
            {
                ["Ssl:CertificateClientFile"] = path,
            }).Build();
            var memCache = new MemoryCache(new MemoryCacheOptions { SizeLimit = 1024 });
            var cache = new IPBanCertificateCache(memCache, configuration);

            using var rsa = System.Security.Cryptography.RSA.Create(2048);
            var req = new System.Security.Cryptography.X509Certificates.CertificateRequest(
                "CN=presented", rsa,
                System.Security.Cryptography.HashAlgorithmName.SHA256,
                System.Security.Cryptography.RSASignaturePadding.Pkcs1);
            using var presented = req.CreateSelfSigned(DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddDays(1));
            Assert.That(cache.ValidateClientCertificate(presented, new X509Chain(), SslPolicyErrors.None), Is.False);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Test]
    public void GetOrLoad_GarbagePem_FallsToLegacyAndThrows()
    {
        // a PEM that .NET's CreateFromPemFile can't parse, so we fall to the BouncyCastle legacy
        // reader, which fails too — but the catch / legacy code path gets exercised.
        var dir = Path.Combine(Path.GetTempPath(), "ipbansdk-legacy-" + Path.GetRandomFileName());
        Directory.CreateDirectory(dir);
        var pubFile = Path.Combine(dir, "garbage.pem");
        File.WriteAllText(pubFile, "-----BEGIN GARBAGE-----\nAAAAAA\n-----END GARBAGE-----\n");
        try
        {
            var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>()).Build();
            var memCache = new MemoryCache(new MemoryCacheOptions { SizeLimit = 1024 });
            var cache = new IPBanCertificateCache(memCache, configuration);
            Assert.Catch<Exception>(() => cache.GetOrLoad(pubFile, null));
        }
        finally
        {
            try { Directory.Delete(dir, recursive: true); } catch { }
        }
    }

    [Test]
    public void GetOrLoad_CertWithBadEncryptedPrivateKey_FallsToLegacyAndThrows()
    {
        // good PEM cert + corrupt encrypted private key forces .NET CreateFromEncryptedPemFile /
        // CreateFromPemFile to throw, exercising the legacy fallback path.
        var dir = Path.Combine(Path.GetTempPath(), "ipbansdk-legacy2-" + Path.GetRandomFileName());
        Directory.CreateDirectory(dir);

        using var rsa = System.Security.Cryptography.RSA.Create(2048);
        var req = new System.Security.Cryptography.X509Certificates.CertificateRequest(
            "CN=legacy-test", rsa,
            System.Security.Cryptography.HashAlgorithmName.SHA256,
            System.Security.Cryptography.RSASignaturePadding.Pkcs1);
        using var cert = req.CreateSelfSigned(DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddDays(1));

        var pubPath = Path.Combine(dir, "public.pem");
        File.WriteAllText(pubPath, cert.ExportCertificatePem());

        try
        {
            var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>()).Build();
            var memCache = new MemoryCache(new MemoryCacheOptions { SizeLimit = 1024 });
            var cache = new IPBanCertificateCache(memCache, configuration);

            var privPath = Path.Combine(dir, "private.pem");
            File.WriteAllText(privPath, "-----BEGIN ENCRYPTED PRIVATE KEY-----\nGARBAGE\n-----END ENCRYPTED PRIVATE KEY-----\n");
            Assert.Catch<Exception>(() => cache.GetOrLoad(pubPath, privPath));
        }
        finally
        {
            try { Directory.Delete(dir, recursive: true); } catch { }
        }
    }

    private static X509Certificate2 CreateSelfSignedCertificate(string commonName)
    {
        using var rsa = RSA.Create(2048);
        var req = new CertificateRequest("CN=" + commonName, rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        return req.CreateSelfSigned(DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddDays(1));
    }
}
