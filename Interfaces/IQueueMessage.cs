/*

IPBanPro SDK - https://ipban.com | https://github.com/DigitalRuby/IPBanProSDK
IPBan and IPBan Pro Copyright(c) 2012 Digital Ruby, LLC
support@ipban.com

The MIT License(MIT)

Copyright(c) 2012 Digital Ruby, LLC

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

*/

using System;

namespace DigitalRuby.IPBanProSDK
{
    /// <summary>
    /// Generic message queueing interface
    /// </summary>
    public interface IQueueMessage : IDisposable
    {
        /// <summary>
        /// Queue a message to send as soon as possible
        /// </summary>
        /// <param name="message">Message to send, should be a Message or byte[]</param>
        /// <param name="groupId">Group id, or 0 for none</param>
        /// <returns>True if success, false if error</returns>
        bool QueueMessage(object message, int groupId = 0);
    }

    /// <summary>
    /// Generic message queueing interface for servers
    /// </summary>
    public interface IQueueMessageServer : IQueueMessage
    {
        /// <summary>
        /// Queues a message on to a message group to be sent to all or some clients at the next interval for that group
        /// </summary>
        /// <param name="groupId">Group id</param>
        /// <param name="message">Message to queue, should be Message or byte[]</param>
        /// <param name="clients">Clients to send to (null for all)</param>
        /// <returns>True if queued, false if no group with the specified id or disposed</returns>
        bool QueueMessageForClients(int groupId, object message, System.Collections.Generic.IEnumerable<IQueueMessage> clients = null);
    }

    /// <summary>
    /// Get clients by key
    /// </summary>
    public interface IGetClientsByKey
    {
        /// <summary>
        /// Attempt to get a client by key
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Clients or null if none found</returns>
        System.Collections.Generic.IEnumerable<IQueueMessage> GetClientsByKey(string key);
    }
}
