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


using System;

namespace DigitalRuby.IPBanProSDK
{
    /// <summary>
    /// IPBan shield ip address event
    /// </summary>
    [Serializable]
    public class IPBanShieldIPAddressEvent
    {
        /// <summary>
        /// Trust level (0 - 10, 0 is lowest)
        /// </summary>
        public int TrustLevel { get; set; }

        /// <summary>
        /// IP address that caused the event
        /// </summary>
        public string IPAddress { get; set; }

        /// <summary>
        /// Geography of the ip address
        /// </summary>
        public IPAddressGeography Geography { get; set; }

        /// <summary>
        /// Ban count for the ip address
        /// </summary>
        public int BanCount { get; set; }

        /// <summary>
        /// Source ip address unique identifier
        /// </summary>
        public string SourceIPAddress { get; set; }

        /// <summary>
        /// Software version that was used
        /// </summary>
        public string Version { get; set; }
    }
}
