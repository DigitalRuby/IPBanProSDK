using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using SdkClientWebSocket = DigitalRuby.IPBanProSDK.ClientWebSocket;

namespace DigitalRuby.IPBanProSDKTests;

/// <summary>
/// In-memory IClientWebSocketImplementation for driving ClientWebSocket through its lifecycle in tests.
/// </summary>
internal sealed class FakeClientWebSocket : SdkClientWebSocket.IClientWebSocketImplementation
{
    private readonly ConcurrentQueue<(WebSocketMessageType type, byte[] data, bool endOfMessage)> incoming = new();
    private readonly ManualResetEventSlim hasIncoming = new(false);

    public WebSocketState State { get; set; } = WebSocketState.None;
    public TimeSpan KeepAliveInterval { get; set; }

    public List<(WebSocketMessageType type, byte[] data)> Sent { get; } = new();
    public Uri ConnectedUri { get; private set; }
    public bool Disposed { get; private set; }
    public bool ConnectAsyncCalled { get; private set; }
    public Func<Uri, Task> OnConnect { get; set; }
    public Exception ConnectThrows { get; set; }
    public Exception SendThrows { get; set; }

    public Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
    {
        State = WebSocketState.Closed;
        return Task.CompletedTask;
    }

    public Task CloseOutputAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
    {
        State = WebSocketState.Closed;
        return Task.CompletedTask;
    }

    public async Task ConnectAsync(Uri uri, CancellationToken cancellationToken)
    {
        ConnectAsyncCalled = true;
        ConnectedUri = uri;
        if (ConnectThrows is not null)
        {
            throw ConnectThrows;
        }
        if (OnConnect is not null)
        {
            await OnConnect(uri);
        }
        State = WebSocketState.Open;
    }

    public async Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken)
    {
        // wait up to 200ms for an incoming message; otherwise return a small idle
        while (!cancellationToken.IsCancellationRequested && State == WebSocketState.Open)
        {
            if (incoming.TryDequeue(out var item))
            {
                int len = Math.Min(item.data.Length, buffer.Count);
                Buffer.BlockCopy(item.data, 0, buffer.Array, buffer.Offset, len);
                return new WebSocketReceiveResult(len, item.type, item.endOfMessage);
            }
            try
            {
                await Task.Delay(20, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }
        cancellationToken.ThrowIfCancellationRequested();
        return new WebSocketReceiveResult(0, WebSocketMessageType.Close, true);
    }

    public Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
    {
        if (SendThrows is not null)
        {
            throw SendThrows;
        }
        var data = new byte[buffer.Count];
        Buffer.BlockCopy(buffer.Array, buffer.Offset, data, 0, buffer.Count);
        lock (Sent) { Sent.Add((messageType, data)); }
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        Disposed = true;
        State = WebSocketState.Closed;
    }

    public void EnqueueIncoming(WebSocketMessageType type, byte[] data, bool endOfMessage = true)
    {
        incoming.Enqueue((type, data, endOfMessage));
    }

    public void EnqueueText(string text) => EnqueueIncoming(WebSocketMessageType.Text, Encoding.UTF8.GetBytes(text));

    public void EnqueueBinary(byte[] data) => EnqueueIncoming(WebSocketMessageType.Binary, data);
}
