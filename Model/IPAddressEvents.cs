using System.Collections.Generic;

namespace DigitalRuby.IPBanProSDK
{
    public class IPAddressEvents
    {
        public IPAddress IPAddress { get; set; }
        public IReadOnlyList<FailedLoginAttempt> FailedLoginAttempts { get; set; } = new FailedLoginAttempt[0];
        public IReadOnlyList<SuccessLoginAttempt> SuccessLoginAttempts { get; set; } = new SuccessLoginAttempt[0];
        public IReadOnlyList<BlacklistedIPAddress> BlacklistedIPAddresses { get; set; } = new BlacklistedIPAddress[0];
    }
}
