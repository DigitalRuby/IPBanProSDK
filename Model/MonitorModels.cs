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
using System.Runtime.Serialization;

namespace DigitalRuby.IPBanProSDK
{
    /// <summary>
    /// Monitor types
    /// </summary>
    public enum MonitorType
    {
        /// <summary>
        /// System performance counters
        /// </summary>
        System = 1
    }

    /// <summary>
    /// Base monitor model
    /// </summary>
    public abstract class BaseMonitorModel : BaseModel
    {
        /// <summary>
        /// Type of monitor
        /// </summary>
        public MonitorType Type { get; init; }
    }

    /// <summary>
    /// Monitor model for performance counter
    /// </summary>
    [Serializable]
    [DataContract]
    public class MonitorPerformanceCounter : BaseMonitorModel
    {
        /// <summary>
        /// Fully qualified domain name
        /// </summary>
        public string FQDN { get; init; }

        /// <summary>
        /// IP address
        /// </summary>
        public string IPAddress { get; init; }

        /// <summary>
        /// Memory total
        /// </summary>
        public long MemoryTotal { get; init; }

        /// <summary>
        /// Memory available
        /// </summary>
        public long MemoryAvailable { get; init; }

        /// <summary>
        /// Storage total (all drives)
        /// </summary>
        public long StorageTotal { get; init; }

        /// <summary>
        /// Storage available (all drives)
        /// </summary>
        public long StorageAvailable { get; init; }

        /// <summary>
        /// Cpu usage (0-1)
        /// </summary>
        public float CpuUsage { get; init; }
    }
}
