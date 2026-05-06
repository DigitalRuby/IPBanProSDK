using DigitalRuby.IPBanProSDK;

using NUnit.Framework;

namespace DigitalRuby.IPBanProSDKTests;

[TestFixture]
public class FailedLoginAttemptTests
{
    [Test]
    public void ToString_IncludesUserNameSourceAndFqdn()
    {
        var f = new FailedLoginAttempt
        {
            Machine = new Machine { FQDN = "host" },
            UserName = "alice",
            Source = "RDP",
        };
        var s = f.ToString();
        Assert.That(s, Does.Contain("alice"));
        Assert.That(s, Does.Contain("RDP"));
        Assert.That(s, Does.Contain("host"));
    }
}
