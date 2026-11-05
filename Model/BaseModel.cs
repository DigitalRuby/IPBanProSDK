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

using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Mvc.ModelBinding;

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
        [DataMember(Order = 1)]
        [NotMapped]
        [BindNever]
        public string Message { get; set; }

        /// <summary>
        /// True if there is an error (check Message), false otherwise
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [DataMember(Order = 2)]
        [NotMapped]
        [BindNever]
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
            JsonNode node = JsonNode.Parse(props);
            return NodeToString(node?[name]);
        }

        /// <summary>
        /// Get a property from a json string
        /// </summary>
        /// <param name="props">Json string</param>
        /// <param name="name">Property name</param>
        /// <returns>Found property value or default if not found/empty</returns>
        public static T GetProp<T>(string props, string name)
        {
            if (string.IsNullOrWhiteSpace(props))
            {
                return default;
            }
            JsonNode node = JsonNode.Parse(props);
            var stringValue = NodeToString(node?[name]);
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return default;
            }
            try
            {
                return (T)Convert.ChangeType(stringValue, typeof(T));
            }
            catch
            {
                return default;
            }
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
            JsonObject obj = JsonNode.Parse(props) as JsonObject ?? new JsonObject();
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
                props = obj.ToJsonString();
            }
            return props;
        }

        public static T SetProp<T>(string props, string name, T value)
        {
            string stringValue = value is null ? null : Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture);
            SetProp(props, name, stringValue);
            return value;
        }

        private static string NodeToString(JsonNode node)
        {
            if (node is null)
            {
                return null;
            }
            if (node is JsonValue val && val.TryGetValue<string>(out string s))
            {
                return s;
            }
            return node.ToJsonString();
        }
    }
}
