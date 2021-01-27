using System;
using System.Runtime.Serialization;

using DigitalRuby.IPBanCore;

namespace DigitalRuby.IPBanProSDK
{
    /// <summary>
    /// Model for license keys
    /// </summary>
    [Serializable]
    [DataContract]
    public class LicensesModel : BaseModel
    {
        /// <summary>
        /// License keys, newline (\n) delimited.
        /// </summary>
        [DataMember(Order = 1)]
        [LocalizedDisplayName(nameof(IPBanResources.Licenses))]
        public string Licenses { get; set; }

        /// <summary>
        /// License keys used
        /// </summary>
        [DataMember(Order = 2)]
        [LocalizedDisplayName(nameof(IPBanResources.Used))]
        public int Used { get; set; }

        /// <summary>
        /// License keys available
        /// </summary>
        [DataMember(Order = 3)]
        [LocalizedDisplayName(nameof(IPBanResources.Available))]

        public int Available { get; set; }

        /// <summary>
        /// Whether to remove all existing uses of license keys and reset usage
        /// This can be done if you had an old machine using license keys that you
        /// no longer need
        /// </summary>
        [DataMember(Order = 4)]
        [LocalizedDisplayName(nameof(IPBanResources.RemoveAllLicenseKeys))]
        public bool RemoveAll { get; set; }
    }
}
