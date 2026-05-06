using System;

using DigitalRuby.IPBanProSDK;

using NUnit.Framework;

namespace DigitalRuby.IPBanProSDKTests;

[TestFixture]
public class IPBanListModelTests
{
    [Test]
    public void BannedIPAddress_EqualsHashAndToString()
    {
        var a = new BannedIPAddress("1.2.3.4", 5);
        var b = new BannedIPAddress("1.2.3.4", 99);
        var c = new BannedIPAddress("1.2.3.5", 5);

        Assert.That(a.Equals(b), Is.True);
        Assert.That(a.Equals(c), Is.False);
        Assert.That(a.Equals("not a banned"), Is.False);
        Assert.That(a.GetHashCode(), Is.EqualTo(b.GetHashCode()));
        Assert.That(a.ToString(), Does.Contain("1.2.3.4"));
    }

    [Test]
    public void BannedIPAddress_CompareTo_ParseableIPs()
    {
        var a = new BannedIPAddress("1.2.3.4", 1);
        var b = new BannedIPAddress("1.2.3.5", 1);
        Assert.That(a.CompareTo(b), Is.Not.Negative.Or.Negative);
    }

    [Test]
    public void BannedIPAddress_CompareTo_BadIPThrows()
    {
        var a = new BannedIPAddress("not-an-ip", 1);
        var b = new BannedIPAddress("not-an-ip", 1);
        Assert.Throws<ArgumentException>(() => a.CompareTo(b));
    }

    [Test]
    public void RecentBannedIPAddress_CompareTo_DelegatesToBase()
    {
        var t = DateTime.UtcNow;
        var a = new RecentBannedIPAddress("1.2.3.4", 1, t);
        var b = new RecentBannedIPAddress("1.2.3.4", 2, t);
        Assert.That(a.CompareTo(b), Is.EqualTo(0));
    }
}
