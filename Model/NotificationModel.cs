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

using DigitalRuby.IPBanCore;
using System;
using System.Collections.Generic;

namespace DigitalRuby.IPBanProSDK
{
    /// <summary>
    /// Represents a notification to send
    /// </summary>
    [Serializable]
    public class Notification
    {
        /// <summary>
        /// Subject
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Body
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Addresses to send the notification to (could be email addresses, user id, device id, etc., depending on notification settings)
        /// </summary>
        public IReadOnlyCollection<string> ToAddresses { get; set; }

        /// <summary>
        /// Format a template
        /// </summary>
        /// <param name="template">Template</param>
        /// <param name="machineName">Machine name</param>
        /// <param name="userName">User name</param>
        /// <param name="machineIp">Machine ip</param>
        /// <param name="bannedIp">Banned ip address</param>
        /// <param name="bannedIpHost">Remote host</param>
        /// <param name="bannedIpCity">City</param>
        /// <param name="bannedIpRegion">Region</param>
        /// <param name="bannedIpCountry">Country</param>
        /// <param name="type">Event type</param>
        /// <returns></returns>
        public static string FormatTemplate(string template, string machineName, string userName, string machineIp, string bannedIp, string bannedIpHost,
            string bannedIpCity, string bannedIpRegion, string bannedIpCountry, IPAddressEventType type)
        {
            // Subject template for sent mail. {0} = machine name, {1} = userName, {2} = machine ip address, {3} = banned ip address, {4} = remote host name, {5} = city, {6} = region, {7} = country, {8} = type
            return string.Format(template, machineName, userName, machineIp, bannedIp, bannedIpHost, bannedIpCity, bannedIpRegion, bannedIpCountry, type);
        }
    }
}
