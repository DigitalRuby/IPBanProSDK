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
        public string CountryBlacklist { get; set; } = string.Empty;

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
        public IPBanCore.IPAddressNotificationFlags NotificationFlags { get; set; }

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
        /// All 0 = no ssl
        /// Bit 1 = enable ssl
        /// Bit 2 = self signed certificate
        /// </summary>
        [DataMember(Order = 11)]
        public int SmtpEnableSsl { get; set; }

        /// <summary>
        /// Smtp enable ssl
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.EnableSSL))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [IgnoreDataMember]
        [NotMapped]
        [XmlIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public bool SmtpEnableSslBool
        {
            get { return (SmtpEnableSsl & 1) == 1; }
            set
            {
                if (value)
                {
                    SmtpEnableSsl |= 1;
                }
                else
                {
                    SmtpEnableSsl &= (~1);
                }
            }
        }

        /// <summary>
        /// Smtp ssl self signed certificate
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.SslSelfSignedCertificate))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [IgnoreDataMember]
        [NotMapped]
        [XmlIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public bool SmtpSslSelfSignedCertificateBool
        {
            get { return (SmtpEnableSsl & 2) == 2; }
            set
            {
                if (value)
                {
                    SmtpEnableSsl |= 2;
                }
                else
                {
                    SmtpEnableSsl &= (~2);
                }
            }
        }

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
        [XmlIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
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
        [XmlIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
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
        [XmlIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
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
        /// Count and hour timeframe to aggregate user names and ban over time
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.AggregateBanUserNames))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 27)]
        public string AggregateBanUserNames { get; set; } = "5,24";

        /// <summary>
        /// Entity framework storage for AggregateBanUserNamesAllowBlankBool
        /// </summary>
        [DataMember(Order = 28)]
        public int AggregateBanUserNamesAllowBlank { get; set; } = 0;

        /// <summary>
        /// Entity framework storage for AggregateBanUserNamesBool
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.AggregateBanUserNamesAllowBlank))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [IgnoreDataMember]
        [NotMapped]
        [XmlIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public bool AggregateBanUserNamesAllowBlankBool
        {
            get => AggregateBanUserNamesAllowBlank == 1;
            set => AggregateBanUserNamesAllowBlank = (value ? 1 : 0);
        }

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
        [XmlIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public bool CountryBlacklistInvertBool
        {
            get => CountryBlacklistInvert == 1;
            set => CountryBlacklistInvert = (value ? 1 : 0);
        }


        /// <summary>
        /// Entity framework storage for CountryBlacklistInvertBool
        /// </summary>
        [DataMember(Order = 29)]
        public int CountryBlacklistInvert { get; set; }

        /// <summary>
        /// Whether to enable monitoring features. This can reduce performance so should be used carefully.
        /// Can be set to 0 for no monitoring, 1 for all monitoring, or a machine name / ip for filtered monitoring.
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.SetFirewallMonitoring))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 30)]
        public string EnableMonitoring { get; set; } = "0";

        /// <summary>
        /// Auto whitelisting days for successful login ip addresses
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.AutoWhitelistSuccessLogins))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Range(0, 365)]
        [DataMember(Order = 31)]
        public int SuccessfulLoginAutoWhitelistDays { get; set; }

        /// <summary>
        /// Other lists (by name) to enable, comma separated.
        /// Format is name=enabled (0 or 1),repeated
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.OtherLists))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 32)]
        public string OtherLists { get; set; } = string.Empty;

        /// <summary>
        /// TimeSpan for ignoring duplicate notifications, empty/zero for no duplicate checking
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.NotificationDuplicateFilterWindow))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 33)]
        public string NotificationDuplicateFilterWindow { get; set; } = string.Empty;

        /// <summary>
        /// Country blacklist precise bool
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.CountryBlacklistPrecise))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [IgnoreDataMember]
        [NotMapped]
        [XmlIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public bool CountryBlacklistPreciseBool
        {
            get => CountryBlacklistPrecise == 1;
            set => CountryBlacklistPrecise = (value ? 1 : 0);
        }

        /// <summary>
        /// Country blacklist precise int
        /// </summary>
        [DataMember(Order = 34)]
        public int CountryBlacklistPrecise { get; set; }

        /// <summary>
        /// Comma separated asn ids to block
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.AsnBlacklist))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 35)]
        public string AsnBlacklist { get; set; } = string.Empty;


        /// <summary>
        /// Whether to whitelist asns instead of blacklist
        /// </summary>
        [NotMapped]
        [IgnoreDataMember]
        [XmlIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [LocalizedDisplayName(nameof(IPBanResources.AsnBlacklistWhitelist))]
        public bool AsnBlacklistWhitelistBool
        {
            get { return PropHelper.GetProp<bool>(PropertiesJson, nameof(AsnBlacklistWhitelistBool)); }
            set { PropertiesJson = PropHelper.SetProp(PropertiesJson, nameof(AsnBlacklistWhitelistBool), value.ToString()); }
        }

        /// <summary>
        /// Concurrent ip banning
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.ConcurrentRemoteIPBan))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 36)]
        public string ConcurrentRemoteIPBan { get; set; } = string.Empty;

        /// <summary>
        /// Threshold for banning an ip address range /24 (count, days)
        /// </summary>
        [NotMapped]
        [IgnoreDataMember]
        [XmlIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [LocalizedDisplayName(nameof(IPBanResources.BanRangeThreshold))]
        public string BanRangeThreshold
        {
            get { return PropHelper.GetProp(PropertiesJson, nameof(BanRangeThreshold)); }
            set { PropertiesJson = PropHelper.SetProp(PropertiesJson, nameof(BanRangeThreshold), value); }
        }

       /// <summary>
       /// All other properties go here. Can document in this comment.
       /// </summary>
       [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 37)]
        public string PropertiesJson { get; set;  } = string.Empty;

        [NotMapped]
        [IgnoreDataMember]
        [XmlIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public Dictionary<string, string> Properties
        {
            get => string.IsNullOrWhiteSpace(PropertiesJson) ? [] : JsonConvert.DeserializeObject<Dictionary<string, string>>(PropertiesJson);
            set => PropertiesJson = JsonConvert.SerializeObject(value);
        }

        [NotMapped]
        [IgnoreDataMember]
        [XmlIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public int LogsMachineId
        {
            get
            {
                string value = PropHelper.GetProp(PropertiesJson, nameof(LogsMachineId));
                if (string.IsNullOrWhiteSpace(value))
                {
                    return 0;
                }
                return int.Parse(value);
            }
            set
            {
                PropertiesJson = PropHelper.SetProp(PropertiesJson, nameof(LogsMachineId), value.ToString());
            }
        }

        private DigitalRuby.IPBanCore.LogLevel lastLogLevel;
        private string lastLogLevelString;
        [NotMapped]
        [IgnoreDataMember]
        [XmlIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public DigitalRuby.IPBanCore.LogLevel LogsLevel
        {
            get
            {
                string value = PropHelper.GetProp(PropertiesJson, nameof(LogsLevel));
                if (string.IsNullOrWhiteSpace(value))
                {
                    return DigitalRuby.IPBanCore.LogLevel.Error;
                }
                else if (value == lastLogLevelString)
                {
                    return lastLogLevel;
                }
                lastLogLevelString = value;
                lastLogLevel = Enum.Parse<DigitalRuby.IPBanCore.LogLevel>(value);
                return lastLogLevel;
            }
            set
            {
                PropertiesJson = PropHelper.SetProp(PropertiesJson, nameof(LogsLevel), value.ToString());
            }
        }

        [NotMapped]
        [IgnoreDataMember]
        [XmlIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public DateTime LogsTimestamp
        {
            get
            {
                string value = PropHelper.GetProp(PropertiesJson, nameof(LogsTimestamp));
                if (string.IsNullOrWhiteSpace(value))
                {
                    return IPBanService.UtcNow;
                }
                return DateTime.Parse(value);
            }
            set
            {
                PropertiesJson = PropHelper.SetProp(PropertiesJson, nameof(LogsTimestamp), value.ToUniversalTime().ToString("s") + "Z");
            }
        }

        [NotMapped]
        [IgnoreDataMember]
        [XmlIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public int LogsMaxCount
        {
            get
            {
                string value = PropHelper.GetProp(PropertiesJson, nameof(LogsMaxCount));
                if (string.IsNullOrWhiteSpace(value))
                {
                    return 10000;
                }
                return int.Parse(value);
            }
            set
            {
                PropertiesJson = PropHelper.SetProp(PropertiesJson, nameof(LogsMaxCount), value.ToString());
            }
        }
    }
}
