using DigitalRuby.IPBanProSDK;

using NUnit.Framework;

namespace DigitalRuby.IPBanProSDKTests;

[TestFixture]
public class SuccessLoginAttemptTests
{
    [Test]
    public void ToString_IncludesUserNameSourceAndFqdn()
    {
        var s = new SuccessLoginAttempt
        {
            Machine = new Machine { FQDN = "host" },
            UserName = "alice",
            Source = "RDP",
        };
        var str = s.ToString();
        Assert.That(str, Does.Contain("alice"));
        Assert.That(str, Does.Contain("RDP"));
        Assert.That(str, Does.Contain("host"));
    }
}
