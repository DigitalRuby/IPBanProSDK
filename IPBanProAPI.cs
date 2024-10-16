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

#region Imports

using DigitalRuby.IPBanCore;
using DigitalRuby.IPBanProSDK.Model.WhitelistUrls;

using System;
using System.Threading.Tasks;
using System.Web;

#endregion Imports

namespace DigitalRuby.IPBanProSDK
{
    /// <summary>
    /// IPBanPro API wrapper
    /// </summary>
    public class IPBanProAPI : IPBanProBaseAPI
    {
        /// <summary>
        /// API uri
        /// </summary>
        public override Uri BaseUri { get; set; } = new Uri("https://api.ipban.com");

        /// <summary>
        /// Public: Get a list of all possible country names
        /// </summary>
        /// <returns>List of all possible country names</returns>
        public async Task<IPAddressCountryNamesModel> GetCountryNamesAsync()
        {
            return await MakeRequestAsync<IPAddressCountryNamesModel>("CountryNames");
        }

        /// <summary>
        /// Public: Get a list of all possible asn names
        /// </summary>
        /// <returns>List of all possible asn names</returns>
        public async Task<IPAddressAsnNamesModel> GetAsnNamesAsync()
        {
            return await MakeRequestAsync<IPAddressAsnNamesModel>("AsnNames");
        }

        /// <summary>
        /// Public: Get a list of countries that match a query
        /// </summary>
        /// <param name="languageCode">Language code</param>
        /// <param name="query">Query</param>
        /// <returns>List of all countries that match the query</returns>
        public async Task<IPAddressCountryNamesListModel> GetCountryNamesListAsync(string languageCode, string query)
        {
            string url = "CountryNamesList?languageCode=" + languageCode.UrlEncode() + "&query=" + query.UrlEncode();
            return await MakeRequestAsync<IPAddressCountryNamesListModel>(url);
        }

        /// <summary>
        /// Public: Get a list of asn that match a query
        /// </summary>
        /// <param name="languageCode">Language code</param>
        /// <param name="query">Query</param>
        /// <returns>List of all asn that match the query</returns>
        public async Task<IPAddressAsnNamesListModel> GetAsnNamesListAsync(string languageCode, string query)
        {
            string url = "AsnNamesList?languageCode=" + languageCode.UrlEncode() + "&query=" + query.UrlEncode();
            return await MakeRequestAsync<IPAddressAsnNamesListModel>(url);
        }

        /// <summary>
        /// Public: Get ip address geography, including city, location and ISP if available
        /// </summary>
        /// <param name="ipAddress">IP address</param>
        /// <returns>IP address geography</returns>
        public async Task<IPAddressGeographyModel> GetIPAddressGeographyAsync(string ipAddress)
        {
            if (!System.Net.IPAddress.TryParse(ipAddress, out System.Net.IPAddress ipAddressObj) ||
                ipAddressObj.IsInternal())
            {
                return new IPAddressGeographyModel { IPAddress = ipAddress };
            }
            return await MakeRequestAsync<IPAddressGeographyModel>("IP/" + ipAddress);
        }

        /// <summary>
        /// Public: Get ip address country
        /// </summary>
        /// <param name="ipAddress">IP address</param>
        /// <returns>IP address country only geography</returns>
        public async Task<IPAddressGeographyModel> GetIPAddressCountryGeographyAsync(string ipAddress)
        {
            if (!System.Net.IPAddress.TryParse(ipAddress, out System.Net.IPAddress ipAddressObj) ||
                ipAddressObj.IsInternal())
            {
                return new IPAddressGeographyModel { IPAddress = ipAddress };
            }
            return await MakeRequestAsync<IPAddressGeographyModel>("IPCountry/" + ipAddress);
        }

        /// <summary>
        /// Private: Retrieve the naughty list
        /// </summary>
        /// <returns>Naughty list, this is a curated list of the most banned or other problem ip addresses</returns>
        public Task<BannedIPAddressesModel> GetNaughtyListAsync()
        {
            return MakeRequestAsync<BannedIPAddressesModel>("IPNaughtyList?ranges=1");
        }

        /// <summary>
        /// Private: Retrieve the recent ban list
        /// </summary>
        /// <returns>Recent ban list, contains the most recently banned ip addresses</returns>
        public Task<RecentBannedIPAddressesModel> GetRecentBanListAsync()
        {
            return MakeRequestAsync<RecentBannedIPAddressesModel>("IPRecentList?ranges=1");
        }

