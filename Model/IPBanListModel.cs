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

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DigitalRuby.IPBanProSDK
{
    /// <summary>
    /// Ban list paged model
    /// </summary>
    [Serializable]
    [DataContract]
    public class BannedIPAddressesModel : BaseModel
    {
        /// <summary>
        /// Banned ip addresses
        /// </summary>
        [DataMember(Order = 1)]
        public IReadOnlyCollection<BannedIPAddress> BannedIPAddresses { get; set; }
    }

    /// <summary>
    /// Ban list paged model
    /// </summary>
    [Serializable]
    public class RecentBannedIPAddressesModel : BaseModel
    {
        /// <summary>
        /// Recent banned ip addresses
        /// </summary>
        [DataMember(Order = 1)]
        public IReadOnlyCollection<RecentBannedIPAddress> RecentBannedIPAddresses { get; set; }
    }

    /// <summary>
    /// Banned ip address
    /// </summary>
    [Serializable]
    public class BannedIPAddress
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ipAddress">IP address</param>
        /// <param name="banCount">Ban count</param>
        public BannedIPAddress(string ipAddress, long banCount)
        {
            IPAddress = ipAddress;
            BanCount = banCount;
        }

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns>String</returns>
        public override string ToString()
        {
            return $"IP: {IPAddress}, Ban Count: {BanCount}";
        }

        /// <summary>
        /// Check for equality
        /// </summary>
        /// <param name="obj">Other object</param>
        /// <returns>True if equal, false if not</returns>
        public override bool Equals(object obj)
        {
            if (obj is BannedIPAddress addr)
            {
                return IPAddress.Equals(addr.IPAddress);
            }
            return false;
        }

        /// <summary>
        /// Get hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return IPAddress.GetHashCode();
        }

        /// <summary>
        /// IP address
        /// </summary>
        [JsonProperty(IPBanProBaseAPI.KeyIPAddress)]
        [DataMember(Order = 1)]
        public string IPAddress { get; set; }

        /// <summary>
        /// Ban count
        /// </summary>
        [JsonProperty(IPBanProBaseAPI.KeyCount)]
        [DataMember(Order = 2)]
        public long BanCount { get; set; }
    }

    /// <summary>
    /// Banned ip address
    /// </summary>
    [Serializable]
    [DataContract]
    public class RecentBannedIPAddress : BannedIPAddress
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ipAddress">IP address</param>
        /// <param name="banCount">Ban count</param>
        /// <param name="timestamp">Timestamp</param>
        public RecentBannedIPAddress(string ipAddress, long banCount, DateTime timestamp) : base(ipAddress, banCount)
        {
            Timestamp = timestamp;
        }

        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonProperty("Timestamp")]
        [DataMember(Order = 1)]
        public DateTime Timestamp { get; set; }
    }
}
