using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

using Newtonsoft.Json;

namespace DigitalRuby.IPBanProSDK
{
    public class IPAddressEvents
    {
        public IPAddressEntry IPAddress { get; set; }
        public IReadOnlyList<FailedLoginAttempt> FailedLoginAttempts;
        public IReadOnlyList<SuccessLoginAttempt> SuccessLoginAttempts;

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool Banned { get; set; }
    }
}
