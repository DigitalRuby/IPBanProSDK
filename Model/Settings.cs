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

using DigitalRuby.IPBan;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalRuby.IPBanProSDK
{
    /// <summary>
    /// Settings
    /// </summary>
    [Serializable]
    public class Settings
    {
        /// <summary>
        /// Id
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// Max blacklisted ip addresses
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.MaxActiveBlacklistedIPAddresses))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Range(0, 1000000)]
        public int MaxActiveBlacklistedIPAddresses { get; set; }

        /// <summary>
        /// Comma separated country codes to ban
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.CountryBlacklist))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CountryBlacklist { get; set; }

        /// <summary>
        /// Recent list max count
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.RecentListCount))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int RecentListCount { get; set; }

        /// <summary>
        /// Naughty list max count
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.NaughtyListCount))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int NaughtyListCount { get; set; }

        /// <summary>
        /// Types of notifications to enable
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.NotificationFlags))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IPBan.IPAddressEventFlags NotificationFlags { get; set; }

        /// <summary>
        /// Smtp server host
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.SmtpServer))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SmtpServer { get; set; }

        /// <summary>
        /// Smtp server port
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.SmtpPort))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int SmtpPort { get; set; }

        /// <summary>
        /// Smtp user
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.UserName))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SmtpUser { get; set; }

        /// <summary>
        /// Smtp password
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.Password))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SmtpPassword { get; set; }

        /// <summary>
        /// Smtp enable ssl
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.EnableSSL))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool SmtpEnableSsl { get; set; }

        /// <summary>
        /// Smtp from
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.SmtpFrom))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SmtpFrom { get; set; }

        /// <summary>
        /// Smtp to
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.SmtpTo))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SmtpTo { get; set; }

        /// <summary>
        /// Smtp subject template
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.SmtpSubjectTemplate))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SmtpSubjectTemplate { get; set; }

        /// <summary>
        /// Smtp body template
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.SmtpBodyTemplate))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SmtpBodyTemplate { get; set; }

        /// <summary>
        /// Smtp who is subject template
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.SmtpWhoisSubjectTemplate))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SmtpWhoIsSubjectTemplate { get; set; }

        /// <summary>
        /// Smtp who is body template
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.SmtpWhoisBodyTemplate))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SmtpWhoIsBodyTemplate { get; set; }

        /// <summary>
        /// Config xml
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.ClientConfiguration))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ConfigXml { get; set; }

        /// <summary>
        /// Public api key
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.PublicAPIKey))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PublicAPIKey { get; set; }

        /// <summary>
        /// Private api key
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.PrivateAPIKey))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PrivateAPIKey { get; set; }

        /// <summary>
        /// Minimum trust level
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.MinimumTrustLevel))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Range(0, 11)]
        public int MinimumTrustLevel { get; set; }

        /// <summary>
        /// Base url
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.BaseUrl))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string BaseUrl { get; set; }
    }
}
