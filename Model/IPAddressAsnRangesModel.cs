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
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DigitalRuby.IPBanProSDK
{
    /// <summary>
    /// Asn ranges model
    /// </summary>
    [Serializable]
    [DataContract]
    public class IPAddressAsnRangesModel : BaseModel
    {
        /// <summary>
        /// Country ranges
        /// </summary>
        [DataMember(Order = 1)]
        public IPAddressAsnRanges AsnRanges { get; set; }
    }

    /// <summary>
    /// A country and it's names an ip address ranges
    /// </summary>
    [Serializable]
    [DataContract]
    public class IPAddressAsnRanges
    {
        /// <summary>
        /// Geo name id
        /// </summary>
        [DataMember(Order = 1)]
        public int Id { get; set; }

        /// <summary>
        /// Two letter iso code
        /// </summary>
        [DataMember(Order = 2)]
        public string Name { get; set; }

        /// <summary>
        /// All ip addresses
        /// </summary>
        [DataMember(Order = 3)]
        public List<string> Ranges { get; set;  } = [];
    }
}
