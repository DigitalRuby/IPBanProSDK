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

using DigitalRuby.IPBan;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Security;
using System.Text;

namespace DigitalRuby.IPBanProSDK
{
    /// <summary>
    /// IPBan Pro SDK Extension Methods
    /// </summary>
    public static class IPBanProSDKExtensionMethods
    {
        private static readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.None,
            NullValueHandling = NullValueHandling.Ignore
        };

        /// <summary>
        /// Get a default json serializer which does not format or indent
        /// </summary>
        /// <returns>JsonSerializer</returns>
        public static JsonSerializer GetJsonSerializer()
        {
            return JsonSerializer.CreateDefault(jsonSerializerSettings);
        }

        /// <summary>
        /// Binary search a list for a key
        /// </summary>
        /// <param name="list">List</param>
        /// <param name="key">Key</param>
        /// <param name="compare">Compare function</param>
        /// <returns>Index of found item, if less than 0 use ~ operator to get index where the next item is the smallest key greater than key</returns>
        public static int BinarySearch<TKey, TValue>(this IList<TValue> list, in TKey key, System.Func<TKey, TValue, int> compare)
        {
            int lo = 0;// index;
            int hi = list.Count - 1;// index + length - 1;
            while (lo <= hi)
            {
                int i = lo + ((hi - lo) >> 1);
                int order = compare(key, list[i]);
                if (order == 0)
                {
                    return i;
                }
                else if (order > 0)
                {
                    lo = i + 1;
                }
                else
                {
                    hi = i - 1;
                }
            }
            return ~lo;
        }

        /// <summary>
        /// Get bytes from an object. Object must be string, SecureString or byte[].
        /// </summary>
        /// <param name="obj">Object</param>
        /// <param name="base64">True to base64 decode strings, false to utf-8 decode.</param>
        /// <returns>Byte[]</returns>
        internal static byte[] BytesFromObject(object obj, bool base64)
        {
            byte[] bytes;
            obj.ThrowIfNull(nameof(obj));
            if (obj is string s)
            {
                bytes = (base64 ? Convert.FromBase64String(s) : Encoding.UTF8.GetBytes(s));
            }
            else if (obj is SecureString ss)
            {
                bytes = (base64 ? Convert.FromBase64String(ss.ToUnsecureString()) : ss.ToUnsecureBytes());
            }
            else if (obj is byte[] b)
            {
                bytes = b;
            }
            else
            {
                throw new ArgumentException($"{nameof(obj)} must be string or SecureString or byte[]");
            }
            return bytes;
        }

        /// <summary>
        /// Get a deflate compressed json message from an object
        /// </summary>
        /// <param name="obj">Object to get compressed bytes for</param>
        /// <returns>Compressed json bytes using deflate compression</returns>
        public static byte[] CreateWegbSocketCompressedJsonMessage(object obj)
        {
            MemoryStream ms = new MemoryStream();
            using (JsonTextWriter jsonWriter = new JsonTextWriter(new StreamWriter(new DeflateStream(ms, CompressionLevel.Optimal, true), IPBanExtensionMethods.Utf8EncodingNoPrefix)))
            {
                if (obj is byte[] bytes)
                {
                    jsonWriter.WriteRaw(Encoding.UTF8.GetString(bytes));
                }
                else if (obj is string text)
                {
                    jsonWriter.WriteRaw(text);
                }
                else
                {
                    JsonSerializer serializer = IPBanProSDKExtensionMethods.GetJsonSerializer();
                    serializer.Serialize(jsonWriter, obj);
                }
            }
            return (ms.GetBuffer().AsSpan(0, (int)ms.Length).ToArray());
        }

        /// <summary>
        /// Parse a json message from a web socket
        /// </summary>
        /// <param name="json">JSON</param>
        /// <returns>Object</returns>
        public static WebSocketMessageResponse ParseWebSocketCompressedJsonMessage(this byte[] json)
        {
            StreamReader reader = new StreamReader(new DeflateStream(new MemoryStream(json), CompressionMode.Decompress), Encoding.UTF8);
            JsonTextReader jsonReader = new JsonTextReader(reader);
            JsonSerializer serializer = IPBanProSDKExtensionMethods.GetJsonSerializer();
            return serializer.Deserialize<WebSocketMessageResponse>(jsonReader);
        }

        /// <summary>
        /// Truncate a string to max length, adding ellipsis if needed. Used for monospace font scenarios only. CJK is counted as 5.0f / 3.0f chars.
        /// </summary>
        /// <param name="s">String</param>
        /// <param name="maxLength">Max length</param>
        /// <param name="padding">True to pad with spaces</param>
        /// <returns>Truncated string or original string if no truncation</returns>
        public static string Truncate(this string s, int maxLength, bool padding = false)
        {
            const float cjkRatio = 5.0f / 3.0f;
            if (s != null && maxLength > 0)
            {
                float charCount = 0.0f;
                for (int i = 0; i < s.Length; i++)
                {
                    char c = s[i];
                    switch (char.GetUnicodeCategory(c))
                    {
                        case System.Globalization.UnicodeCategory.Format:
                        case System.Globalization.UnicodeCategory.ModifierLetter:
                        case System.Globalization.UnicodeCategory.ModifierSymbol:
                        case System.Globalization.UnicodeCategory.NonSpacingMark:
                        case System.Globalization.UnicodeCategory.SpacingCombiningMark:
                        case System.Globalization.UnicodeCategory.Surrogate:
                            break;

                        default:
                            if (c.IsCJK())
                            {
                                charCount += cjkRatio;
                            }
                            else
                            {
                                charCount++;
                            }
                            break;
                    }
                    if (charCount > maxLength)
                    {
                        charCount = maxLength;
                        s = s.Substring(0, i - 1) + "…";
                        break;
                    }
                }
                if (padding)
                {
                    s += new string(' ', maxLength - (int)charCount);
                }
            }
            return s;
        }

        /// <summary>
        /// Detect if char is Chinese, Japanese or Korean
        /// </summary>
        /// <param name="c">Char to check</param>
        /// <returns>True if CJK, false if not</returns>
        public static bool IsCJK(this char c)
        {
            return (c >= 0x4E00 && c <= 0x2FA1F);
        }
    }
}
