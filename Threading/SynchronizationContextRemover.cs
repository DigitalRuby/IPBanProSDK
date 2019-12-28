using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace DigitalRuby.IPBanProSDK
{
    /// <summary>
    /// Remove sync context, this is done automatically in IPBan Pro API calls where needed
    /// </summary>
    public struct SynchronizationContextRemover : INotifyCompletion
    {
        /// <summary>
        /// Is the context completed
        /// </summary>
        public bool IsCompleted
        {
            get { return SynchronizationContext.Current is null; }
        }

        /// <summary>
        /// Fires when context completes
        /// </summary>
        /// <param name="continuation">Continuation</param>
        public void OnCompleted(Action continuation)
        {
            var prevContext = SynchronizationContext.Current;
            if (prevContext is null)
            {
                continuation?.Invoke();
            }
            else
            {
                try
                {
                    SynchronizationContext.SetSynchronizationContext(null);
                    continuation?.Invoke();
                }
                finally
                {
                    SynchronizationContext.SetSynchronizationContext(prevContext);
                }
            }
        }

        /// <summary>
        /// Get an awaiter
        /// </summary>
        /// <returns></returns>
        public SynchronizationContextRemover GetAwaiter()
        {
            return this;
        }

        /// <summary>
        /// Get the result
        /// </summary>
        public void GetResult()
        {
        }
    }
}
