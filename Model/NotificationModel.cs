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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace DigitalRuby.IPBanProSDK
{
    /// <summary>
    /// Represents a notification to send
    /// </summary>
    [Serializable]
    [DataContract]
    public class Notification
    {
        /// <summary>
        /// Subject
        /// </summary>
        [DataMember(Order = 1)]
        public string Subject { get; set; }

        /// <summary>
        /// Body
        /// </summary>
        [DataMember(Order = 2)]
        public string Body { get; set; }

        /// <summary>
        /// Addresses to send the notification to (could be email addresses, user id, device id, etc., depending on notification settings)
        /// </summary>
        [DataMember(Order = 3)]
        public IReadOnlyCollection<string> ToAddresses { get; set; }

        /// <summary>
        /// Extra data
        /// </summary>
        [DataMember(Order = 4)]
        public Dictionary<string, object> Data { get; } = new(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Format a string
        /// </summary>
        /// <param name="context">Context</param>
        /// <param name="text">String</param>
        /// <param name="args">Args</param>
        /// <returns>String</returns>
        public static string FormatString(string context, string text, params object[] args)
        {
            try
            {
                return string.Format(text, args);
            }
            catch (Exception ex)
            {
                Logger.Error("Unable to format string '{0}' ({1}): {2}", text, context, ex);
                return null;
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var to = string.Join(',', ToAddresses ?? []);
            return $"{to} {Subject}";
        }
    }
}
