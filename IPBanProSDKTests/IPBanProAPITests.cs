using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using DigitalRuby.IPBanProSDK;
using DigitalRuby.IPBanProSDK.Model.WhitelistUrls;

using NUnit.Framework;

namespace DigitalRuby.IPBanProSDKTests;

[TestFixture]
public class IPBanProAPITests
{
    private FakeHttpRequestMaker fake;
    private IPBanProAPI api;

    [SetUp]
    public void SetUp()
    {
        fake = new FakeHttpRequestMaker
        {
            ResponseFactory = (uri, body) => FakeHttpRequestMaker.Json(new { Message = "ok" }),
        };
        api = new IPBanProAPI { RequestMaker = fake };
    }

    [TearDown]
    public void TearDown() => api.Dispose();

    [Test]
    public async Task GetCountryNames_HitsExpectedPath()
    {
        await api.GetCountryNamesAsync();
        Assert.That(fake.LastUri.AbsolutePath, Does.EndWith("/CountryNames"));
    }

    [Test]
    public async Task GetAsnNames_HitsExpectedPath()
    {
        await api.GetAsnNamesAsync();
        Assert.That(fake.LastUri.AbsolutePath, Does.EndWith("/AsnNames"));
    }

    [Test]
    public async Task GetCountryNamesList_EncodesQuery()
    {
        await api.GetCountryNamesListAsync("en", "uni ted");
        Assert.That(fake.LastUri.Query, Does.Contain("languageCode=en"));
        Assert.That(fake.LastUri.Query, Does.Contain("query=uni"));
    }

    [Test]
    public async Task GetAsnNamesList_EncodesQuery()
    {
        await api.GetAsnNamesListAsync("en", "google");
        Assert.That(fake.LastUri.Query, Does.Contain("languageCode=en"));
        Assert.That(fake.LastUri.Query, Does.Contain("query=google"));
    }

    [Test]
    public async Task GetIPAddressGeography_PublicIPMakesRequest()
    {
        var result = await api.GetIPAddressGeographyAsync("8.8.8.8");
        Assert.That(fake.CallCount, Is.EqualTo(1));
        Assert.That(fake.LastUri.AbsolutePath, Does.EndWith("/IP/8.8.8.8"));
    }

    [Test]
    public async Task GetIPAddressGeography_InternalIPSkipsRequest()
    {
        var result = await api.GetIPAddressGeographyAsync("127.0.0.1");
        Assert.That(fake.CallCount, Is.EqualTo(0));
        Assert.That(result, Is.Not.Null);
        Assert.That(result.IPAddress, Is.EqualTo("127.0.0.1"));
    }

    [Test]
    public async Task GetIPAddressGeography_InvalidIPSkipsRequest()
    {
        var result = await api.GetIPAddressGeographyAsync("not-an-ip");
        Assert.That(fake.CallCount, Is.EqualTo(0));
        Assert.That(result.IPAddress, Is.EqualTo("not-an-ip"));
    }

    [Test]
    public async Task GetIPAddressCountryGeography_PublicIPMakesRequest()
    {
        await api.GetIPAddressCountryGeographyAsync("8.8.8.8");
        Assert.That(fake.LastUri.AbsolutePath, Does.EndWith("/IPCountry/8.8.8.8"));
    }

    [Test]
    public async Task GetIPAddressCountryGeography_InternalIPSkipsRequest()
    {
        var result = await api.GetIPAddressCountryGeographyAsync("10.0.0.1");
        Assert.That(fake.CallCount, Is.EqualTo(0));
        Assert.That(result.IPAddress, Is.EqualTo("10.0.0.1"));
    }

    [Test]
    public async Task GetNaughtyList_RequestsRangedList()
    {
        await api.GetNaughtyListAsync();
        Assert.That(fake.LastUri.PathAndQuery, Does.Contain("IPNaughtyList?ranges=1"));
    }

    [Test]
    public async Task GetRecentBanList_RequestsRangedList()
    {
        await api.GetRecentBanListAsync();
        Assert.That(fake.LastUri.PathAndQuery, Does.Contain("IPRecentList?ranges=1"));
    }

    [Test]
    public async Task GetIPListsMetadata_HitsExpectedPath()
    {
        await api.GetIPListsMetadataAsync();
        Assert.That(fake.LastUri.AbsolutePath, Does.EndWith("/iplistsmetadata"));
    }

    [Test]
    public async Task GetIPList_EncodesKey()
    {
        await api.GetIPList("my key");
        Assert.That(fake.LastUri.Query, Does.Contain("key=my"));
    }

    [Test]
    public async Task GetRecentNaughtyCounts_HitsExpectedPath()
    {
        await api.GetRecentNaughtyCounts();
        Assert.That(fake.LastUri.AbsolutePath, Does.EndWith("/iprecentnaughtylistcount"));
    }

    [Test]
    public async Task GetIPAddressCountryRanges_HitsExpectedPath()
    {
        await api.GetIPAddressCountryRangesAsync("US");
        Assert.That(fake.LastUri.AbsolutePath, Does.EndWith("/IPCountryRanges/US"));
    }

    [Test]
    public async Task GetIPAddressAsnRanges_HitsExpectedPath()
    {
        await api.GetIPAddressAsnRangesAsync("15169");
        Assert.That(fake.LastUri.AbsolutePath, Does.EndWith("/IPAsnRanges/15169"));
    }

    [Test]
    public async Task GetIPAddressAsnInfo_HitsExpectedPath()
    {
        await api.GetIPAddressAsnInfoAsync();
        Assert.That(fake.LastUri.AbsolutePath, Does.EndWith("/IPAsnInfo"));
    }

    [Test]
    public async Task WhitelistGetUrls_NoIdSendsBareEndpoint()
    {
        await api.WhitelistGetUrls(new GetUrlsRequest { Id = "" });
        Assert.That(fake.LastUri.AbsolutePath, Does.EndWith("/aw/urls"));
        Assert.That(fake.LastUri.Query, Is.Empty);
    }

    [Test]
    public async Task WhitelistGetUrls_WithIdAddsQuery()
    {
        await api.WhitelistGetUrls(new GetUrlsRequest { Id = "abc 1" });
        Assert.That(fake.LastUri.Query, Does.Contain("id=abc"));
    }

    [Test]
    public async Task UpsertWhitelistUrl_PostsBody()
    {
        await api.UpsertWhitelistUrl(new UpsertUrlRequest());
        Assert.That(fake.LastUri.AbsolutePath, Does.EndWith("/aw/upsert"));
        Assert.That(fake.LastPostJson, Is.Not.Null);
    }

    [Test]
    public async Task DeleteWhitelistUrl_UsesDeleteMethod()
    {
        await api.DeleteWhitelistUrl(new DeleteUrlRequest());
        Assert.That(fake.LastUri.AbsolutePath, Does.EndWith("/aw/delete"));
        Assert.That(fake.LastMethod, Is.EqualTo(HttpMethod.Delete.Method));
    }

    [Test]
    public async Task ConsumeUrl_BuildsQuery()
    {
        await api.ConsumeUrl(new ConsumeUrlRequest { GroupId = "g1", Id = "i1" });
        Assert.That(fake.LastUri.Query, Does.Contain("a=g1"));
        Assert.That(fake.LastUri.Query, Does.Contain("b=i1"));
    }
}
