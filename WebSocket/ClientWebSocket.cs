/*

IPBanPro SDK - https://ipban.com | https://github.com/DigitalRuby/IPBanProSDK
IPBan and IPBan Pro Copyright(c) 2012 Digital Ruby, LLC
support@digitalruby.com

The MIT License(MIT)

Copyright(c) 2012 Digital Ruby, LLC

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

*/

#region Imports

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using DigitalRuby.IPBanCore;

#endregion Imports

namespace DigitalRuby.IPBanProSDK
{
    /// <summary>
    /// Wraps a client web socket for easy dispose later, along with auto-reconnect and message and reader queues
    /// </summary>
    public sealed class ClientWebSocket : IQueueMessage
    {
        /// <summary>
        /// Client web socket implementation
        /// </summary>
        public interface IClientWebSocketImplementation : IDisposable
        {
            /// <summary>
            /// Web socket state
            /// </summary>
            WebSocketState State { get; }

            /// <summary>
            /// Keep alive interval (heartbeat)
            /// </summary>
            TimeSpan KeepAliveInterval { get; set; }

            /// <summary>
            /// Close cleanly
            /// </summary>
            /// <param name="closeStatus">Status</param>
            /// <param name="statusDescription">Description</param>
            /// <param name="cancellationToken">Cancel token</param>
            /// <returns>Task</returns>
            Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken);

            /// <summary>
            /// Close output immediately
            /// </summary>
            /// <param name="closeStatus">Status</param>
            /// <param name="statusDescription">Description</param>
            /// <param name="cancellationToken">Cancel token</param>
            /// <returns>Task</returns>
            Task CloseOutputAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken);

            /// <summary>
            /// Connect
            /// </summary>
            /// <param name="uri">Uri</param>
            /// <param name="cancellationToken">Cancel token</param>
            /// <returns>Task</returns>
            Task ConnectAsync(Uri uri, CancellationToken cancellationToken);

