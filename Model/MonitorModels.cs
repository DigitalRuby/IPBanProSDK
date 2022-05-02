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

        /// <summary>
        /// Network usage (0-1)
        /// </summary>
        public float NetworkUsage { get; init; }

        /// <summary>
        /// Storage IO usage (0-1) - this is the percent of total possible IOPS that current storage can handle given current load
        /// </summary>
        public float StorageIopsUsage { get; init; }
    }

    /// <summary>
    /// Base monitor country model
    /// </summary>
    public class MonitorCountryModelBase
    {
        /// <summary>
        /// Country code
        /// </summary>
        public string CountryCode { get; init; }

        /// <summary>
        /// Dropped packet count
        /// </summary>
        public long DropCount { get; set; }

        /// <summary>
        /// Allowed packet count
        /// </summary>
        public long AllowCount { get; set; }
    }

    /// <summary>
    /// Country monitor data
    /// </summary>
    public class MonitorCountryModel : MonitorCountryModelBase
    {
        /// <summary>
        /// Timestamp
        /// </summary>
        public DateTime TimeStamp { get; init; }
    }

    /// <summary>
    /// Machine names model
    /// </summary>
    public class MachineNamesModel : BaseMonitorModel
    {
        /// <summary>
        /// Machine names
        /// </summary>
        public System.Collections.Generic.SortedSet<string> MachineNames { get; init; }

        /// <summary>
        /// Aggregate value, 0 for nothing, 1 for everything otherwise ,fqdn1,fqdn2, format
        /// </summary>
        public string AggregateValue { get; init; } = "0";

        /// <summary>
        /// Create a machine name from an fqdn and ip address
        /// </summary>
        /// <param name="fqdn">FQDN</param>
        /// <param name="ipAddress">IP address</param>
        /// <returns></returns>
        public static string CreateMachineName(string fqdn, string ipAddress)
        {
            return fqdn + "-" + ipAddress;
        }
    }

    /// <summary>
    /// Update monitoring
    /// </summary>
    public class UpdateMonitoringModel : BaseModel
    {
        /// <summary>
        /// Whether to enable monitoring (0 = off, 1 = on, or use a machine names / ips for targetted monitoring)
        /// </summary>
        public string MonitoringEnabled { get; init; }
    }
}
