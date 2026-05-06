using DigitalRuby.IPBanProSDK;

using NUnit.Framework;

namespace DigitalRuby.IPBanProSDKTests;

[TestFixture]
public class IPAddressGeographyModelTests
{
    [Test]
    public void ToString_PullsFromGeography()
    {
        var m = new IPAddressGeographyModel
        {
            IPAddress = "1.2.3.4",
            Geography = new IPAddressGeography { City = "Springfield", Region = "OR", Country = "US", ISP = "Acme" },
        };
        var s = m.ToString();
        Assert.That(s, Does.Contain("1.2.3.4"));
        Assert.That(s, Does.Contain("Springfield"));
    }
}