            /// <summary>
            /// Receive
            /// </summary>
            /// <param name="buffer">Buffer</param>
            /// <param name="cancellationToken">Cancel token</param>
            /// <returns>Result</returns>
            Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken);

            /// <summary>
            /// Send
            /// </summary>
            /// <param name="buffer">Buffer</param>
            /// <param name="messageType">Message type</param>
            /// <param name="endOfMessage">True if end of message, false otherwise</param>
            /// <param name="cancellationToken">Cancel token</param>
            /// <returns>Task</returns>
            Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken);
        }

        private class ClientWebSocketImplementation : IClientWebSocketImplementation
        {
            private readonly System.Net.WebSockets.ClientWebSocket webSocket = new System.Net.WebSockets.ClientWebSocket();

            public WebSocketState State
            {
                get { return webSocket.State; }
            }

            public TimeSpan KeepAliveInterval
            {
                get { return webSocket.Options.KeepAliveInterval; }
                set { webSocket.Options.KeepAliveInterval = value; }
            }

            public ClientWebSocketImplementation(IEnumerable<KeyValuePair<string, object>> requestHeaders)
            {
                foreach (KeyValuePair<string, object> header in requestHeaders)
                {
                    webSocket.Options.SetRequestHeader(header.Key, header.Value.ToHttpHeaderString());
                }
            }

            public void Dispose()
            {
                webSocket.Dispose();
            }

            public Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
            {
                return webSocket.CloseAsync(closeStatus, statusDescription, cancellationToken);
            }

            public Task CloseOutputAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
            {
                return webSocket.CloseOutputAsync(closeStatus, statusDescription, cancellationToken);
            }

            public Task ConnectAsync(Uri uri, CancellationToken cancellationToken)
            {
                return webSocket.ConnectAsync(uri, cancellationToken);
            }

            public Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken)
            {
                return webSocket.ReceiveAsync(buffer, cancellationToken);
            }

            public Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
            {
                return webSocket.SendAsync(buffer, messageType, endOfMessage, cancellationToken);
            }
        }

        private const int receiveChunkSize = 8192;

        private readonly AsyncQueue<object> messageQueue = new AsyncQueue<object>();
        private readonly Dictionary<string, ManualResetEvent> acks = new Dictionary<string, ManualResetEvent>();
        private readonly ISerializer serializer;

        private static Func<IEnumerable<KeyValuePair<string, object>>, IClientWebSocketImplementation> webSocketCreator;

        // created from factory, allows swapping out underlying implementation
        private IClientWebSocketImplementation webSocket;
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private bool firstConnect = true;
        private bool disposed;

        private void CreateWebSocket()
        {
            if (webSocketCreator is null)
            {
                webSocket = new ClientWebSocketImplementation(RequestHeaders ?? emptyRequestHeaders);
            }
            else
            {
                webSocket = webSocketCreator(RequestHeaders ?? emptyRequestHeaders);
            }
        }

        /// <summary>
        /// The uri to connect to
        /// </summary>
        public Uri Uri { get; set; }

        /// <summary>
        /// Amount to wait between dropped connections for a reconnect attempt, default is 5 seconds
        /// </summary>
        public TimeSpan ReconnectInterval { get; set; } = TimeSpan.FromSeconds(5.0);

        /// <summary>
        /// Action to handle incoming text messages. If null, text messages are handled with OnBinaryMessage.
        /// </summary>
        public Func<IQueueMessage, string, Task> OnTextMessage { get; set; }

        /// <summary>
        /// Action to handle incoming binary messages
        /// </summary>
        public Func<IQueueMessage, byte[], Task> OnBinaryMessage { get; set; }

        /// <summary>
        /// Action to handle incoming parsed messages
        /// </summary>
        public Func<IQueueMessage, Message, Task> OnMessage { get; set; }

        /// <summary>
        /// Interval to call connect at regularly (default is 1 hour)
        /// </summary>
        public TimeSpan ConnectInterval { get; set; } = TimeSpan.FromHours(1.0);

        /// <summary>
        /// Keep alive interval (default is 30 seconds)
        /// </summary>
        public TimeSpan KeepAlive { get; set; } = TimeSpan.FromSeconds(30.0);

        private static readonly IEnumerable<KeyValuePair<string, object>> emptyRequestHeaders = new KeyValuePair<string, object>[0];
        /// <summary>
        /// Optional request headers to send with the connect request
        /// </summary>
        public IEnumerable<KeyValuePair<string, object>> RequestHeaders { get; set; }

        /// <summary>
        /// Allows additional listeners for connect event
        /// </summary>
        public event WebSocketConnectionDelegate Connected;

        /// <summary>
        /// Allows additional listeners for disconnect event
        /// </summary>
        public event WebSocketConnectionDelegate Disconnected;

        /// <summary>
        /// Whether to close the connection gracefully, this can cause the close to take longer.
        /// </summary
        public bool CloseCleanly { get; set; }

        /// <summary>
        /// Whether ack is async or synchronous. By default they are synchronous, meaning the socket will block after sending a message with an id until an ack has been received.
        /// For asynchronous ack mode, the socket will not block and the event handler will need to check for ack messages with the correct id and perform appropriate processing.
        /// </summary>
        public bool AckSynchronous { get; set; } = true;

        /// <summary>
        /// Register a function that will be responsible for creating the underlying web socket implementation
        /// By default, C# built-in web sockets are used (Windows 8.1+ required). But you could swap out
        /// a different web socket for other platforms, testing, or other specialized needs.
        /// Creator takes a key/value pair list for extra http request headers.
        /// </summary>
        /// <param name="creator">Creator function. Pass null to go back to the default implementation.</param>
        public static void RegisterWebSocketCreator(Func<IEnumerable<KeyValuePair<string, object>>, IClientWebSocketImplementation> creator)
        {
            webSocketCreator = creator;
        }

        /// <summary>
        /// Default constructor, does not begin listening immediately. You must set the properties and then call Start.
        /// </summary>
        /// <param name="serializer">Serializer, null for default serializer</param>
        public ClientWebSocket(ISerializer serializer = null)
        {
            this.serializer = (serializer ?? DefaultSerializer.Instance);
        }

        /// <summary>
        /// Start the web socket listening and processing
        /// </summary>
        public void Start()
        {
            CreateWebSocket();

            // kick off message parser and message listener
            Task.Run(MessageTask);
            Task.Run(ReadTask);
        }

        /// <summary>
        /// Close and dispose of all resources, stops the web socket and shuts it down.
        /// </summary>
        public void Dispose()
        {
            if (!disposed)
            {
                disposed = true;
                cancellationTokenSource.Cancel();
                Task.Run(async () =>
                {
                    try
                    {
                        if (webSocket.State == WebSocketState.Open)
                        {
                            if (CloseCleanly)
                            {
                                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Dispose", default);
                            }
                            else
                            {
                                await webSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "Dispose", default);
                            }
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        // dont care
                    }
                    catch (Exception ex)
                    {
                        IPBanCore.Logger.Info(ex.ToString());
                    }
                });
            }
        }

        /// <summary>
        /// Queue a message to the WebSocket server, it will be sent as soon as possible.
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="groupId">Group id, ignored for now</param>
        /// <returns>True if success, false if error</returns>
        public bool QueueMessage(Message message, int groupId = IPBanProBaseAPI.WebSocketGroupIdNone)
        {
            if (webSocket is null || webSocket.State == WebSocketState.Closed || webSocket.State == WebSocketState.CloseReceived || message is null)
            {
                return false;
            }

            string id = null;
            if (AckSynchronous && !string.IsNullOrWhiteSpace(message.Id))
            {
                id = message.Id;
                lock (acks)
                {
                    acks.Add(id, new ManualResetEvent(false));
                }
            }

            async Task enqueueCallback(IQueueMessage socket)
            {
                try
                {
                    WebSocketRawMessage rawMessage = new WebSocketRawMessage(message, serializer);
                    ArraySegment<byte> messageArraySegment = new ArraySegment<byte>(rawMessage.Data);
                    await webSocket.SendAsync(messageArraySegment, rawMessage.MessageType, true, cancellationTokenSource.Token);
                }
                catch
                {
                    // failed to send, remove the ack
                    if (!string.IsNullOrWhiteSpace(id))
                    {
                        try
                        {
                            lock (acks)
                            {
                                if (acks.TryGetValue(id, out ManualResetEvent evt))
                                {
                                    evt.Set();
                                    acks.Remove(id);
                                }
                            }
                        }
                        catch
                        {
                        }
                    }
                    throw;
                }
            }

            QueueActions(enqueueCallback);
            return true;
        }

        /// <summary>
        /// Wait for an ack message, the Id field of an object will trigger an Ack if it is detected.
        /// If AckSynchronous is false, this method immediately returns.
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="timeoutMilliseconds">Timeout in milliseconds</param>
        /// <exception cref="System.TimeoutException">Timeout before ack</exception>
        public void WaitForAck(string id, int timeoutMilliseconds = 5000)
        {
            if (AckSynchronous && !string.IsNullOrWhiteSpace(id))
            {
                ManualResetEvent evt;
                lock (acks)
                {
                    if (!acks.TryGetValue(id, out evt))
                    {
                        // not waiting for an ack on this id or it was removed before we even got here
                        return;
                    }
                }
                if (!evt.WaitOne(timeoutMilliseconds))
                {
                    throw new TimeoutException("Timeout waiting for ack id " + id);
                }
            }
        }

        private void QueueActions(params Func<IQueueMessage, Task>[] actions)
        {
            if (actions != null && actions.Length != 0)
            {
                Func<IQueueMessage, Task>[] actionsCopy = actions;
                Func<Task> enqueueActionsFunc = async () =>
                {
                    foreach (var action in actionsCopy.Where(a => a != null))
                    {
                        try
                        {
                            await action.Invoke(this);
                        }
                        catch (Exception ex)
                        {
                            IPBanCore.Logger.Info(ex.ToString());
                        }
                    }
                };

                messageQueue.Enqueue(enqueueActionsFunc);
            }
        }

        private void QueueActionsWithNoExceptions(params Func<IQueueMessage, Task>[] actions)
        {
            if (actions != null && actions.Length != 0)
            {
                Func<IQueueMessage, Task>[] actionsCopy = actions;
                messageQueue.Enqueue((Func<Task>)(async () =>
                {
                    foreach (var action in actionsCopy.Where(a => a != null))
                    {
                        while (!disposed)
                        {
                            try
                            {
                                await action.Invoke(this);
                                break;
                            }
                            catch (Exception ex)
                            {
                                IPBanCore.Logger.Info(ex.ToString());
                            }
                        }
                    }
                }));
            }
        }

        private async Task InvokeConnected(IQueueMessage socket)
        {
            var connected = Connected;
            if (connected != null)
            {
                bool reconnect = !firstConnect;
                firstConnect = false;
                await connected.Invoke(socket, reconnect);
            }
        }

        private async Task InvokeDisconnected(IQueueMessage socket)
        {
            var disconnected = Disconnected;
            if (disconnected != null)
            {
                await disconnected.Invoke(this, false);
            }
        }

        private async Task ReadTask()
        {
            ArraySegment<byte> receiveBuffer = new ArraySegment<byte>(new byte[receiveChunkSize]);
            TimeSpan keepAlive = webSocket.KeepAliveInterval;
            MemoryStream stream = new MemoryStream();
            WebSocketReceiveResult result;
            bool wasConnected = false;

            while (!disposed)
            {
                try
                {
                    // open the socket
                    webSocket.KeepAliveInterval = KeepAlive;
                    wasConnected = false;
                    await webSocket.ConnectAsync(Uri, cancellationTokenSource.Token);
                    while (!disposed && webSocket.State == WebSocketState.Connecting)
                    {
                        await Task.Delay(20);
                    }
                    if (disposed || webSocket.State != WebSocketState.Open)
                    {
                        continue;
                    }
                    wasConnected = true;

                    // on connect may make additional calls that must succeed, such as rest calls
                    // for lists, etc.
                    QueueActionsWithNoExceptions(InvokeConnected);

                    while (webSocket.State == WebSocketState.Open)
                    {
                        do
                        {
                            result = await webSocket.ReceiveAsync(receiveBuffer, cancellationTokenSource.Token);
                            if (result != null)
                            {
                                if (result.MessageType == WebSocketMessageType.Close)
                                {
                                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, cancellationTokenSource.Token);
                                    QueueActions(InvokeDisconnected);
                                }
                                else
                                {
                                    stream.Write(receiveBuffer.Array, 0, result.Count);
                                }
                            }
                        }
                        while (result != null && !result.EndOfMessage);
                        if (stream.Length != 0)
                        {
                            try
                            {
                                // if text message and we are handling text messages
                                if (result.MessageType == WebSocketMessageType.Text)
                                {
                                    string text = Encoding.UTF8.GetString(stream.GetBuffer(), 0, (int)stream.Length);
                                    messageQueue.Enqueue(text);
                                }
                                // otherwise treat message as binary
                                else
                                {
                                    // make a copy of the bytes, the memory stream will be re-used and could potentially corrupt in multi-threaded environments
                                    // not using ToArray just in case it is making a slice/span from the internal bytes, we want an actual physical copy
                                    byte[] bytesCopy = new byte[stream.Length];
                                    Array.Copy(stream.GetBuffer(), bytesCopy, stream.Length);
                                    if (OnMessage != null)
                                    {
                                        Message message;
                                        try
                                        {
                                            message = serializer.Deserialize(bytesCopy, Message.Type) as Message;
                                        }
                                        catch (Exception ex)
                                        {
                                            IPBanCore.Logger.Error("Error decoding web socket message", ex);
                                            continue;
                                        }

                                        // remove acks
                                        if (AckSynchronous && !string.IsNullOrWhiteSpace(message.Id) && message.Name == IPBanProBaseAPI.MessageAck)
                                        {
                                            lock (acks)
                                            {
                                                if (acks.TryGetValue(message.Id, out ManualResetEvent evt))
                                                {
                                                    acks.Remove(message.Id);
                                                    evt.Set();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            messageQueue.Enqueue(message);
                                        }
                                    }
                                    else
                                    {
                                        messageQueue.Enqueue(bytesCopy);
                                    }
                                }
                            }
                            finally
                            {
                                stream.SetLength(0);
                            }
                        }
                        result = null;
                    }
                }
                catch (Exception ex)
                {
                    // we don't care about these exception types
                    if (!(ex is OperationCanceledException || ex is WebSocketException))
                    {
                        // eat exceptions, most likely a result of a disconnect, either way we will re-create the web socket
                        IPBanCore.Logger.Info(ex.ToString());
                    }
                }

                if (wasConnected)
                {
                    QueueActions(InvokeDisconnected);
                }
                try
                {
                    webSocket.Dispose();
                }
                catch (Exception ex)
                {
                    IPBanCore.Logger.Info(ex.ToString());
                }
                firstConnect = true;
                cancellationTokenSource.Cancel();
                cancellationTokenSource = new CancellationTokenSource();
                if (!disposed)
                {
                    // wait 5 seconds before attempting reconnect
                    CreateWebSocket();
                    await Task.Delay(ReconnectInterval);
                }
            }
        }

        private async Task MessageTask()
        {
            DateTime lastCheck = IPBanService.UtcNow;
            KeyValuePair<bool, object> result;
            object message;

            while (!disposed)
            {
                if (webSocket.State != WebSocketState.Open)
                {
                    await Task.Delay(20);
                    continue;
                }
                if ((result = await messageQueue.TryDequeueAsync(cancellationTokenSource.Token)).Key)
                {
                    message = result.Value;
                    try
                    {
                        if (message is Func<Task> action)
                        {
                            await action();
                        }
                        else if (message is Message parsedMessage)
                        {
                            Func<IQueueMessage, Message, Task> actionCopy = OnMessage;
                            if (actionCopy != null)
                            {
                                await actionCopy.Invoke(this, parsedMessage);
                            }
                        }
                        else if (message is byte[] messageBytes)
                        {
                            // multi-thread safe null check
                            Func<IQueueMessage, byte[], Task> actionCopy = OnBinaryMessage;
                            if (actionCopy != null)
                            {
                                await actionCopy.Invoke(this, messageBytes);
                            }
                        }
                        else if (message is string messageString)
                        {
                            // multi-thread safe null check
                            Func<IQueueMessage, string, Task> actionCopy = OnTextMessage;
                            if (actionCopy != null)
                            {
                                await actionCopy.Invoke(this, messageString);
                            }
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        // dont care
                    }
                    catch (Exception ex)
                    {
                        IPBanCore.Logger.Info(ex.ToString());
                    }
                    result = default;
                }
                if (ConnectInterval.Ticks > 0 && (IPBanService.UtcNow - lastCheck) >= ConnectInterval)
                {
                    lastCheck = IPBanService.UtcNow;

                    // this must succeed, the callback may be requests lists or other resources that must not fail
                    QueueActionsWithNoExceptions(InvokeConnected);
                }
            }
        }
    }

    /// <summary>
    /// Delegate for web socket connect / disconnect events
    /// </summary>
    /// <param name="socket">Web socket</param>
    /// <param name="reconnect">False if first connection, true if a subsequent reconnection or reping</param>
    /// <returns>Task</returns>
    public delegate Task WebSocketConnectionDelegate(IQueueMessage socket, bool reconnect);
}
