﻿/*

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
using System.Runtime.CompilerServices;
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
        public readonly bool IsCompleted
        {
            get { return SynchronizationContext.Current is null; }
        }

        /// <summary>
        /// Fires when context completes
        /// </summary>
        /// <param name="continuation">Continuation</param>
        public readonly void OnCompleted(Action continuation)
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
        public readonly SynchronizationContextRemover GetAwaiter()
        {
            return this;
        }

        /// <summary>
        /// Get the result
        /// </summary>
        public readonly void GetResult()
        {
        }
    }
}
