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
    /// Settings
    /// </summary>
    [Serializable]
    [DataContract]
    public class Settings
    {
        private IPBanConfig config;

        /// <summary>
        /// Config from ConfigXml
        /// </summary>
        public IPBanConfig GetConfig() => config ??= IPBanConfig.LoadFromXml(ConfigXml);

        /// <summary>
        /// Id
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [DataMember(Order = 1)]
        public long Id { get; set; }

        /// <summary>
        /// Max blacklisted ip addresses
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.MaxActiveBlacklistedIPAddresses))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Range(0, 1000000)]
        [DataMember(Order = 2)]
        public int MaxActiveBlacklistedIPAddresses { get; set; }

        /// <summary>
        /// Comma separated country codes to ban
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.CountryBlacklist))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 3)]
        public string CountryBlacklist { get; set; }

        /// <summary>
        /// Recent list max count
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.RecentListCount))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 4)]
        [Obsolete]
        public int RecentListCount { get; set; }

        /// <summary>
        /// Naughty list max count
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.NaughtyListCount))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 5)]
        [Obsolete]
        public int NaughtyListCount { get; set; }

        /// <summary>
        /// Types of notifications to enable
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.NotificationFlags))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 6)]
        public IPBanCore.IPAddressEventFlags NotificationFlags { get; set; }

        /// <summary>
        /// Smtp server host
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.SmtpServer))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 7)]
        public string SmtpServer { get; set; }

        /// <summary>
        /// Smtp server port
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.SmtpPort))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 8)]
        public int SmtpPort { get; set; }

        /// <summary>
        /// Smtp user
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.UserName))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 9)]
        public string SmtpUser { get; set; }

        /// <summary>
        /// Smtp password
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.Password))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 10)]
        public string SmtpPassword { get; set; }

        /// <summary>
        /// Smtp enable ssl
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.EnableSSL))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [IgnoreDataMember]
        [NotMapped]
        [JsonIgnore]
        [XmlIgnore]
        public bool SmtpEnableSslBool
        {
            get => SmtpEnableSsl == 1;
            set => SmtpEnableSsl = value ? 1 : 0;
        }

        /// <summary>
        /// Entity framework storage for SmtpEnableSslBool
        /// </summary>
        [DataMember(Order = 11)]
        public int SmtpEnableSsl { get; set; }

        /// <summary>
        /// Smtp from
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.SmtpFrom))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 12)]
        public string SmtpFrom { get; set; }

        /// <summary>
        /// Smtp to
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.SmtpTo))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 13)]
        public string SmtpTo { get; set; }

        /// <summary>
        /// Splits the SmtpTo property into distinct addresses
        /// </summary>
        [IgnoreDataMember]
        [NotMapped]
        [JsonIgnore]
        [XmlIgnore]
        public IReadOnlyCollection<string> SmtpToAddresses => SmtpTo.SplitWithNoEmptyEntries(',', ';');

        /// <summary>
        /// Smtp subject template
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.SmtpSubjectTemplate))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 14)]
        public string SmtpSubjectTemplate { get; set; }

        /// <summary>
        /// Smtp body template
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.SmtpBodyTemplate))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 15)]
        public string SmtpBodyTemplate { get; set; }

        /// <summary>
        /// Smtp who is subject template
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.SmtpWhoisSubjectTemplate))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 16)]
        public string SmtpWhoIsSubjectTemplate { get; set; }

        /// <summary>
        /// Smtp who is body template
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.SmtpWhoisBodyTemplate))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 17)]
        public string SmtpWhoIsBodyTemplate { get; set; }

        /// <summary>
        /// Web hook for notifications
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.NotificationWebHook))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 18)]
        public string NotificationWebHook { get; set; }

        /// <summary>
        /// Config xml
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.ClientConfiguration))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 19)]
        public string ConfigXml { get; set; }

        /// <summary>
        /// Public api key
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.PublicAPIKey))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 20)]
        public string PublicAPIKey { get; set; }

        /// <summary>
        /// Private api key
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.PrivateAPIKey))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 21)]
        public string PrivateAPIKey { get; set; }

        /// <summary>
        /// Minimum trust level
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.MinimumTrustLevel))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Range(0, 11)]
        [DataMember(Order = 22)]
        public int MinimumTrustLevel { get; set; }

        /// <summary>
        /// Base url of the web admin server
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.BaseUrl))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 23)]
        public string BaseUrl { get; set; }

        /// <summary>
        /// Whether to enable lists like recent/naughty list
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.EnableLists))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [IgnoreDataMember]
        [NotMapped]
        [JsonIgnore]
        [XmlIgnore]
        public bool EnableListsBool
        {
            get => EnableLists == 1;
            set => EnableLists = (value ? 1 : 0);
        }

        /// <summary>
        /// Enable lists int for entity framework
        /// </summary>
        [DataMember(Order = 24)]
        public int EnableLists { get; set; } = 1;

        /// <summary>
        /// Allowed ports for country black list
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.AllowedPorts))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 25)]

        public string CountryBlacklistAllowedPorts { get; set; } = string.Empty;

        /// <summary>
        /// Whether to block country ip on first failed login (true) or to
        /// attempt to add the entire country to the firewall (false)
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.CountryBlacklistFirstFailedLogin))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [IgnoreDataMember]
        [NotMapped]
        [JsonIgnore]
        [XmlIgnore]
        public bool CountryBlacklistFirstFailedLoginBool
        {
            get => CountryBlacklistFirstFailedLogin == 1;
            set => CountryBlacklistFirstFailedLogin = (value ? 1 : 0);
        }


        /// <summary>
        /// Entity framework storage for CountryBlacklistFirstFailedLoginBool
        /// </summary>
        [DataMember(Order = 26)]
        public int CountryBlacklistFirstFailedLogin { get; set; } = 1;

        /// <summary>
        /// Entity framework storage for AggregateBanUserNamesBool
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.AggregateBanUserNames))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 27)]
        public string AggregateBanUserNames { get; set; } = "5,24";

        /// <summary>
        /// Whether to invert the country block list as allow only in list and block everything else (true) or to
        /// use the default behavior which is block if in list (false)
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.CountryBlacklistInvert))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [IgnoreDataMember]
        [NotMapped]
        [JsonIgnore]
        [XmlIgnore]
        public bool CountryBlacklistInvertBool
        {
            get => CountryBlacklistInvert == 1;
            set => CountryBlacklistInvert = (value ? 1 : 0);
        }


        /// <summary>
        /// Entity framework storage for CountryBlacklistInvertBool
        /// </summary>
        [DataMember(Order = 28)]
        public int CountryBlacklistInvert { get; set; }

        /// <summary>
        /// Whether to enable monitoring features. This can reduce performance so should be used carefully.
        /// Can be set to 0 for no monitoring, 1 for all monitoring, or a machine name / ip for filtered monitoring.
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.SetFirewallMonitoring))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 29)]
        public string EnableMonitoring { get; set; } = "0";

        /// <summary>
        /// Auto whitelisting days for successful login ip addresses
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.AutoWhitelistSuccessLogins))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Range(0, 365)]
        [DataMember(Order = 30)]
        public int SuccessfulLoginAutoWhitelistDays { get; set; }

        /// <summary>
        /// Other lists (by name) to enable, comma separated.
        /// Format is name=enabled (0 or 1),repeated
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.OtherLists))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 31)]
        public string OtherLists { get; set; } = string.Empty;

        /// <summary>
        /// TimeSpan for ignoring duplicate notifications, empty/zero for no duplicate checking
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.NotificationDuplicateFilterWindow))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 32)]
        public string NotificationDuplicateFilterWindow { get; set; } = string.Empty;
    }
}
