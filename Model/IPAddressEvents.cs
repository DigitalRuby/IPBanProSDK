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

using System.Collections.Generic;

namespace DigitalRuby.IPBanProSDK
{
    /// <summary>
    /// Events for an ip address
    /// </summary>
    public class IPAddressEvents
    {
        /// <summary>
        /// IP address
        /// </summary>
        public IPAddress IPAddress { get; set; }

        /// <summary>
        /// Failed login attempts
        /// </summary>
        public IReadOnlyList<FailedLoginAttempt> FailedLoginAttempts { get; set; } = new FailedLoginAttempt[0];

        /// <summary>
        /// Successful login attempts
        /// </summary>
        public IReadOnlyList<SuccessLoginAttempt> SuccessLoginAttempts { get; set; } = new SuccessLoginAttempt[0];

        /// <summary>
        /// Blacklisted ip addresses
        /// </summary>
        public IReadOnlyList<BlacklistedIPAddress> BlacklistedIPAddresses { get; set; } = new BlacklistedIPAddress[0];
    }
}
