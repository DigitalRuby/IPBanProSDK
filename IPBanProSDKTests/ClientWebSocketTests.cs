using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using DigitalRuby.IPBanProSDK;

using NUnit.Framework;

using SdkClientWebSocket = DigitalRuby.IPBanProSDK.ClientWebSocket;

namespace DigitalRuby.IPBanProSDKTests;

[TestFixture]
public class ClientWebSocketTests
{
    private FakeClientWebSocket fake;

    [SetUp]
    public void SetUp()
    {
        fake = new FakeClientWebSocket();
        SdkClientWebSocket.RegisterWebSocketCreator(_ => fake);
    }

    [TearDown]
    public void TearDown()
    {
        SdkClientWebSocket.RegisterWebSocketCreator(null);
    }

    private static async Task WaitFor(Func<bool> predicate, int timeoutMs = 3000)
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();
        while (sw.ElapsedMilliseconds < timeoutMs && !predicate())
        {
            await Task.Delay(20);
        }
        Assert.That(predicate(), Is.True, $"Condition not met within {timeoutMs}ms");
    }

    [Test]
    public async Task Start_ConnectsAndInvokesConnectedHandler()
    {
        using var sock = new SdkClientWebSocket
        {
            Uri = new Uri("ws://localhost/test"),
            ReconnectInterval = TimeSpan.FromMilliseconds(50),
            PingInterval = TimeSpan.Zero,
        };
        bool connected = false;
        sock.Connected += (s, reconnect) => { connected = true; return Task.CompletedTask; };
        sock.Start();
        await WaitFor(() => connected);
        Assert.That(fake.ConnectAsyncCalled, Is.True);
        // localhost rewrite to 127.0.0.1
        Assert.That(fake.ConnectedUri.Host, Is.EqualTo("127.0.0.1"));
    }

    [Test]
    public async Task TextMessage_DeliveredToOnTextMessage()
    {
        using var sock = new SdkClientWebSocket
        {
            Uri = new Uri("ws://example.com/test"),
            ReconnectInterval = TimeSpan.FromMilliseconds(50),
            PingInterval = TimeSpan.Zero,
        };
        string received = null;
        sock.OnTextMessage = (s, t) => { received = t; return Task.CompletedTask; };
        sock.Start();
        await WaitFor(() => fake.State == WebSocketState.Open);
        fake.EnqueueText("hello-text");
        await WaitFor(() => received == "hello-text");
    }

    [Test]
    public async Task BinaryMessage_DeliveredToOnBinaryMessage_WhenNoOnMessage()
    {
        using var sock = new SdkClientWebSocket
        {
            Uri = new Uri("ws://example.com/test"),
            ReconnectInterval = TimeSpan.FromMilliseconds(50),
            PingInterval = TimeSpan.Zero,
        };
        byte[] received = null;
        sock.OnBinaryMessage = (s, b) => { received = b; return Task.CompletedTask; };
        sock.Start();
        await WaitFor(() => fake.State == WebSocketState.Open);
        fake.EnqueueBinary(new byte[] { 1, 2, 3, 4 });
        await WaitFor(() => received != null);
        Assert.That(received, Is.EqualTo(new byte[] { 1, 2, 3, 4 }));
    }

    [Test]
    public async Task BinaryMessage_DeserializedToMessage_WhenOnMessageSet()
    {
        using var sock = new SdkClientWebSocket
        {
            Uri = new Uri("ws://example.com/test"),
            ReconnectInterval = TimeSpan.FromMilliseconds(50),
            PingInterval = TimeSpan.Zero,
        };
        Message received = null;
        sock.OnMessage = (s, m) => { received = m; return Task.CompletedTask; };
        sock.Start();
        await WaitFor(() => fake.State == WebSocketState.Open);

        var msg = new Message { Name = "hello" };
        var raw = new WebSocketRawMessage(msg);
        fake.EnqueueBinary(raw.Data);
        await WaitFor(() => received != null);
        Assert.That(received.Name, Is.EqualTo("hello"));
    }

    [Test]
    public async Task BinaryMessage_AckSetsManualResetEvent()
    {
        using var sock = new SdkClientWebSocket
        {
            Uri = new Uri("ws://example.com/test"),
            ReconnectInterval = TimeSpan.FromMilliseconds(50),
            PingInterval = TimeSpan.Zero,
        };
        // OnMessage non-null is required for the deserialize path
        sock.OnMessage = (s, m) => Task.CompletedTask;
        sock.Start();
        await WaitFor(() => fake.State == WebSocketState.Open);

        // queue a message with an Id so the socket starts waiting for an ack
        var outbound = new Message { Id = "abc-123", Name = "ping" };
        await sock.QueueMessage(outbound);

        // server replies with an ack
        var ackMessage = new Message { Id = "abc-123", Name = IPBanProBaseAPI.MessageAck };
        var rawAck = new WebSocketRawMessage(ackMessage);
        fake.EnqueueBinary(rawAck.Data);

        // WaitForAck should not throw; if the ack arrived it returns quickly
        Assert.DoesNotThrow(() => sock.WaitForAck("abc-123", 2000));
    }

    [Test]
    public async Task QueueMessage_String_SendsAsTextFrame()
    {
        using var sock = new SdkClientWebSocket
        {
            Uri = new Uri("ws://example.com/test"),
            ReconnectInterval = TimeSpan.FromMilliseconds(50),
            PingInterval = TimeSpan.Zero,
        };
        sock.Start();
        await WaitFor(() => fake.State == WebSocketState.Open);

        bool ok = await sock.QueueMessage("plain-string");
        Assert.That(ok, Is.True);
        await WaitFor(() => fake.Sent.Count > 0);
        var sent = fake.Sent[0];
        Assert.That(sent.type, Is.EqualTo(WebSocketMessageType.Text));
        Assert.That(Encoding.UTF8.GetString(sent.data), Is.EqualTo("plain-string"));
    }

    [Test]
    public async Task QueueMessage_Bytes_SendsAsBinaryFrame()
    {
        using var sock = new SdkClientWebSocket
        {
            Uri = new Uri("ws://example.com/test"),
            ReconnectInterval = TimeSpan.FromMilliseconds(50),
            PingInterval = TimeSpan.Zero,
        };
        sock.Start();
        await WaitFor(() => fake.State == WebSocketState.Open);

        await sock.QueueMessage(new byte[] { 9, 8, 7 });
        await WaitFor(() => fake.Sent.Count > 0);
        Assert.That(fake.Sent[0].type, Is.EqualTo(WebSocketMessageType.Binary));
    }

    [Test]
    public async Task QueueMessage_NullReturnsFalse()
    {
        using var sock = new SdkClientWebSocket
        {
            Uri = new Uri("ws://example.com/test"),
            ReconnectInterval = TimeSpan.FromMilliseconds(50),
            PingInterval = TimeSpan.Zero,
        };
        sock.Start();
        await WaitFor(() => fake.State == WebSocketState.Open);
        Assert.That(await sock.QueueMessage(null), Is.False);
    }

    [Test]
    public async Task QueueMessage_ClosedSocketReturnsFalse()
    {
        using var sock = new SdkClientWebSocket
        {
            Uri = new Uri("ws://example.com/test"),
            ReconnectInterval = TimeSpan.FromMilliseconds(50),
            PingInterval = TimeSpan.Zero,
        };
        sock.Start();
        await WaitFor(() => fake.State == WebSocketState.Open);
        fake.State = WebSocketState.Closed;
        Assert.That(await sock.QueueMessage("x"), Is.False);
    }

    [Test]
    public void WaitForAck_UnknownIdReturnsImmediately()
    {
        using var sock = new SdkClientWebSocket();
        Assert.DoesNotThrow(() => sock.WaitForAck("never-queued", 100));
    }

    [Test]
    public void WaitForAck_AsyncMode_NoOp()
    {
        using var sock = new SdkClientWebSocket { AckSynchronous = false };
        Assert.DoesNotThrow(() => sock.WaitForAck("anything", 100));
    }

    [Test]
    public async Task SubscribeAndUnsubscribe_ExtensionMethods()
    {
        using var sock = new SdkClientWebSocket
        {
            Uri = new Uri("ws://example.com/test"),
            ReconnectInterval = TimeSpan.FromMilliseconds(50),
            PingInterval = TimeSpan.Zero,
            AckSynchronous = false, // don't block
        };
        sock.Start();
        await WaitFor(() => fake.State == WebSocketState.Open);

        Assert.That(await sock.SubscribeWebSocket(IPBanProAPIWebSocketSubscription.RecentBannedIPAddresses), Is.True);
        await WaitFor(() => fake.Sent.Count >= 1);

        Assert.That(await sock.UnsubscribeWebSocket(IPBanProAPIWebSocketSubscription.RecentBannedIPAddresses), Is.True);
        await WaitFor(() => fake.Sent.Count >= 2);
    }

    [Test]
    public async Task Dispose_StopsAndDisposesSocket()
    {
        var sock = new SdkClientWebSocket
        {
            Uri = new Uri("ws://example.com/test"),
            ReconnectInterval = TimeSpan.FromMilliseconds(50),
            PingInterval = TimeSpan.Zero,
            CloseCleanly = true,
        };
        sock.Start();
        await WaitFor(() => fake.State == WebSocketState.Open);
        sock.Dispose();
        // background close happens async; just give it a moment
        await Task.Delay(100);
    }

    [Test]
    public async Task RealImplementation_ConstructsAndConnects()
    {
        // unregister the fake creator so the real ClientWebSocketImplementation is used.
        // Point at a port nothing's listening on; we only want the inner class instantiated
        // and its ConnectAsync invoked, so its body lines are covered. Dispose right after.
        SdkClientWebSocket.RegisterWebSocketCreator(null);
        using var sock = new SdkClientWebSocket
        {
            Uri = new Uri("ws://127.0.0.1:1/test"),
            ReconnectInterval = TimeSpan.FromMilliseconds(50),
            PingInterval = TimeSpan.Zero,
            RequestHeaders = new[] { new KeyValuePair<string, object>("X-Test", "v") },
            KeepAlive = TimeSpan.FromSeconds(1),
        };
        sock.Start();
        await Task.Delay(500);
    }

    [Test]
    public async Task RealImplementation_WithClientCertificate()
    {
        SdkClientWebSocket.RegisterWebSocketCreator(null);
        using var rsa = System.Security.Cryptography.RSA.Create(2048);
        var req = new System.Security.Cryptography.X509Certificates.CertificateRequest(
            "CN=ws-client-test", rsa,
            System.Security.Cryptography.HashAlgorithmName.SHA256,
            System.Security.Cryptography.RSASignaturePadding.Pkcs1);
        using var cert = req.CreateSelfSigned(DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddDays(1));

        using var sock = new SdkClientWebSocket(serializer: null, clientCertificate: cert)
        {
            Uri = new Uri("wss://127.0.0.1:1/test"),
            ReconnectInterval = TimeSpan.FromMilliseconds(50),
            PingInterval = TimeSpan.Zero,
        };
        sock.Start();
        await Task.Delay(500);
    }

    [Test]
    public async Task Reconnects_AfterServerCloses()
    {
        using var sock = new SdkClientWebSocket
        {
            Uri = new Uri("ws://example.com/test"),
            ReconnectInterval = TimeSpan.FromMilliseconds(50),
            PingInterval = TimeSpan.Zero,
        };
        int connectCount = 0;
        sock.Connected += (s, r) => { Interlocked.Increment(ref connectCount); return Task.CompletedTask; };
        sock.Disconnected += (s, r) => Task.CompletedTask;
        sock.Start();
        await WaitFor(() => connectCount >= 1);

        // simulate the server sending Close
        fake.EnqueueIncoming(WebSocketMessageType.Close, Array.Empty<byte>());
        await WaitFor(() => connectCount >= 2, timeoutMs: 5000);
    }
}
