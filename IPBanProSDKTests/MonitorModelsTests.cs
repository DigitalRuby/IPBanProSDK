using DigitalRuby.IPBanProSDK;

using NUnit.Framework;

namespace DigitalRuby.IPBanProSDKTests;

[TestFixture]
public class MonitorModelsTests
{
    [Test]
    public void MachineNamesModel_CreateMachineName_NoAlias_UsesFqdn()
    {
        Assert.That(MachineNamesModel.CreateMachineName("fqdn", "1.2.3.4"), Is.EqualTo("fqdn-1.2.3.4"));
    }

    [Test]
    public void MachineNamesModel_CreateMachineName_WithAlias_UsesAlias()
    {
        Assert.That(MachineNamesModel.CreateMachineName("fqdn", "1.2.3.4", "alias"), Is.EqualTo("alias-1.2.3.4"));
    }

    [Test]
    public void MachineName_CompareTo_OrdersByName()
    {
        var a = new MachineNamesModel.MachineName("alpha", "v");
        var b = new MachineNamesModel.MachineName("beta", "v");
        Assert.That(a.CompareTo(b), Is.LessThan(0));
    }
}
