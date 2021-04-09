using DigitalRuby.IPBanCore;

using System;
using System.Runtime.Serialization;

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
        /// License key info, newline (\n) delimited.
        /// </summary>
        [DataMember(Order = 1)]
        [LocalizedDisplayName(nameof(IPBanResources.Licenses))]
        public string Licenses { get; set; }

        /// <summary>
        /// Number of connected clients
        /// </summary>
        [DataMember(Order = 2)]
        [LocalizedDisplayName(nameof(IPBanResources.ConnectedClients))]
        public long ConnectedClients { get; set; }
    }
}
