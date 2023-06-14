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
using System.Collections.Generic;

namespace DigitalRuby.IPBanProSDK.Model.WhitelistUrls
{

    /// <summary>
    /// Whitelist url
    /// </summary>
    public sealed class WhitelistUrl
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; init; }

        /// <summary>
        /// Org key hashed
        /// </summary>
        public string OrgKeyHashed { get; init; }

        /// <summary>
        /// Expiration timestamp
        /// </summary>
        public string Expires { get; init; }

        /// <summary>
        /// Access duration for each use
        /// </summary>
        public TimeSpan AccessDuration { get; init; }

        /// <summary>
        /// Remaining allowed uses
        /// </summary>
        public int RemainingUses { get; init; }

        /// <summary>
        /// Machine access or null for all
        /// </summary>
        public string MachineAccess { get; init; }

        /// <summary>
        /// Notes or empty if none
        /// </summary>
        public string Notes { get; init; } = string.Empty;

        /// <summary>
        /// IP addresses that have registered with this url
        /// </summary>
        public List<UrlIPAddress> IPAddresses { get; init; } = new();
    }

    /// <summary>
    /// Url ip address
    /// </summary>
    /// <param name="IPAddress">IP address</param>
    /// <param name="Expires">Expires</param>
    /// <param name="Notes">Notes</param>
    public record UrlIPAddress(string IPAddress, string Expires, string Notes)
    {
        /// <inheritdoc />
        public override string ToString()
        {
            return $"{IPAddress},{Expires},{Notes}";
        }
    }
}
