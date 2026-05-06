using System.Collections.Generic;

using DigitalRuby.IPBanProSDK;

using NUnit.Framework;

namespace DigitalRuby.IPBanProSDKTests;

[TestFixture]
public class MessageTests
{
    [Test]
    public void ToString_IncludesIdNameAndParameters()
    {
        var m = new Message
        {
            Id = "id",
            Name = "n",
            Parameters = new List<KeyValuePair<string, object>>
            {
                new("k1", "v1"),
                new("k2", 2),
            },
        };
        var s = m.ToString();
        Assert.That(s, Does.Contain("id"));
        Assert.That(s, Does.Contain("n"));
        Assert.That(s, Does.Contain("k1"));
    }

    [Test]
    public void ToString_NullParameters_StillPrintsHeader()
    {
        var m = new Message { Id = "id", Name = "n" };
        Assert.That(m.ToString(), Does.Contain("Parameters: "));
    }
}
