using System;
using System.Runtime.Serialization;

namespace DigitalRuby.IPBanProSDK
{
    /// <summary>
    /// Machine whitelist model
    /// </summary>
    [Serializable]
    [DataContract]
    public sealed class MachineWhitelistModel : BaseModel
    {
        /// <summary>
        /// Machine id
        /// </summary>
        [DataMember(Order = 1)]
        public long Id { get; set; }

        /// <summary>
        /// Allowed IP addresses (comma separated, can include CIDR notation)
        /// Example: 1.1.1.1,2.2.2.0/24.
        /// </summary>
        [DataMember(Order = 2)]
        public string AllowedIPAddresses { get; set; } = string.Empty;

        /// <summary>
        /// Allowed ports (comma separated, ranges denoted with -). If set, any port not in this list will be blocked.
        /// These are independent of the allowed ip addresses, which take precedence.
        /// Example: 80,443,5000-5050.
        /// </summary>
        [DataMember(Order = 3)]
        public string AllowedPorts { get; set; } = string.Empty;
    }
}
