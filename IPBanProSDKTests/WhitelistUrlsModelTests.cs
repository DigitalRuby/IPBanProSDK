using System;

using DigitalRuby.IPBanProSDK.Model.WhitelistUrls;

using NUnit.Framework;

namespace DigitalRuby.IPBanProSDKTests;

[TestFixture]
public class WhitelistUrlsModelTests
{
    [Test]
    public void AllRequestResponseTypes_AreInstantiable()
    {
        // exercise constructors / auto-properties so coverlet sees them
        var url = new WhitelistUrl("id", "hash", "exp", TimeSpan.FromHours(1), 5, null, "n");
        url.IPAddresses.Add(new UrlIPAddress("1.2.3.4", "exp", "note"));
        Assert.That(url.IPAddresses.Count, Is.EqualTo(1));

        _ = new GetUrlsRequest { Id = "x" };
        _ = new GetUrlsResponse();
        _ = new UpsertUrlRequest();
        _ = new UpsertUrlResponse();
        _ = new DeleteUrlRequest();
        _ = new DeleteUrlResponse();
        _ = new ConsumeUrlRequest { Id = "x", GroupId = "y" };
        _ = new ConsumeUrlResponse();
    }
}
