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

using DigitalRuby.IPBanCore;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace DigitalRuby.IPBanProSDK
{
    /// <summary>
    /// Wraps settings from the SettingsModel and XML IPBan config to make it easier to use a UI to update.
    /// Only the properties from the ipban config xml need to go in this class.
    /// </summary>
    [Serializable]
    [DataContract]
    public class SettingsUpdateModel : SettingsModel
    {
        /// <summary>
        /// Get hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            var message = Message;
            var error = Error;
            Message = null;
            Error = false;
            try
            {
                var jsonText = ExtensionMethods.SerializeJson(this);
                jsonText = jsonText.Normalize().Replace("\r", string.Empty).Replace("\n", "\r\n").Trim();
                var jsonBytes = System.Text.Encoding.UTF8.GetBytes(jsonText);
                var sha512 = System.Security.Cryptography.SHA512.HashData(jsonBytes);
                var hashCode = Convert.ToBase64String(sha512).GetHashCode();
                return hashCode;
            }
            finally
            {
                Error = error;
                Message = message;
            }
        }

        /// <summary>
        /// Failed login attempts before ban
        /// </summary>
        [LocalizedDisplayName(nameof(IPBanResources.FailedLoginAttemptsBeforeBan))]
        [Range(0, 60)]
        [DataMember(Order = 1)]
        public int? FailedLoginAttemptsBeforeBan { get; set; }

        /// <summary>
        /// Ban time
        /// </summary>
        [LocalizedDisplayName(nameof(IPBanResources.BanTime))]
        [DataMember(Order = 2)]
        public string BanTime { get; set; }

        /// <summary>
        /// Whether to reset failed login count to 0 for unbanned ip addresses
        /// </summary>
        [LocalizedDisplayName(nameof(IPBanResources.ResetFailedLoginCountForUnbannedIPAddresses))]
        [DataMember(Order = 3)]
        public bool ResetFailedLoginCountForUnbannedIPAddresses { get; set; }

        /// <summary>
        /// Clear bans on restart
        /// </summary>
        [LocalizedDisplayName(nameof(IPBanResources.ClearBannedIPAddressesOnRestart))]
        [DataMember(Order = 4)]
        public bool ClearBannedIPAddressesOnRestart { get; set; }

        /// <summary>
        /// Expire time to forget failed login
        /// </summary>
        [LocalizedDisplayName(nameof(IPBanResources.ExpireTime))]
        [DisplayFormat(DataFormatString = "{0:dd\\:hh\\:mm\\:ss}", ApplyFormatInEditMode = true)]
        [DataMember(Order = 5)]
        public TimeSpan? ExpireTime { get; set; }

        /// <summary>
        /// How often to run the ipban service
        /// </summary>
        [LocalizedDisplayName(nameof(IPBanResources.CycleTime))]
        [DisplayFormat(DataFormatString = "{0:dd\\:hh\\:mm\\:ss}", ApplyFormatInEditMode = true)]
        [DataMember(Order = 6)]
        public TimeSpan? CycleTime { get; set; }

        /// <summary>
        /// Minimum time between failed login attempts to still count them
        /// </summary>
        [LocalizedDisplayName(nameof(IPBanResources.MinimumTimeBetweenFailedLoginAttempts))]
        [DisplayFormat(DataFormatString = "{0:dd\\:hh\\:mm\\:ss}", ApplyFormatInEditMode = true)]
        [DataMember(Order = 7)]
        public TimeSpan? MinimumTimeBetweenFailedLoginAttempts { get; set; }

        // order 8 was firewall type

        /// <summary>
        /// Firewall rule prefix, use only letters, numbers and underscore
        /// </summary>
        [LocalizedDisplayName(nameof(IPBanResources.FirewallRulePrefix))]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [DataMember(Order = 9)]
        public string FirewallRulePrefix { get; set; }

        /// <summary>
        /// Comma separated list of ip addresses to always allow
        /// </summary>
        [LocalizedDisplayName(nameof(IPBanResources.IPWhitelist))]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [DataMember(Order = 10)]
        public string Whitelist { get; set; }

        /// <summary>
        /// Regex of ip addresses to always allow
        /// </summary>
        [LocalizedDisplayName(nameof(IPBanResources.IPWhitelistRegex))]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [DataMember(Order = 11)]
        public string WhitelistRegex { get; set; }

        /// <summary>
        /// Comma separated list of ip addresses to always ban
        /// </summary>
        [LocalizedDisplayName(nameof(IPBanResources.IPBlacklist))]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [DataMember(Order = 12)]
        public string Blacklist { get; set; }

        /// <summary>
        /// Regex of ip addresses to always ban
        /// </summary>
        [LocalizedDisplayName(nameof(IPBanResources.IPBlacklistRegex))]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [DataMember(Order = 13)]
        public string BlacklistRegex { get; set; }

        /// <summary>
        /// Comma separated list of user names to always allow
        /// </summary>
        [LocalizedDisplayName(nameof(IPBanResources.UserNameWhitelist))]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [DataMember(Order = 14)]
        public string UserNameWhitelist { get; set; }

        /// <summary>
        /// User name whitelist regex. If set, any mismatch user name causes immediate ban.
        /// </summary>
        [LocalizedDisplayName(nameof(IPBanResources.UserNameWhitelistRegex))]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [DataMember(Order = 15)]
        public string UserNameWhitelistRegex { get; set; }

        /// <summary>
        /// Always ban a user name if it is not within this edit distance of a user name white list entry. Ignored if user name whitelist is empty.
        /// </summary>
        [LocalizedDisplayName(nameof(IPBanResources.UserNameWhitelistMinimumEditDistance))]
        [Range(0, 5)]
        [DataMember(Order = 16)]
        public int? UserNameWhitelistMinimumEditDistance { get; set; }

        /// <summary>
        /// Failed login attempts for a whitelisted user name before banning
        /// </summary>
        [LocalizedDisplayName(nameof(IPBanResources.FailedLoginsBeforeBanUserNameWhitelist))]
        [Range(0, 9999)]
        [DataMember(Order = 17)]
        public int? FailedLoginAttemptsBeforeBanUserNameWhitelist { get; set; }

        /// <summary>
        /// Whether to clear failed logins on a successful login
        /// </summary>
        [LocalizedDisplayName(nameof(IPBanResources.ClearFailedLoginsOnSuccessfulLogin))]
        [DataMember(Order = 18)]
        public bool ClearFailedLoginsOnSuccessfulLogin { get; set; }

        /// <summary>
        /// Process to run when there is a ban, relative to the local machine
        /// </summary>
        [LocalizedDisplayName(nameof(IPBanResources.ProcessToRunOnBan))]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [DataMember(Order = 19)]
        public string ProcessToRunOnBan { get; set; }

        /// <summary>
        /// Process to run when there is a successful login, relative to the local machine
        /// </summary>
        [LocalizedDisplayName(nameof(IPBanResources.ProcessToRunOnSuccessfulLogin))]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [DataMember(Order = 20)]
        public string ProcessToRunOnSuccessfulLogin { get; set; }

        /// <summary>
        /// Process to run when there is a unban, relative to the local machine
        /// </summary>
        [LocalizedDisplayName(nameof(IPBanResources.ProcessToRunOnUnban))]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [DataMember(Order = 21)]
        public string ProcessToRunOnUnban { get; set; }

        [LocalizedDisplayName(nameof(IPBanResources.FirewallRules))]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [DataMember(Order = 22)]
        public string FirewallRules { get; set; }

        /// <summary>
        /// Event viewer expressions to block (failed logins)
        /// </summary>
        [DataMember(Order = 23)]
        public EventViewerExpressionsToBlock EventViewerExpressionsBlock { get; set; } = new EventViewerExpressionsToBlock();

        /// <summary>
        /// Event viewer expressions to notify (successful logins)
        /// </summary>
        [DataMember(Order = 24)]
        public EventViewerExpressionsToNotify EventViewerExpressionsNotify { get; set; } = new EventViewerExpressionsToNotify();

        /// <summary>
        /// Log files to parse
        /// </summary>
        [DataMember(Order = 25)]
        public List<IPBanLogFileToParse> LogFilesToParse { get; set; } = [];

        /// <summary>
        /// Uris to pull lists of ip addresses from. Format is RulePrefix,Interval(DD:HH:MM:SS),Uri[NEWLINE].
        /// </summary>
        [LocalizedDisplayName(nameof(IPBanResources.FirewallUriRules))]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [DataMember(Order = 26)]
        public string FirewallUriRules { get; set; }

        /// <summary>
        /// Whether to process internal ip addresses
        /// </summary>
        [LocalizedDisplayName(nameof(IPBanResources.ProcessInternalIPAddresses))]
        [DataMember(Order = 27)]
        public bool ProcessInternalIPAddresses { get; set; }

        /// <summary>
        /// IP threat api key
        /// </summary>
        [LocalizedDisplayName(nameof(IPBanResources.IPThreatApiKey))]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [DataMember(Order = 28)]
        public string IPThreatApiKey { get; set; }

        /// <summary>
        /// User name truncation characters
        /// </summary>
        [LocalizedDisplayName(nameof(IPBanResources.UserNameTruncationChars))]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [DataMember(Order = 29)]
        public string TruncateUserNameChars { get; set; }

        /// <summary>
        /// Whether to use default banned ip address handler
        /// </summary>
        [LocalizedDisplayName(nameof(IPBanResources.UseDefaultBannedIPAddressHandler))]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [DataMember(Order = 30)]
        public bool UseDefaultBannedIPAddressHandler { get; set; } = true;

        // informational

        /// <summary>
        /// Current hash code
        /// </summary>
        [DataMember(Order = 89)]
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public int CurrentHashCode { get; set; }

        /// <summary>
        /// Country block range entry count
        /// </summary>
        [DataMember(Order = 90)]
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public int CountryBlockRangeEntryCount { get; set; }

        /// <summary>
        /// Recent list count
        /// </summary>
        [DataMember(Order = 91)]
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public int RecentListEntryCount { get; set; }

        /// <summary>
        /// Naughty list count
        /// </summary>
        [DataMember(Order = 92)]
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public int NaughtyListEntryCount { get; set; }

        /// <summary>
        /// Country block range entry count
        /// </summary>
        [DataMember(Order = 93)]
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public int AsnBlockRangeEntryCount { get; set; }
    }
}
