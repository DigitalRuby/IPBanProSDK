﻿/*

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace DigitalRuby.IPBanProSDK
{
    /// <summary>
    /// A generic message useful for communciations in a variety of scenarios. Data will most likely be a JToken as json is the standard format used internally.
    /// </summary>
    [Serializable]
    [DataContract]
    public class Message
    {
        /// <summary>
        /// Unique id for the message, null if none
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 1)]
        public string Id { get; set; }

        /// <summary>
        /// Message name
        /// </summary>
        [DataMember(Order = 2)]
        public string Name { get; set; }

        /// <summary>
        /// Message parameters
        /// </summary>
        [DataMember(Order = 3)]
        public List<KeyValuePair<string, object>> Parameters { get; set; }

        /// <summary>
        /// Message data/payload
        /// </summary>
        [DataMember(Order = 4)]
        [ProtoBuf.ProtoMember(4)] // needs dynamic type
        public object Data { get; set; }

        /// <summary>
        /// typeof(Message)
        /// </summary>
        public static Type Type { get; } = typeof(Message);

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns>String</returns>
        public override string ToString()
        {
            string paramString = (Parameters == null ? string.Empty : string.Join(';', Parameters.Select(p => p.Key + ": " + p.Value)));
            return $"Id: {Id}, Name: {Name}, Parameters: {paramString}";
        }
    }
}
