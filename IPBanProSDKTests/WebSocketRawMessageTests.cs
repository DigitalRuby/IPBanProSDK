using System.Net.WebSockets;
using System.Text;

using DigitalRuby.IPBanProSDK;

using NUnit.Framework;

namespace DigitalRuby.IPBanProSDKTests;

[TestFixture]
public class WebSocketRawMessageTests
{
    [Test]
    public void StringData_ProducesTextFrame()
    {
        var raw = new WebSocketRawMessage(new Message { Data = "hi-there" });
        Assert.That(raw.MessageType, Is.EqualTo(WebSocketMessageType.Text));
        Assert.That(Encoding.UTF8.GetString(raw.Data), Is.EqualTo("hi-there"));
    }

    [Test]
    public void NonStringData_ProducesBinaryFrame()
    {
        var raw = new WebSocketRawMessage(new Message { Name = "x", Data = new { A = 1 } });
        Assert.That(raw.MessageType, Is.EqualTo(WebSocketMessageType.Binary));
        Assert.That(raw.Data, Is.Not.Null.And.Not.Empty);
    }
}
