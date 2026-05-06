using DigitalRuby.IPBanProSDK;

using NUnit.Framework;

namespace DigitalRuby.IPBanProSDKTests;

[TestFixture]
public class IPAddressAsnNamesListModelTests
{
    [Test]
    public void IPAddressAsnName_StructEqualityToStringAndOperators()
    {
        var a = new IPAddressAsnName { Id = 1, Name = "Google", LanguageCode = "en" };
        var b = new IPAddressAsnName { Id = 1, Name = "Google", LanguageCode = "en" };
        var c = new IPAddressAsnName { Id = 2, Name = "Other", LanguageCode = "en" };

        Assert.That(a.ToString(), Is.Not.Null);
        Assert.That(a.Equals(b), Is.True);
        Assert.That(a.Equals(c), Is.False);
        Assert.That(a.Equals("nope"), Is.False);
        Assert.That(a == b, Is.True);
        Assert.That(a != c, Is.True);
        Assert.That(a.GetHashCode(), Is.EqualTo(b.GetHashCode()));
    }
}
