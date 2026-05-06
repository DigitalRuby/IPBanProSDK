using DigitalRuby.IPBanProSDK;

using NUnit.Framework;

namespace DigitalRuby.IPBanProSDKTests;

[TestFixture]
public class SettingsUpdateModelTests
{
    [Test]
    public void GetHashCode_IsDeterministicAndIgnoresMessageError()
    {
        var m1 = new SettingsUpdateModel { Message = "x", Error = true, BanTime = "01:00:00" };
        var m2 = new SettingsUpdateModel { Message = "y", Error = false, BanTime = "01:00:00" };
        Assert.That(m1.GetHashCode(), Is.EqualTo(m2.GetHashCode()));
        // Message and Error should be restored after computing the hash
        Assert.That(m1.Message, Is.EqualTo("x"));
        Assert.That(m1.Error, Is.True);
    }
}
