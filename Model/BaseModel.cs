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

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Runtime.Serialization;

namespace DigitalRuby.IPBanProSDK
{
    /// <summary>
    /// Base model
    /// </summary>
    [Serializable]
    [DataContract]
    public class BaseModel
    {
        /// <summary>
        /// Message if any
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 1)]
        public string Message { get; set; }

        /// <summary>
        /// True if there is an error (check Message), false otherwise
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 2)]
        public bool Error { get; set; }
    }

    /// <summary>
    /// Prop extensions for get/set properties on json strings
    /// </summary>
    public static class PropHelper
    {
        /// <summary>
        /// Get a property from a json string
        /// </summary>
        /// <param name="props">Json string</param>
        /// <param name="name">Property name</param>
        /// <returns>Found property value or null if not found/empty</returns>
        public static string GetProp(string props, string name)
        {
            if (string.IsNullOrWhiteSpace(props))
            {
                return null;
            }
            JToken token = JToken.Parse(props);
            return token[name]?.ToString();
        }

        /// <summary>
        /// Set a property on a json string
        /// </summary>
        /// <param name="props">Json string</param>
        /// <param name="name">Name of the property</param>
        /// <param name="value">Value of the property, pass null to remove</param>
        /// <returns>The updated props value</returns>
        public static string SetProp(string props, string name, string value)
        {
            if (string.IsNullOrWhiteSpace(props))
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return props;
                }
                props = "{}";
            }
            JObject obj = JObject.Parse(props);
            if (string.IsNullOrWhiteSpace(value))
            {
                obj.Remove(name);
            }
            else
            {
                obj[name] = value;
            }
            if (obj.Count == 0)
            {
                props = null;
            }
            else
            {
                props = obj.ToString();
            }
            return props;
        }
    }
}
