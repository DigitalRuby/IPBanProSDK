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
    /// Machine
    /// </summary>
    [Serializable]
    public class Machine
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
        /// Fully qualified domain name
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string FQDN { get; set; }

        /// <summary>
        /// Machine guid
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string MachineGuid { get; set; }

        /// <summary>
        /// Local ip address
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string LocalIPAddress { get; set; }

        /// <summary>
        /// Remote ip address
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string RemoteIPAddress { get; set; }

        /// <summary>
        /// Version
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Version { get; set; }

        /// <summary>
        /// OS name
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        public string OSName { get; set; }

        /// <summary>
        /// OS version
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string OSVersion { get; set; }

        /// <summary>
        /// Created at
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Last updated at
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }

        /// <summary>
        /// Comma separated ip addresses to always allow on just this machine
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.MachineWhitelist))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string MachineWhitelist { get; set; }

        /// <summary>
        /// Email addresses for just this machine
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.SmtpTo))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string EmailAddresses { get; set; }

        /// <summary>
        /// Config xml override for just this machine
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.ClientConfiguration))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ConfigXml { get; set; }

        /// <summary>
        /// Notes
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.Notes))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Notes { get; set; }

        /// <summary>
        /// JSON properties
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Props { get; set; }
    }
}
