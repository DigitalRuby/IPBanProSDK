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
using System.Collections.Generic;
using System.Net;

namespace DigitalRuby.IPBanProSDK
{
    /// <summary>
    /// Represents an http request
    /// </summary>
    public interface IHttpRequest
    {
        /// <summary>
        /// The requested uri
        /// </summary>
        Uri Uri { get; }

        /// <summary>
        /// Remote end point
        /// </summary>
        IPEndPoint RemoteEndPoint { get; }

        /// <summary>
        /// Local end point
        /// </summary>
        IPEndPoint LocalEndPoint { get; }

        /// <summary>
        /// The remote port of the connecting client
        /// </summary>
        int RemotePort { get; }

        /// <summary>
        /// Connection specific state
        /// </summary>
        Dictionary<object, object> Items { get; }
    }

    /// <summary>
    /// Represents a default http request
    /// </summary>
    public class DefaultHttpRequest : IHttpRequest
    {
        /// <summary>
        /// Uri
        /// </summary>
        public Uri Uri { get; set; }

        /// <summary>
        /// Remote end point
        /// </summary>
        public IPEndPoint RemoteEndPoint { get; set; }

        /// <summary>
        /// Local end point
        /// </summary>
        public IPEndPoint LocalEndPoint { get; set; }

        /// <summary>
        /// Remote port of the connecting client
        /// </summary>
        public int RemotePort { get; set; }

        /// <summary>
        /// Client specific state
        /// </summary>
        public Dictionary<object, object> Items { get; } = [];
    }
}
