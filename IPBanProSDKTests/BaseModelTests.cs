using DigitalRuby.IPBanProSDK;

using NUnit.Framework;

namespace DigitalRuby.IPBanProSDKTests;

[TestFixture]
public class BaseModelTests
{
    [Test]
    public void DefaultsAndProperties()
    {
        var m = new BaseModel();
        Assert.That(m.Error, Is.False);
        Assert.That(m.Message, Is.Null);
        m.Error = true;
        m.Message = "x";
        Assert.That(m.Error, Is.True);
        Assert.That(m.Message, Is.EqualTo("x"));
    }
}
