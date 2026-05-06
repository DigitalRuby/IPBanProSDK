using System;
using System.Linq;
using System.Net.Http;
using System.Security;
using System.Text;
using System.Threading.Tasks;

using DigitalRuby.IPBanCore;
using DigitalRuby.IPBanProSDK;

using NUnit.Framework;

namespace DigitalRuby.IPBanProSDKTests;

[TestFixture]
public class IPBanProBaseAPITests
{
    private static SecureString Sec(string s) => s.ToSecureString();

    [Test]
    public void CreateBasicAuthorization_BuildsBasicHeader()
    {
        var auth = IPBanProBaseAPI.CreateBasicAuthorization("alice", "s3cret");
        Assert.That(auth, Does.StartWith("Basic "));
        var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(auth["Basic ".Length..]));
        Assert.That(decoded, Is.EqualTo("alice:s3cret"));
    }

    [Test]
    public void ReEncodeBasicAuthorization_ReturnsSha256OfPassword()
    {
        var original = IPBanProBaseAPI.CreateBasicAuthorization("alice", "hunter2");
        var reencoded = IPBanProBaseAPI.ReEncodeBasicAuthorization(original, out string user, out string password);
        Assert.That(user, Is.EqualTo("alice"));
        Assert.That(password, Is.EqualTo("hunter2"));
        Assert.That(reencoded, Does.StartWith("Basic "));
        // sha256-hashed password should not match the cleartext encoding
        Assert.That(reencoded, Is.Not.EqualTo(original));
    }

    [Test]
    public void ReEncodeBasicAuthorization_NullOrShortHeader_PassesThrough()
    {
        var result = IPBanProBaseAPI.ReEncodeBasicAuthorization(null, out string u, out string p);
        Assert.That(result, Is.Null);
        Assert.That(u, Is.Null);
        Assert.That(p, Is.Null);

        var result2 = IPBanProBaseAPI.ReEncodeBasicAuthorization("Bas", out u, out p);
        Assert.That(result2, Is.EqualTo("Bas"));
    }

    [Test]
    public void ReEncodeBasicAuthorization_NoColon_UnknownUser()
    {
        var bad = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes("nocolon"));
        var result = IPBanProBaseAPI.ReEncodeBasicAuthorization(bad, out string u, out string p);
        Assert.That(u, Is.EqualTo("Unknown"));
        Assert.That(p, Is.Null);
    }

    [Test]
    public void CreateSignatureDataString_StripsTokenQueryParam()
    {
        var uri = new Uri("https://api.ipban.com/Endpoint?a=1&token=abcdef&b=2");
        var data = IPBanProBaseAPI.CreateSignatureDataString(uri, "12345", Sec("PUBKEY"), "1.2.3.4");
        Assert.That(data, Does.Not.Contain("token=abcdef"));
        Assert.That(data, Does.Contain("12345"));
        Assert.That(data, Does.Contain("PUBKEY"));
    }

    [Test]
    public void CreateSignatureDataString_RelativeUriThrows()
    {
        Assert.Throws<ArgumentException>(() =>
            IPBanProBaseAPI.CreateSignatureDataString(new Uri("relative", UriKind.Relative), "1", Sec("k")));
    }

    [Test]
    public void ComputeAndVerifySignature_RoundTrip()
    {
        IPBanProCryptography.GenerateKeyPair(out string priv, out string pub);
        var uri = new Uri("https://api.ipban.com/Endpoint?x=1");
        var sig = IPBanProBaseAPI.ComputeSignature(uri, "100", Sec(pub), Sec(priv));
        Assert.That(sig, Is.Not.Null);
        Assert.That(IPBanProBaseAPI.VerifySignature(uri, "100", Sec(pub), sig), Is.True);
    }

    [Test]
    public void GetApiRequestHeaders_NoApiKey_NoApiHeaders()
    {
        using var api = new IPBanProBaseAPI { BaseUri = new Uri("https://api.ipban.com") };
        var headers = api.GetApiRequestHeaders(new Uri("https://api.ipban.com/foo"));
        Assert.That(headers.Any(h => h.Key == IPBanProBaseAPI.HeaderApiKey), Is.False);
        Assert.That(headers.Any(h => h.Key == "User-Agent"), Is.True);
    }

    [Test]
    public void GetApiRequestHeaders_WithKeysAddsSignatureHeaders()
    {
        IPBanProCryptography.GenerateKeyPair(out string priv, out string pub);
        using var api = new IPBanProBaseAPI { BaseUri = new Uri("https://api.ipban.com") };
        api.SetKeys(pub, priv);
        var headers = api.GetApiRequestHeaders(new Uri("https://api.ipban.com/foo"));
        Assert.That(headers.Any(h => h.Key == IPBanProBaseAPI.HeaderApiKey), Is.True);
        Assert.That(headers.Any(h => h.Key == IPBanProBaseAPI.HeaderSignature), Is.True);
        Assert.That(headers.Any(h => h.Key == IPBanProBaseAPI.HeaderTimestamp), Is.True);
        Assert.That(headers.Any(h => h.Key == IPBanProBaseAPI.HeaderOrigin), Is.True);
    }

    [Test]
    public void GetApiRequestHeaders_DisableCacheAddsHeader()
    {
        IPBanProBaseAPI.DisableCache = true;
        try
        {
            using var api = new IPBanProBaseAPI { BaseUri = new Uri("https://api.ipban.com") };
            var headers = api.GetApiRequestHeaders(new Uri("https://api.ipban.com/foo"));
            Assert.That(headers.Any(h => h.Key == "Cache-Control"), Is.True);
        }
        finally
        {
            IPBanProBaseAPI.DisableCache = false;
        }
    }

    [Test]
    public void GetApiRequestHeaders_AuthorizationAddsHeader()
    {
        using var api = new IPBanProBaseAPI
        {
            BaseUri = new Uri("https://api.ipban.com"),
            Authorization = Sec("token-xyz"),
        };
        var headers = api.GetApiRequestHeaders(new Uri("https://api.ipban.com/foo"));
        Assert.That(headers.Any(h => h.Key == "Authorization"), Is.True);
    }

    [Test]
    public void Timestamp_DefaultIsUtcNow()
    {
        using var api = new IPBanProBaseAPI();
        var before = IPBanService.UtcNow.AddSeconds(-5);
        var after = IPBanService.UtcNow.AddSeconds(5);
        Assert.That(api.Timestamp, Is.InRange(before, after));
    }

    [Test]
    public void Timestamp_SetAndReset()
    {
        using var api = new IPBanProBaseAPI();
        var t = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc);
        api.Timestamp = t;
        Assert.That(api.Timestamp, Is.EqualTo(t));
        api.Timestamp = default; // reset
        Assert.That(api.Timestamp, Is.Not.EqualTo(t));
    }

    [Test]
    public void RequestMaker_NullThrows()
    {
        using var api = new IPBanProBaseAPI();
        Assert.Throws<ArgumentNullException>(() => api.RequestMaker = null);
    }

    [Test]
    public void SetKeys_NullClearsKeys()
    {
        using var api = new IPBanProBaseAPI();
        api.SetKeys("pub", "priv");
        Assert.That(api.PublicApiKey, Is.Not.Null);
        api.SetKeys(null, null);
        Assert.That(api.PublicApiKey, Is.Null);
    }

    [Test]
    public void SetKeys_DisposeReturnsKeysToNull()
    {
        using var api = new IPBanProBaseAPI();
        var d = api.SetKeys("pub", "priv");
        Assert.That(api.PublicApiKey, Is.Not.Null);
        d.Dispose();
        Assert.That(api.PublicApiKey, Is.Null);
        Assert.That(api.PrivateApiKey, Is.Null);
    }

    [Test]
    public void SetKeys_BadPublicTypeThrows()
    {
        using var api = new IPBanProBaseAPI();
        Assert.Throws<ArgumentException>(() => api.SetKeys(123, "priv"));
    }

    [Test]
    public void SetKeys_BadPrivateTypeThrows()
    {
        using var api = new IPBanProBaseAPI();
        Assert.Throws<ArgumentException>(() => api.SetKeys("pub", 123));
    }

    [Test]
    public void SetKeys_AcceptsSecureStringsDirectly()
    {
        using var api = new IPBanProBaseAPI();
        api.SetKeys(Sec("pub"), Sec("priv"));
        Assert.That(api.PublicApiKey, Is.Not.Null);
        Assert.That(api.PrivateApiKey, Is.Not.Null);
    }

    [Test]
    public async Task MakeRequestAsync_PostJson_SerializesObjectAndDeserializesResponse()
    {
        var fake = new FakeHttpRequestMaker
        {
            Response = FakeHttpRequestMaker.Json(new { Message = "ok" }),
        };
        using var api = new IPBanProBaseAPI
        {
            BaseUri = new Uri("https://api.ipban.com"),
            RequestMaker = fake,
        };
        var result = await api.MakeRequestAsync<BlacklistedIPAddressesModel>("foo", new { hello = "world" });
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Message, Is.EqualTo("ok"));
        Assert.That(fake.LastUri.AbsoluteUri, Is.EqualTo("https://api.ipban.com/foo"));
        Assert.That(Encoding.UTF8.GetString(fake.LastPostJson), Does.Contain("hello"));
    }

    [Test]
    public async Task MakeRequestAsync_PostStringPassesAsBytes()
    {
        var fake = new FakeHttpRequestMaker { Response = FakeHttpRequestMaker.Json(new { Message = "ok" }) };
        using var api = new IPBanProBaseAPI { BaseUri = new Uri("https://api.ipban.com"), RequestMaker = fake };
        await api.MakeRequestAsync<BlacklistedIPAddressesModel>("foo", "literal-string-body");
        Assert.That(Encoding.UTF8.GetString(fake.LastPostJson), Is.EqualTo("literal-string-body"));
    }

    [Test]
    public async Task MakeRequestAsync_PostBytesPassesThrough()
    {
        var fake = new FakeHttpRequestMaker { Response = FakeHttpRequestMaker.Json(new { Message = "ok" }) };
        using var api = new IPBanProBaseAPI { BaseUri = new Uri("https://api.ipban.com"), RequestMaker = fake };
        var bytes = new byte[] { 1, 2, 3 };
        await api.MakeRequestAsync<BlacklistedIPAddressesModel>("foo", bytes);
        Assert.That(fake.LastPostJson, Is.EqualTo(bytes));
    }

    [Test]
    public async Task MakeRequestAsync_EmptyResponseReturnsNull()
    {
        var fake = new FakeHttpRequestMaker { Response = Array.Empty<byte>() };
        using var api = new IPBanProBaseAPI { BaseUri = new Uri("https://api.ipban.com"), RequestMaker = fake };
        var result = await api.MakeRequestAsync<BlacklistedIPAddressesModel>("foo");
        Assert.That(result, Is.Null);
    }

    [Test]
    public void MakeRequestAsync_ErrorTrueResponseThrows()
    {
        var fake = new FakeHttpRequestMaker
        {
            Response = FakeHttpRequestMaker.Json(new { Error = true, Message = "bad" }),
        };
        using var api = new IPBanProBaseAPI { BaseUri = new Uri("https://api.ipban.com"), RequestMaker = fake };
        var ex = Assert.ThrowsAsync<HttpRequestException>(async () =>
            await api.MakeRequestAsync<BlacklistedIPAddressesModel>("foo"));
        Assert.That(ex.Message, Does.Contain("bad"));
    }

    [Test]
    public void MakeRequestAsync_TransportErrorRethrowsWrapped()
    {
        var fake = new FakeHttpRequestMaker
        {
            ToThrow = new HttpRequestException("boom", null, System.Net.HttpStatusCode.BadGateway),
        };
        using var api = new IPBanProBaseAPI { BaseUri = new Uri("https://api.ipban.com"), RequestMaker = fake };
        Assert.ThrowsAsync<HttpRequestException>(async () =>
            await api.MakeRequestAsync<BlacklistedIPAddressesModel>("foo"));
    }

    [Test]
    public void BaseUriOverride_AppliedOnConstruction()
    {
        IPBanProBaseAPI.BaseUriOverride[typeof(MyTestApi)] = new Uri("https://override.example.com");
        try
        {
            using var api = new MyTestApi();
            Assert.That(api.BaseUri.Host, Is.EqualTo("override.example.com"));
        }
        finally
        {
            IPBanProBaseAPI.BaseUriOverride.Remove(typeof(MyTestApi));
        }
    }

    [Test]
    public void StartWebSocket_AssignsRequestHeadersAndStartsSocket()
    {
        var fake = new FakeClientWebSocket();
        DigitalRuby.IPBanProSDK.ClientWebSocket.RegisterWebSocketCreator(_ => fake);
        try
        {
            using var api = new IPBanProBaseAPI { BaseUri = new Uri("https://api.ipban.com") };
            using var sock = new DigitalRuby.IPBanProSDK.ClientWebSocket
            {
                Uri = new Uri("ws://example.com/test"),
                ReconnectInterval = TimeSpan.FromMilliseconds(50),
                PingInterval = TimeSpan.Zero,
            };
            api.StartWebSocket(sock);
            Assert.That(sock.RequestHeaders, Is.Not.Null);
        }
        finally
        {
            DigitalRuby.IPBanProSDK.ClientWebSocket.RegisterWebSocketCreator(null);
        }
    }

    private sealed class MyTestApi : IPBanProBaseAPI
    {
    }
}
