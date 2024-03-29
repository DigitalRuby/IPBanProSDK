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

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DigitalRuby.IPBanProSDK
{
    /// <summary>
    /// IP address country name
    /// </summary>
    [Serializable]
    [DataContract]
    public struct IPAddressCountryName
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Order = 1)]
        public int Id { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [DataMember(Order = 2)]
        public string Name { get; set; }

        /// <summary>
        /// Country code
        /// </summary>
        [DataMember(Order = 3)]
        public string CountryCode { get; set; }

        /// <summary>
        /// Language code
        /// </summary>
        [DataMember(Order = 4)]
        public string LanguageCode { get; set; }

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns>String</returns>
        public override readonly string ToString()
        {
            return $"Id: {Id}, Name: {Name}, Code: {CountryCode}, Lang: {LanguageCode}";
        }

        /// <summary>
        /// Check for equality
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>True if equal, false if not</returns>
        public override readonly bool Equals(object obj)
        {
            if (obj is IPAddressCountryName name)
            {
                return (Id == name.Id && LanguageCode == name.LanguageCode);
            }
            return false;
        }

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="r1">Name1</param>
        /// <param name="r2">Name2</param>
        /// <returns>True if equal</returns>
        public static bool operator ==(IPAddressCountryName r1, IPAddressCountryName r2)
        {
            return r1.Equals(r2);
        }

        /// <summary>
        /// Not equals
        /// </summary>
        /// <param name="r1">Name1</param>
        /// <param name="r2">Name2</param>
        /// <returns>True if not equal</returns>
        public static bool operator !=(IPAddressCountryName r1, IPAddressCountryName r2)
        {
            return !r1.Equals(r2);
        }

        /// <summary>
        /// Get hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override readonly int GetHashCode()
        {
            return Id;
        }
    }

    /// <summary>
    /// Country names list model
    /// </summary>
    [Serializable]
    [DataContract]
    public class IPAddressCountryNamesListModel : BaseModel
    {
        /// <summary>
        /// Country names matching the query and language
        /// </summary>
        [DataMember(Order = 1)]
        public IReadOnlyList<IPAddressCountryName> Names { get; set; }

        /// <summary>
        /// The query
        /// </summary>
        [DataMember(Order = 2)]
        public string Query { get; set; }

        /// <summary>
        /// The language code
        /// </summary>
        [DataMember(Order = 3)]
        public string LanguageCode { get; set; }
    }

    /// <summary>
    /// Country codes to country names model
    /// </summary>
    [Serializable]
    [DataContract]
    public class CountryCodesToCountryNamesModel : BaseModel
    {
        /// <summary>
        /// Country names matching each country code
        /// </summary>
        [DataMember(Order = 1)]
        public List<string> CountryNames { get; set; }

        /// <summary>
        /// The language code
        /// </summary>
        [DataMember(Order = 2)]
        public string LanguageCode { get; set; }
    }
}
