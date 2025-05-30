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
using System.Runtime.Serialization;

namespace DigitalRuby.IPBanProSDK
{
    /// <summary>
    /// Recent activity model
    /// </summary>
    [Serializable]
    [DataContract]
    public class RecentActivityModel : BaseModel
    {
        /// <summary>
        /// Summary
        /// </summary>
        [DataMember(Order = 1)]
        public RecentActivitySummaryModel Summary { get; set; }

        /// <summary>
        /// Recent blacklisted ip addresses
        /// </summary>
        [DataMember(Order = 2)]
        public List<BlacklistedIPAddress> BlacklistedIPAddresses { get; set; }

        /// <summary>
        /// Recent success login attempts
        /// </summary>
        [DataMember(Order = 3)]
        public List<SuccessLoginAttempt> SuccessLoginAttempts { get; set; }

        /// <summary>
        /// Recent failed login attempts
        /// </summary>
        [DataMember(Order = 4)]
        public List<RecentActivityFailedLogin> FailedLoginAttempts { get; set; }

        /// <summary>
        /// Machine if activity is for a single machine, null for all machines
        /// </summary>
        [DataMember(Order = 5)]
        public Machine Machine { get; set; }
    }

    /// <summary>
    /// Recent activity failed login
    /// </summary>
    [Serializable]
    [DataContract]
    public class RecentActivityFailedLogin
    {
        /// <summary>
        /// Failed login attempt
        /// </summary>
        [DataMember(Order = 1)]
        public FailedLoginAttempt FailedLoginAttempt { get; set; }

        /// <summary>
        /// IP address
        /// </summary>
        [DataMember(Order = 2)]
        public IPAddress IPAddress { get; set; }

        /// <summary>
        /// Machine
        /// </summary>
        [DataMember(Order = 3)]
        public Machine Machine { get; set; }

        /// <summary>
        /// Is blacklisted
        /// </summary>
        [DataMember(Order = 4)]
        public bool IsBlacklisted { get; set; }
    }

    /// <summary>
    /// Failged logins model
    /// </summary>
    public class RecentActivityFailedLoginsModel : BaseModel
    {
        /// <summary>
        /// Failed logins
        /// </summary>
        public List<RecentActivityFailedLogin> FailedLogins { get; set; }
    }

    /// <summary>
    /// Blocked ip addresses model
    /// </summary>
    public class RecentActivityBlockedIPAddressesModel : BaseModel
    {
        /// <summary>
        /// Blocked ip addresses
        /// </summary>
        public List<BlacklistedIPAddress> BlockedIPAddresses { get; set; }
    }

    /// <summary>
    /// Successful logins model
    /// </summary>
    public class RecentActivitySuccessfulLoginsModel : BaseModel
    {
        /// <summary>
        /// Successful logins
        /// </summary>
        public List<SuccessLoginAttempt> SuccessfulLogins { get; set; }
    }
}
