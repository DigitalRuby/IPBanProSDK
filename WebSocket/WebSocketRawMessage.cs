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
using System.Net.WebSockets;
using System.Text;

#endregion Imports

namespace DigitalRuby.IPBanProSDK
{
    /// <summary>
    /// A web socket raw message
    /// </summary>
    public class WebSocketRawMessage
    {
        /// <summary>
        /// Constructor for no message
        /// </summary>
        public WebSocketRawMessage()
        {
        }

        /// <summary>
        /// Constructor for text or binary message
        /// </summary>
        /// <param name="obj">Object</param>
        /// <param name="messageType">Message type (forced to text if obj is a string)</param>
        public WebSocketRawMessage(object obj, WebSocketMessageType messageType = WebSocketMessageType.Binary)
        {
            if (obj is string text)
            {
                Data = (Encoding.UTF8.GetBytes(text)).AsMemory();
                MessageType = WebSocketMessageType.Text;
            }
            else
            {
                if (obj is byte[] bytes)
                {
                    Data = bytes.AsMemory();
                }
                else
                {
                    Data = IPBanProSDKExtensionMethods.CreateWegbSocketCompressedJsonMessage(obj).AsMemory();
                }
                MessageType = messageType;
            }
        }

        /// <summary>
        /// Constructor for text message
        /// </summary>
        /// <param name="text">Text</param>
        /// <param name="messageType">Message type</param>
        public WebSocketRawMessage(string text)
        {
            Data = (Encoding.UTF8.GetBytes(text)).AsMemory();
            MessageType = WebSocketMessageType.Text;
        }

        /// <summary>
        /// Constructor for text or binary message
        /// </summary>
        /// <param name="bytes">Bytes</param>
        /// <param name="messageType">Message type</param>
        public WebSocketRawMessage(byte[] bytes, WebSocketMessageType messageType = WebSocketMessageType.Binary)
        {
            Data = bytes.AsMemory();
            MessageType = messageType;
        }

        /// <summary>
        /// Message data, if length is 0 there is no message
        /// </summary>
        public Memory<byte> Data { get; set; }

        /// <summary>
        /// Message type
        /// </summary>
        public WebSocketMessageType MessageType { get; private set; }
    }
}
