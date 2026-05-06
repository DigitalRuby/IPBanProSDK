using DigitalRuby.IPBanProSDK;

using NUnit.Framework;

namespace DigitalRuby.IPBanProSDKTests;

[TestFixture]
public class IPAddressCountryNamesListModelTests
{
    [Test]
    public void IPAddressCountryName_StructEqualityToStringAndOperators()
    {
        var a = new IPAddressCountryName { Id = 1, Name = "US", CountryCode = "US", LanguageCode = "en" };
        var b = new IPAddressCountryName { Id = 1, Name = "US", CountryCode = "US", LanguageCode = "en" };
        var c = new IPAddressCountryName { Id = 2, Name = "FR", CountryCode = "FR", LanguageCode = "en" };

        Assert.That(a.ToString(), Does.Contain("US"));
        Assert.That(a.Equals(b), Is.True);
        Assert.That(a.Equals(c), Is.False);
        Assert.That(a.Equals("nope"), Is.False);
        Assert.That(a == b, Is.True);
        Assert.That(a != c, Is.True);
        Assert.That(a.GetHashCode(), Is.EqualTo(b.GetHashCode()));
    }
}
