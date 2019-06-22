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

using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalRuby.IPBanProSDK
{
    /// <summary>
    /// IP address country name
    /// </summary>
    [Serializable]
    public struct IPAddressCountryName
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Country code
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// Language code
        /// </summary>
        public string LanguageCode { get; set; }

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns>String</returns>
        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}, Code: {CountryCode}, Lang: {LanguageCode}";
        }

        /// <summary>
        /// Check for equality
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>True if equal, false if not</returns>
        public override bool Equals(object obj)
        {
            if (obj is IPAddressCountryName name)
            {
                return (Id == name.Id && LanguageCode == name.LanguageCode);
            }
            return false;
        }

        /// <summary>
        /// Get hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    /// <summary>
    /// Country names list model
    /// </summary>
    [Serializable]
    public class IPAddressCountryNamesListModel : BaseModel
    {
        /// <summary>
        /// Country names matching the query and language
        /// </summary>
        public IReadOnlyList<IPAddressCountryName> Names { get; set; }

        /// <summary>
        /// The query
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// The language code
        /// </summary>
        public string LanguageCode { get; set; }
    }

    /// <summary>
    /// Country codes to country names model
    /// </summary>
    [Serializable]
    public class CountryCodesToCountryNamesModel : BaseModel
    {
        /// <summary>
        /// Country names matching each country code
        /// </summary>
        public List<string> CountryNames { get; set; }

        /// <summary>
        /// The language code
        /// </summary>
        public string LanguageCode { get; set; }
    }
}
