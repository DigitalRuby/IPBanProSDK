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
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DigitalRuby.IPBanProSDK
{
    /// <summary>
    /// Base model
    /// </summary>
    [Serializable]
    [DataContract]
    public class IPListsMetadataModel : BaseModel
    {
        /// <summary>
        /// Entries
        /// </summary>
        [DataMember(Order = 1)]
        public IPListMetadataEntryModel[] Entries { get; set; }
    }

    /// <summary>
    /// Entry in IPListMetadataModel
    /// </summary>
    public class IPListMetadataEntryModel : BaseModel
    {
        /// <summary>
        /// Unique key for this list
        /// </summary>
        [DataMember(Order = 1)]
        public string Key { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [DataMember(Order = 2)]
        public string Name { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [DataMember(Order = 3)]
        public string Description { get; set; }

        /// <summary>
        /// Count of entries
        /// </summary>
        [DataMember(Order = 4)]
        public int Count { get; set; }
    }

    /// <summary>
    /// IP list model
    /// </summary>
    [Serializable]
    [DataContract]
    public class IPListModel : IPListMetadataEntryModel
    {
        /// <summary>
        /// IP addresses/ranges
        /// </summary>
        [DataMember(Order = 1)]
        public IReadOnlyCollection<string> Entries { get; set; } = Array.Empty<string>();
    }

    /// <summary>
    /// Recent and naughty list count
    /// </summary>
    [Serializable]
    [DataContract]
    public class IPRecentNaughtyCountsModel : BaseModel
    {
        /// <summary>
        /// Recent list count
        /// </summary>
        [DataMember(Order = 1)]
        public int Recent { get; set; }

        /// <summary>
        /// Naughty list count
        /// </summary>
        [DataMember(Order = 2)]
        public int Naughty { get; set; }
    }
}