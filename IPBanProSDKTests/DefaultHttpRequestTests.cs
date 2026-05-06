using System;

using DigitalRuby.IPBanProSDK;

using NUnit.Framework;

namespace DigitalRuby.IPBanProSDKTests;

[TestFixture]
public class DefaultHttpRequestTests
{
    [Test]
    public void Properties_RoundTrip()
    {
        var r = new DefaultHttpRequest
        {
            Uri = new Uri("https://example.com"),
            RemotePort = 443,
        };
        r.Items["x"] = 1;
        Assert.That(r.Uri.Host, Is.EqualTo("example.com"));
        Assert.That(r.RemotePort, Is.EqualTo(443));
        Assert.That(r.Items["x"], Is.EqualTo(1));
    }
}
