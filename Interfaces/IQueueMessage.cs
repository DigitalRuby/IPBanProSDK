using System;
using System.Collections.Generic;
using System.Text;

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
        /// <param name="message">Message to send</param>
        /// <param name="groupId">Group id, or 0 for none</param>
        /// <returns>True if success, false if error</returns>
        bool QueueMessage(object message, int groupId = 0);
    }
}
