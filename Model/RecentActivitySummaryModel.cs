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
using System.Runtime.Serialization;

namespace DigitalRuby.IPBanProSDK
{
    /// <summary>
    /// Recent activity model
    /// </summary>
    [Serializable]
    [DataContract]
    public class RecentActivitySummaryModel
    {
        /// <summary>
        /// Country summary list
        /// </summary>
        [DataMember(Order = 1)]
        public List<CountrySummaryModel> CountrySummaries { get; set; }

        /// <summary>
        /// Sum of all recet failed logins
        /// </summary>
        [DataMember(Order = 2)]
        public int SumRecentFailedLogins { get; set; }

        /// <summary>
        /// Sum of all recent banned ip addresses
        /// </summary>
        [DataMember(Order = 3)]
        public int SumRecentBannedIPAddresses { get; set; }

        /// <summary>
        /// Sum of all recent successful logins
        /// </summary>
        [DataMember(Order = 4)]
        public int SumRecentSuccessfulLogins { get; set; }
    }

    /// <summary>
    /// Country summary
    /// </summary>
    [DataContract]
    public class CountrySummaryModel
    {
        /// <summary>
        /// Country name
        /// </summary>
        [DataMember(Order = 1)]
        public string Country { get; set; }

        /// <summary>
        /// Banned ip address count
        /// </summary>
        [DataMember(Order = 2)]
        public int BannedIPAddressCount { get; set; }

        /// <summary>
        /// Failed login count
        /// </summary>
        [DataMember(Order = 3)]
        public int FailedLoginCount { get; set; }
    }
}