        /// <summary>
        /// Get ip list information
        /// </summary>
        /// <returns>IP list information</returns>
        public Task<IPListsMetadataModel> GetIPListsMetadataAsync()
        {
            return MakeRequestAsync<IPListsMetadataModel>("iplistsmetadata");
        }

        /// <summary>
        /// Private: Get an ip list
        /// </summary>
        /// <param name="key">Key of the list</param>
        /// <returns>IP list</returns>
        public Task<IPListModel> GetIPList(string key)
        {
            return MakeRequestAsync<IPListModel>("iplist?key=" + HttpUtility.UrlEncode(key));
        }

        /// <summary>
        /// Get counts of items in recent and naughty list
        /// </summary>
        /// <returns>Counts of recent and naughty list</returns>
        public Task<IPRecentNaughtyCountsModel> GetRecentNaughtyCounts()
        {
            return MakeRequestAsync<IPRecentNaughtyCountsModel>("iprecentnaughtylistcount");
        }

        /// <summary>
        /// Get country ip address codes
        /// </summary>
        /// <param name="code">Country or state code (state codes must include the country code with a hyphen prefix)</param>
        /// <returns>IP address ranges for the country</returns>
        public Task<IPAddressCountryRangesModel> GetIPAddressCountryRangesAsync(string code)
        {
            return MakeRequestAsync<IPAddressCountryRangesModel>($"IPCountryRanges/{code}");
        }

        /// <summary>
        /// Get asn ranges
        /// </summary>
        /// <param name="asn">Asn id or name without AS or ASN prefix</param>
        /// <returns>IP address ranges for the asn</returns>
        public Task<IPAddressAsnRangesModel> GetIPAddressAsnRangesAsync(string asn)
        {
            return MakeRequestAsync<IPAddressAsnRangesModel>($"IPAsnRanges/{asn}");
        }

        /// <summary>
        /// Get asn info
        /// </summary>
        /// <returns>Asn info</returns>
        public Task<IPAddressAsnInfoModel> GetIPAddressAsnInfoAsync()
        {
            return MakeRequestAsync<IPAddressAsnInfoModel>($"IPAsnInfo");
        }

        /// <summary>
        /// Get defined whitelist urls
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Response</returns>
        public Task<GetUrlsResponse> WhitelistGetUrls(GetUrlsRequest request)
        {
            var id = string.IsNullOrWhiteSpace(request.Id) ? null : "id=" + HttpUtility.UrlEncode(request.Id);
            var query = string.Empty;
            if (id is not null)
            {
                query = "?" + id;
            }
            return MakeRequestAsync<GetUrlsResponse>($"aw/urls{query}");
        }

        /// <summary>
        /// Upsert a new whitelist url for consumption
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Response</returns>
        public Task<UpsertUrlResponse> UpsertWhitelistUrl(UpsertUrlRequest request)
        {
            return MakeRequestAsync<UpsertUrlResponse>($"aw/upsert", request);
        }

        /// <summary>
        /// Delete a whitelist url so it can no longer be consumed. Any consumptions for the url are also deleted.
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Response</returns>
        public Task<DeleteUrlResponse> DeleteWhitelistUrl(DeleteUrlRequest request)
        {
            return MakeRequestAsync<DeleteUrlResponse>($"aw/delete", request, System.Net.Http.HttpMethod.Delete.Method);
        }

        /// <summary>
        /// Consume a url so that an entry is created with the callers ip address to allow access
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Response</returns>
        public Task<ConsumeUrlResponse> ConsumeUrl(ConsumeUrlRequest request)
        {
            var query = "?a=" + HttpUtility.UrlEncode(request.GroupId) + "&b=" + HttpUtility.UrlEncode(request.Id);
            return MakeRequestAsync<ConsumeUrlResponse>($"aw/use{query}");
        }
    }

    /// <summary>
    /// Types of web socket subscriptions for IPBan Pro API
    /// </summary>
    public enum IPBanProAPIWebSocketSubscription
    {
        /// <summary>
        /// Stream of recent banned ip addresses
        /// </summary>
        RecentBannedIPAddresses = 1000,

        /// <summary>
        /// Stream of whitelist url changes
        /// </summary>
        WhitelistUrlChange = 1001
    }
}
