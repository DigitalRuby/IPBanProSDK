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
    /// <param name="Id">Id</param>
    /// <param name="OrgKeyHashed">Org key hashed</param>
    /// <param name="Expires">Expires</param>
    /// <param name="AccessDuration">Access duration</param>
    /// <param name="RemainingUses">Remaining uses</param>
    /// <param name="MachineAccess">Machine access or null for none</param>
    /// <param name="Notes">Notes</param>
    /// <param name="IPAddresses">IP addresses</param>
    public record WhitelistUrl(string Id, string OrgKeyHashed, string Expires, TimeSpan AccessDuration, int RemainingUses, string MachineAccess, string Notes)
    {
        /// <summary>
        /// IP addresses
        /// </summary>
        public List<UrlIPAddress> IPAddresses { get; set; }
    };

    /// <summary>
    /// Url ip address
    /// </summary>
    /// <param name="IPAddress">IP address</param>
    /// <param name="Expires">Expires</param>
    /// <param name="Notes">Notes</param>
    public record UrlIPAddress(string IPAddress, string Expires, string Notes);
}
