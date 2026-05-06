using DigitalRuby.IPBanProSDK;

using NUnit.Framework;

namespace DigitalRuby.IPBanProSDKTests;

[TestFixture]
public class PropHelperTests
{
    [Test]
    public void GetProp_NullProps_ReturnsDefault()
    {
        Assert.That(PropHelper.GetProp(null, "k"), Is.Null);
        Assert.That(PropHelper.GetProp<int>(null, "k"), Is.EqualTo(0));
    }

    [Test]
    public void GetProp_MissingKey_ReturnsDefault()
    {
        Assert.That(PropHelper.GetProp("{\"a\":\"1\"}", "missing"), Is.Null);
        Assert.That(PropHelper.GetProp<int>("{\"a\":\"1\"}", "missing"), Is.EqualTo(0));
    }

    [Test]
    public void SetAndGetString_RoundTrip()
    {
        var s = PropHelper.SetProp(null, "k", "v");
        Assert.That(s, Does.Contain("v"));
        Assert.That(PropHelper.GetProp(s, "k"), Is.EqualTo("v"));
    }

    [Test]
    public void SetProp_NullValue_RemovesAndCollapsesEmptyObject()
    {
        var s = PropHelper.SetProp("{\"a\":\"1\",\"b\":\"2\"}", "a", null);
        Assert.That(s, Does.Not.Contain("\"a\""));
        var s2 = PropHelper.SetProp("{\"a\":\"1\"}", "a", null);
        Assert.That(s2, Is.Null); // empty object becomes null
    }

    [Test]
    public void SetProp_NullPropsAndNullValue_ReturnsPropsUnchanged()
    {
        // covers the early return at the head of SetProp where both inputs are empty
        Assert.That(PropHelper.SetProp(null, "k", null), Is.Null);
        Assert.That(PropHelper.SetProp(string.Empty, "k", string.Empty), Is.EqualTo(string.Empty));
    }

    [Test]
    public void SetPropGeneric_ReturnsValue()
    {
        var v = PropHelper.SetProp("{}", "k", 42);
        Assert.That(v, Is.EqualTo(42));
    }

    [Test]
    public void GetPropTyped_ConvertibleValue_ReturnsConverted()
    {
        var s = PropHelper.SetProp(null, "n", "5");
        Assert.That(PropHelper.GetProp<int>(s, "n"), Is.EqualTo(5));
    }

    [Test]
    public void GetPropTyped_BadConvert_ReturnsDefault()
    {
        var s = PropHelper.SetProp(null, "n", "not-a-number");
        Assert.That(PropHelper.GetProp<int>(s, "n"), Is.EqualTo(0));
    }
}
