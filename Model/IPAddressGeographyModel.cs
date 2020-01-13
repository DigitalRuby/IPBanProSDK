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
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace DigitalRuby.IPBanProSDK
{
    /// <summary>
    /// Geography / ISP info for an ip address
    /// </summary>
    [Serializable]
    [DataContract]
    public class IPAddressGeographyModel : BaseModel
    {
        /// <summary>
        /// IP address
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 1)]
        public string IPAddress { get; set; }

        /// <summary>
        /// Geography
        /// </summary>
        [DataMember(Order = 2)]
        public IPAddressGeography Geography { get; set; } = new IPAddressGeography();
    }

    /// <summary>
    /// Geography / ISP info for an ip address
    /// </summary>
    [Serializable]
    [DataContract]
    public class IPAddressGeography
    {
        /// <summary>
        /// City
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 1)]
        public string City { get; set; }

        /// <summary>
        /// Postal code
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 2)]
        public string PostalCode { get; set; }

        /// <summary>
        /// Region
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 3)]
        public string Region { get; set; }

        /// <summary>
        /// Region code
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 4)]
        public string RegionCode { get; set; }

        /// <summary>
        /// Country
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 5)]
        public string Country { get; set; }

        /// <summary>
        /// Country code
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 6)]
        public string CountryCode { get; set; }

        /// <summary>
        /// Continent
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 7)]
        public string Continent { get; set; }

        /// <summary>
        /// Continent code
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 8)]
        public string ContinentCode { get; set; }

        /// <summary>
        /// Latitude
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 9)]
        public float? Latitude { get; set; }

        /// <summary>
        /// Longitude
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 10)]
        public float? Longitude { get; set; }

        /// <summary>
        /// Location accuracy radius
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 11)]
        public int? LocationAccuracyRadius { get; set; }

        /// <summary>
        /// ISP (Internet service provider)
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Order = 12)]
        public string ISP { get; set; }

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns>String</returns>
        public override string ToString()
        {
            return $"{City}, {Region}, {Country}, {ISP}, {Latitude}, {Longitude}";
        }
    }
}
