using DigitalRuby.IPBanProSDK;

using NUnit.Framework;

namespace DigitalRuby.IPBanProSDKTests;

[TestFixture]
public class NotificationModelTests
{
    [Test]
    public void ToString_JoinsToAddressesWithSubject()
    {
        var n = new Notification
        {
            Subject = "Subj",
            Body = "Body {0}",
            ToAddresses = new[] { "x@y.com", "a@b.com" },
        };
        Assert.That(n.ToString(), Does.Contain("Subj"));
        Assert.That(n.ToString(), Does.Contain("x@y.com"));
    }

    [Test]
    public void ToString_NullToAddresses_DoesNotThrow()
    {
        var n = new Notification { Subject = "S" };
        Assert.That(n.ToString(), Does.Contain("S"));
    }

    [Test]
    public void FormatString_ValidFormat_ReturnsFormatted()
    {
        Assert.That(Notification.FormatString("ctx", "hello {0}", "world"), Is.EqualTo("hello world"));
    }

    [Test]
    public void FormatString_BadFormat_ReturnsNull()
    {
        Assert.That(Notification.FormatString("ctx", "{99}", "x"), Is.Null);
    }
}
