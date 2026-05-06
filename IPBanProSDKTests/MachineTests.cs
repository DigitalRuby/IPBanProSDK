using DigitalRuby.IPBanProSDK;

using NUnit.Framework;

namespace DigitalRuby.IPBanProSDKTests;

[TestFixture]
public class MachineTests
{
    [Test]
    public void Alias_GetSet_RoundTripsThroughProps()
    {
        var m = new Machine();
        Assert.That(m.Alias, Is.Null);
        m.Alias = "my-name";
        Assert.That(m.Alias, Is.EqualTo("my-name"));
    }

    [Test]
    public void EmailAddressesCollection_Splits()
    {
        var m = new Machine { EmailAddresses = "a@b.com,c@d.com" };
        Assert.That(m.EmailAddressesCollection.Count, Is.EqualTo(2));
    }

    [Test]
    public void Props_Setter_RoundTripsThroughBackingField()
    {
        var m = new Machine();
        m.Props = "{\"x\":1}";
        Assert.That(m.Props, Is.EqualTo("{\"x\":1}"));
    }
}
