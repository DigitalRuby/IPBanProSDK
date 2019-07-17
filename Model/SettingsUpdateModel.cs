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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

using DigitalRuby.IPBan;

namespace DigitalRuby.IPBanProSDK
{
    /// <summary>
    /// Wraps settings from the SettingsModel and XML IPBan config to make it easier to use a UI to update
    /// </summary>
    [Serializable]
    public class SettingsUpdateModel : SettingsModel
    {
        /// <summary>
        /// Failed login attempts before ban
        /// </summary>
        [LocalizedDisplayName(nameof(IPBanResources.FailedLoginAttemptsBeforeBan))]
        [Range(0, 60)]
        public int? FailedLoginAttemptsBeforeBan { get; set; }

        /// <summary>
        /// Ban time
        /// </summary>
        [LocalizedDisplayName(nameof(IPBanResources.BanTime))]
        public string BanTime { get; set; }

        /// <summary>
        /// Whether to reset failed login count to 0 for unbanned ip addresses
        /// </summary>
        [LocalizedDisplayName(nameof(IPBanResources.ResetFailedLoginCountForUnbannedIPAddresses))]
        public bool? ResetFailedLoginCountForUnbannedIPAddresses { get; set; }

        /// <summary>
        /// Clear bans on restart
        /// </summary>
        [LocalizedDisplayName(nameof(IPBanResources.ClearBannedIPAddressesOnRestart))]
        public bool? ClearBannedIPAddressesOnRestart { get; set; }

        /// <summary>
        /// Expire time to forget failed login
        /// </summary>
        [LocalizedDisplayName(nameof(IPBanResources.ExpireTime))]
        [DisplayFormat(DataFormatString = "{0:dd\\:hh\\:mm\\:ss}", ApplyFormatInEditMode = true)]
        public TimeSpan? ExpireTime { get; set; }

        /// <summary>
        /// How often to run the ipban service
        /// </summary>
        [LocalizedDisplayName(nameof(IPBanResources.CycleTime))]
        [DisplayFormat(DataFormatString = "{0:dd\\:hh\\:mm\\:ss}", ApplyFormatInEditMode = true)]
        public TimeSpan? CycleTime { get; set; }

        /// <summary>
        /// Minimum time between failed login attempts to still count them
        /// </summary>
        [LocalizedDisplayName(nameof(IPBanResources.MinimumTimeBetweenFailedLoginAttempts))]
        [DisplayFormat(DataFormatString = "{0:dd\\:hh\\:mm\\:ss}", ApplyFormatInEditMode = true)]
        public TimeSpan? MinimumTimeBetweenFailedLoginAttempts { get; set; }

        /// <summary>
        /// Firewall type
        /// </summary>
        [LocalizedDisplayName(nameof(IPBanResources.FirewallType))]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string FirewallType { get; set; }

        /// <summary>
        /// Firewall rule prefix, use only letters, numbers and underscore
        /// </summary>
        [LocalizedDisplayName(nameof(IPBanResources.FirewallRulePrefix))]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string FirewallRulePrefix { get; set; }

        /// <summary>
        /// Comma separated list of ip addresses to always allow
        /// </summary>
        [LocalizedDisplayName(nameof(IPBanResources.IPWhitelist))]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Whitelist { get; set; }

        /// <summary>
        /// Regex of ip addresses to always allow
        /// </summary>
        [LocalizedDisplayName(nameof(IPBanResources.IPWhitelistRegex))]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string WhitelistRegex { get; set; }

        /// <summary>
        /// Comma separated list of ip addresses to always ban
        /// </summary>
        [LocalizedDisplayName(nameof(IPBanResources.IPBlacklist))]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Blacklist { get; set; }

        /// <summary>
        /// Regex of ip addresses to always ban
        /// </summary>
        [LocalizedDisplayName(nameof(IPBanResources.IPBlacklistRegex))]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string BlacklistRegex { get; set; }

        /// <summary>
        /// Comma separated list of user names to always allow
        /// </summary>
        [LocalizedDisplayName(nameof(IPBanResources.UserNameWhitelist))]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string UserNameWhitelist { get; set; }

        /// <summary>
        /// Always ban a user name if it is not within this edit distance of a user name white list entry. Ignored if user name whitelist is empty.
        /// </summary>
        [LocalizedDisplayName(nameof(IPBanResources.UserNameWhiteListMinimumEditDistance))]
        [Range(0, 5)]
        public int? UserNameWhiteListMinimumEditDistance { get; set; }

        /// <summary>
        /// Failed login attempts for a whitelisted user name before banning
        /// </summary>
        [LocalizedDisplayName(nameof(IPBanResources.FailedLoginsBeforeBanUserNameWhitelist))]
        [Range(0, 60)]
        public int? FailedLoginAttemptsBeforeBanUserNameWhitelist { get; set; }

        /// <summary>
        /// Process to run when there is a ban, relative to the local machine
        /// </summary>
        [LocalizedDisplayName(nameof(IPBanResources.ProcessToRunOnBan))]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ProcessToRunOnBan { get; set; }

        /// <summary>
        /// Event viewer expressions to block (failed logins)
        /// </summary>
        public EventViewerExpressionsToBlock EventViewerExpressionsBlock { get; set; } = new EventViewerExpressionsToBlock();

        /// <summary>
        /// Event viewer expressions to notify (successful logins)
        /// </summary>
        public EventViewerExpressionsToNotify EventViewerExpressionsNotify { get; set; } = new EventViewerExpressionsToNotify();

        /// <summary>
        /// Log files to parse
        /// </summary>
        public List<IPBanLogFileToParse> LogFilesToParse { get; set; } = new List<IPBanLogFileToParse>();

        /// <summary>
        /// Number of ip address ranges currently in the country block list
        /// </summary>
        public int CountryBlockRangeEntryCount { get; set; }

        /// <summary>
        /// Number of ip addresses currently in the recent block list
        /// </summary>
        public int RecentListEntryCount { get; set; }

        /// <summary>
        /// Number of ip addresses currently in the naughty block list
        /// </summary>
        public int NaughtyListEntryCount { get; set; }
    }
}
