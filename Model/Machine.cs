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

namespace DigitalRuby.IPBanProSDK
{
    /// <summary>
    /// Machine
    /// </summary>
    [Serializable]
    [DataContract]
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
        [DataMember(Order = 1)]
        public long Id { get; set; }

        /// <summary>
        /// Fully qualified domain name
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 2)]
        public string FQDN { get; set; }

        /// <summary>
        /// Machine guid
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 3)]
        public string MachineGuid { get; set; }

        /// <summary>
        /// Local ip address
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 4)]
        public string LocalIPAddress { get; set; }

        /// <summary>
        /// Remote ip address
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 5)]
        public string RemoteIPAddress { get; set; }

        /// <summary>
        /// Other ip addresses
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 6)]
        public string OtherIPAddresses { get; set; }

        /// <summary>
        /// Version
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 7)]
        public string Version { get; set; }

        /// <summary>
        /// OS name
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [DataMember(Order = 8)]
        public string OSName { get; set; }

        /// <summary>
        /// OS version
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 9)]
        public string OSVersion { get; set; }

        /// <summary>
        /// Created at
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 10)]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Last updated at
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 11)]
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 12)]
        public string Status { get; set; }

        /// <summary>
        /// Comma separated ip addresses to always allow on just this machine. Can use | to delimit with allowed ports (all other ports are blocked).
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.MachineWhitelist))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 13)]
        public string MachineWhitelist { get; set; }

        /// <summary>
        /// Email addresses for just this machine
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.SmtpTo))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 14)]
        public string EmailAddresses { get; set; }

        /// <summary>
        /// Split the EmailAdresses property into a list of addresses
        /// </summary>
        [JsonIgnore]
        [IgnoreDataMember]
        [NotMapped]
        public IReadOnlyCollection<string> EmailAddressesCollection => EmailAddresses.SplitWithNoEmptyEntries(',', ';');

        /// <summary>
        /// Config xml override for just this machine
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.ClientConfiguration))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 15)]
        public string ConfigXml { get; set; }

        /// <summary>
        /// Notes
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [LocalizedDisplayName(nameof(IPBanResources.Notes))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 16)]
        public string Notes { get; set; }

        /// <summary>
        /// JSON properties
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 17)]
        public string Props { get => props; set => props = value; }
        private string props;

        /// <summary>
        /// Allow an alias for this machine instead of displaying fqdn
        /// </summary>
        [JsonIgnore]
        [IgnoreDataMember]
        [NotMapped]
        public string Alias
        {
            get => PropHelper.GetProp(props, "Alias");
            set => props = PropHelper.SetProp(props, "Alias", value);
        }
    }
}
