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

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

#endregion Imports

namespace DigitalRuby.IPBanProSDK
{
    /// <summary>
    /// IPBanPro base API wrapper
    /// </summary>
    public class IPBanProBaseAPI : IDisposable
    {
        /// <summary>
        /// Web socket group name for commands
        /// </summary>
        public const string WebSocketGroupNameCommands = "WebSocketGroupIdCommands";

        /// <summary>
        /// No group id, always send
        /// </summary>
        public const int WebSocketGroupIdNone = 0;

        /// <summary>
        /// Web socket group id for commands, commands are always sent regardless of whether the client is subscribed
        /// </summary>
        public const int WebSocketGroupIdCommands = 2000;

        /// <summary>
        /// A message that denotes that a received message has been received and finished processing
        /// </summary>
        public const string MessageAck = "ack";

        /// <summary>
        /// A text message
        /// </summary>
        public const string MessageText = "text";

        /// <summary>
        /// Subscribe message name
        /// </summary>
        public const string MessageSubscribe = "subscribe";

        /// <summary>
        /// Unsubscribe message name
        /// </summary>
        public const string MessageUnsubscribe = "unsubscribe";

        /// <summary>
        /// Heartbeat message name
        /// </summary>
        public const string MessagePushHeartbeat = "push_heartbeat";

        /// <summary>
        /// Config message name
        /// </summary>
        public const string MessagePushConfig = "push_config";

        /// <summary>
        /// API key message name
        /// </summary>
        public const string MessagePushAPIKey = "push_api_key";

        /// <summary>
        /// Push an ip address event out
        /// </summary>
        public const string MessagePushIPAddressEvent = "push_ipaddress_event";

        /// <summary>
        /// Push the local blacklist
        /// </summary>
        public const string MessagePushLocalBlacklist = "push_local_blacklist";

        /// <summary>
        /// Push the global naughty list
        /// </summary>
        public const string MessagePushNaughtyBlacklist = "push_naughty_blacklist";

        /// <summary>
        /// Push the global recent blacklist
        /// </summary>
        public const string MessagePushRecentBlacklist = "push_recent_blacklist";

        /// <summary>
        /// Push a country blacklist changed notification
        /// </summary>
        public const string MessagePushCountryBlacklistChanged = "push_country_blacklist_changed";

        /// <summary>
        /// Push an other list change
        /// </summary>
        public const string MessagePushOtherList = "push_other_blacklist";

        /// <summary>
        /// Push a whitelist
        /// </summary>
        public const string MessagePushWhitelist = "push_whitelist";

        /// <summary>
        /// Push a software update
        /// </summary>
        public const string MessagePushSoftwareUpdate = "push_software_update";

        /// <summary>
        /// Push new credentials for clients
        /// </summary>
        public const string MessagePushCredentials = "push_credentials";

        /// <summary>
        /// Push an action, the client should perform the action with the data. Parameters contains 'action' key and the value is the action to perform.
        /// </summary>
        public const string MessagePushAction = "push_action";

        /// <summary>
        /// Push a packet event to the server
        /// </summary>
        public const string MessagePushPacketEvent = "push_packet_event";

        /// <summary>
        /// Push a monitor change to a client
        /// </summary>
        public const string MessagePushMonitorEnabled = "push_monitor_enabled";

        /// <summary>
        /// Client pushing monitor data to server
        /// </summary>
        public const string MessagePushMonitorData = "push_monitor_data";

        /// <summary>
        /// Push firewall rules
        /// </summary>
        public const string MessagePushFirewallRules = "push_firewall_rules";

        /// <summary>
        /// Push a request for firewall rules
        /// </summary>
        public const string MessagePushRequestFirewallRules = "push_request_firewall_rules";

        /// <summary>
        /// Push whitelist url change
        /// </summary>
        public const string MessagePushWhitelistUrl = "push_whitelist_url";

        /// <summary>
        /// Push pro config
        /// </summary>
        public const string MessagePushProConfig = "push_pro_config";

        /// <summary>
        /// Push log
        /// </summary>
        public const string MessagePushLog = "push_log";

        /// <summary>
        /// Push reset machine guid
        /// </summary>
        public const string MessagePushResetMachineGuid = "push_reset_machine_guid";

        /// <summary>
        /// Key for ip address
        /// </summary>
        public const string KeyIPAddress = "IPAddress";

        /// <summary>
        /// Key for count
        /// </summary>
        public const string KeyCount = "Count";

        /// <summary>
        /// Key for allow ports
        /// </summary>
        public const string KeyAllowedPorts = "AllowedPorts";

        /// <summary>
        /// Key for countries
        /// </summary>
        public const string KeyCountries = "Countries";

        /// <summary>
        /// Key for country
        /// </summary>
        public const string KeyCountry = "Country";

        /// <summary>
        /// Key for country ranges
        /// </summary>
        public const string KeyCountryRanges = "CountryRanges";

        /// <summary>
        /// Key for ranges
        /// </summary>
        public const string KeyRanges = "Ranges";

        private class SetKeysScopedDisposable : IDisposable
        {
            private readonly IPBanProBaseAPI api;

            public SetKeysScopedDisposable(IPBanProBaseAPI api, SecureString publicApiKey, SecureString privateApiKey)
            {
                this.api = api;
                api.PublicApiKey = publicApiKey;
                api.PrivateApiKey = privateApiKey;
            }

            public void Dispose()
            {
                api.PublicApiKey = null;
                api.PrivateApiKey = null;
            }
        }

        /// <summary>
        /// Api key header: X-IPBAN-API-KEY
        /// </summary>
        public const string HeaderApiKey = "X-IPBAN-API-KEY";

        /// <summary>
        /// Api signature header: X-IPBAN-SIGNATURE
        /// </summary>
        public const string HeaderSignature = "X-IPBAN-SIGNATURE";

        /// <summary>
        /// Api origin header: X-IPBAN-ORIGIN
        /// </summary>
        public const string HeaderOrigin = "X-IPBAN-ORIGIN";

        /// <summary>
        /// Api timestamp header: X-IPBAN-TIMESTAMP
        /// </summary>
        public const string HeaderTimestamp = "X-IPBAN-TIMESTAMP";

        /// <summary>
        /// Get or set the base API url
        /// </summary>
        public virtual Uri BaseUri { get; set; }

        /// <summary>
        /// Allow override of BaseUri for all constructor calls. If null, no override is done. Great for unit testing.
        /// </summary>
        public static Dictionary<Type, Uri> BaseUriOverride { get; } = [];

        /// <summary>
        /// Public API key (required for private API use)
        /// </summary>
        public SecureString PublicApiKey { get; set; }

        /// <summary>
        /// Private API key (optional, can be provided for signing requests)
        /// </summary>
        public SecureString PrivateApiKey { get; set; }

        /// <summary>
        /// Authorization header (null if none).
        /// </summary>
        /// <see cref="IPBanProSDK.IPBanProBaseAPI.CreateBasicAuthorization"/>
        public SecureString Authorization { get; set; }

        /// <summary>
        /// Allowed ip addresses (comma separated) or * for all. Ranges allowed.
        /// </summary>
        public string AllowedIPAddresses { get; set; } = "*";

        /// <summary>
        /// User agent. Default is IPBanProSDK.
        /// </summary>
        public string UserAgent { get; set; } = "IPBanProSDK";

        /// <summary>
        /// Whether to disable cache. This should be left enabled generally.
        /// </summary>
        public static bool DisableCache { get; set; }

        private DateTime? timestamp;

        /// <summary>
        /// Get or set the timestamp to use for requests, default(DateTime) for default
        /// </summary>
        public DateTime Timestamp
        {
            get => timestamp ?? IPBanService.UtcNow;
            set => timestamp = (value == default ? null : (DateTime?)value);
        }

        private IHttpRequestMaker requestMaker = DefaultHttpRequestMaker.Instance;
        private readonly List<KeyValuePair<string, object>> headers = [];

        /// <summary>
        /// The request maker. Defaults to a standard http request maker.
        /// </summary>
        public IHttpRequestMaker RequestMaker
        {
            get => requestMaker;
            set => requestMaker = value ?? throw new ArgumentNullException(nameof(value), "Request maker must not be null");
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public IPBanProBaseAPI()
        {
            if (BaseUriOverride.TryGetValue(GetType(), out Uri uriOverride))
            {
                BaseUri = uriOverride;
            }
        }

        /// <summary>
        /// Dispose of the api
        /// </summary>
        public void Dispose() => GC.SuppressFinalize(this);

        /// <summary>
        /// Create a signature data string that can then be used to compute a signature
        /// </summary>
        /// <param name="uri">Uri</param>
        /// <param name="timestamp">Timestamp (unix milliseconds)</param>
        /// <param name="publicApiKey">Public api key</param>
        /// <param name="ipAddress">IP address or '*' for any ip address</param>
        /// <returns>Signature data string</returns>
        public static string CreateSignatureDataString(Uri uri, string timestamp, SecureString publicApiKey, string ipAddress = "*")
        {
            uri.ThrowIfNull(nameof(uri));
            timestamp.ThrowIfNull(nameof(timestamp));
            publicApiKey.ThrowIfNull(nameof(publicApiKey));
            ipAddress.ThrowIfNull(nameof(ipAddress));
            if (!uri.IsAbsoluteUri)
            {
                throw new ArgumentException("Uri must be an absolute uri");
            }

            string pathAndQuery = uri.PathAndQuery;

            // strip of token for redirect handling of signature
            pathAndQuery = Regex.Replace(pathAndQuery, @"[\?\&]token=[A-Za-z0-9,_\-]+", string.Empty,
                RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

            string data = pathAndQuery + "|" + timestamp + "|" + publicApiKey.ToUnsecureString();
            return data;
        }

        /// <summary>
        /// Compute a signature from a request url, timestamp and public api key and private api key.
        /// Signature logic:
        /// Create a concatenated string from the following, in order, delimited by pipe |
        /// - Request path + query string, with url encoding, i.e. /EndPoint?param1=param1Value&param2=param2%20Value (include the leading slash, DO NOT include the scheme and host).
        /// - Current unix timestamp in milliseconds (UTC), i.e. 1548014026783
        /// - Get the remote ip address string of the machine making the request, i.e. 100.100.100.100 or a * to not protect the request with the ip address
        /// - Public api key string
        /// - Final concatenated string example: /EndPoint?param1=param1Value&param2=param2%20Value|1548014026783|100.100.100.100|PUBLIC_API_KEY
        /// Take the string and call ComputeSignature on it, using the private api key to sign
        /// - string signature = ComputeSignature(finalString, privateKey);
        /// - Ensure that signature is set as X-SIGNATURE header in ALL https:// and wss:// requests
        /// </summary>
        /// <param name="uri">Uri</param>
        /// <param name="timestamp">Timestamp (unix milliseconds)</param>
        /// <param name="publicApiKey">Public api key</param>
        /// <param name="privateApiKey">Private api key</param>
        /// <param name="ipAddress">IP address or '*' for any ip address</param>
        /// <returns>Signature</returns>
        /// <exception cref="System.ArgumentException">Invalid parameters</exception>
        public static string ComputeSignature(Uri uri, string timestamp, SecureString publicApiKey, SecureString privateApiKey, string ipAddress = "*")
        {
            // rest of param are checked in CreateSignatureDataString
            privateApiKey.ThrowIfNull(nameof(privateApiKey));

            string data = CreateSignatureDataString(uri, timestamp, publicApiKey, ipAddress);
            return IPBanProCryptography.ComputeSignature(data, privateApiKey);
        }

        /// <summary>
        /// Verify an API call signature. See ComputeSignature for implementation details.
        /// </summary>
        /// <param name="uri">Uri</param>
        /// <param name="timestamp">Timestamp</param>
        /// <param name="publicApiKey">Public api key</param>
        /// <param name="signature">Signature</param>
        /// <param name="ipAddress">IP address or '*' for any ip address</param>
        /// <returns>True if verified, false if not</returns>
        public static bool VerifySignature(Uri uri, string timestamp, SecureString publicApiKey, string signature, string ipAddress = "*")
        {
            // rest of param are checked in CreateSignatureDataString
            signature.ThrowIfNullOrWhiteSpace(nameof(signature));

            string data = CreateSignatureDataString(uri, timestamp, publicApiKey, ipAddress);
            return IPBanProCryptography.VerifySignature(data, publicApiKey, signature);
        }

        /// <summary>
        /// Create a basic authorization header
        /// </summary>
        /// <param name="userName">User name</param>
        /// <param name="password">Password</param>
        /// <returns>Authorization string</returns>
        public static string CreateBasicAuthorization(string userName, string password)
        {
            return "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(userName + ":" + password));
        }

        /// <summary>
        /// Re-encode basic auth header with SHA256 pasword
        /// </summary>
        /// <param name="header">Header</param>
        /// <param name="userName">Receives the user name</param>
        /// <param name="password">Receives the password</param>
        /// <returns>Re-encoded auth header</returns>
        public static string ReEncodeBasicAuthorization(string header, out string userName, out string password)
        {
            if (string.IsNullOrWhiteSpace(header) || header.Length < "Basic ".Length)
            {
                userName = null;
                password = null;
                return header;
            }

            header = header["Basic ".Length..];
            byte[] base64Bytes = Convert.FromBase64String(header);
            string base64String = Encoding.UTF8.GetString(base64Bytes);
            string[] pieces = base64String.Split(':');
            if (pieces.Length != 2)
            {
                userName = "Unknown";
                password = null;
                return header;
            }
            userName = pieces[0];
            password = pieces[1];
            return CreateBasicAuthorization(userName, password.ToSHA256String());
        }

        /// <summary>
        /// Get http headers needed to make an API request
        /// </summary>
        /// <param name="uri">Uri</param>
        /// <returns>Http headers</returns>
        public ICollection<KeyValuePair<string, object>> GetApiRequestHeaders(Uri uri)
        {
            List<KeyValuePair<string, object>> headers = [];
            if (PublicApiKey != null)
            {
                headers.Add(new KeyValuePair<string, object>(HeaderApiKey, PublicApiKey));
                if (PrivateApiKey != null)
                {
                    string timestamp = Timestamp.ToUnixMillisecondsLong().ToString(CultureInfo.InvariantCulture);
                    string signature = IPBanProAPI.ComputeSignature(uri, timestamp, PublicApiKey, PrivateApiKey, AllowedIPAddresses);
                    if (signature != null)
                    {
                        headers.Add(new KeyValuePair<string, object>(HeaderSignature, signature));
                    }
                    headers.Add(new KeyValuePair<string, object>(HeaderOrigin, AllowedIPAddresses));
                    headers.Add(new KeyValuePair<string, object>(HeaderTimestamp, timestamp));
                }
            }
            if (Authorization != null)
            {
                headers.Add(new KeyValuePair<string, object>("Authorization", Authorization));
            }
            headers.Add(new KeyValuePair<string, object>("User-Agent", UserAgent));
            if (DisableCache)
            {
                headers.Add(new KeyValuePair<string, object>("Cache-Control", "no-store, no-cache"));
            }
            return headers;
        }

        /// <summary>
        /// Setup a web socket to connect to the api. This method will then call Start on the web socket.
        /// </summary>
        public void StartWebSocket(ClientWebSocket socket)
        {
            socket.RequestHeaders = GetApiRequestHeaders(socket.Uri);
            socket.Start();
        }

        /// <summary>
        /// Make a raw request to the API.
        /// X-IPBAN-TIMESTAMP will be set to unix milliseconds from Timestamp property
        /// X-IPBAN-SIGNATURE will be set if public and private api key are not null using CalculateSignature
        /// X-IPBAN-APIKEY will be set to public api key if both private api key and public api key are not null
        /// </summary>
        /// <typeparam name="T">Type of return value</typeparam>
        /// <param name="pathAndQuery">Path and query to request, not including the root scheme and host</param>
        /// <param name="postJson">Optional data to POST, if null GET is used instead</param>
        /// <param name="method">Method or null for default</param>
        /// <returns>Task of T</returns>
        /// <exception cref="HttpRequestException">Any error</exception>
        public async Task<T> MakeRequestAsync<T>(string pathAndQuery, object postJson = null, string method = null) where T : BaseModel
        {
            byte[] response = [];
            try
            {
                await new SynchronizationContextRemover();
                Uri uri = new(BaseUri, pathAndQuery);
                ICollection<KeyValuePair<string, object>> headers = GetApiRequestHeaders(uri);
                byte[] postJsonBytes = postJson as byte[];
                if (postJsonBytes is null)
                {
                    string postJsonString = postJson as string;
                    if (postJsonString is not null)
                    {
                        postJsonBytes = Encoding.UTF8.GetBytes(postJsonString);
                    }
                    else if (postJson is not null)
                    {
                        postJsonBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(postJson));
                    }
                }
                response = await RequestMaker.MakeRequestAsync(uri, postJsonBytes, headers, method);
                if (response is null || response.Length == 0)
                {
                    return null;
                }
                var model = System.Text.Json.JsonSerializer.Deserialize<T>(response, jsonOptions);
                if (model.Error)
                {
                    throw new HttpRequestException("Error making request to " + uri.ToString() + ", error: " + model.Message);
                }
                return model;
            }
            catch (Exception ex)
            {
                if (ex is HttpRequestException rex)
                {
                    throw new HttpRequestException("Request failed: " + Encoding.UTF8.GetString(response), rex, rex.StatusCode);
                }
                throw;
            }
        }

        /// <summary>
        /// Set public and private api keys with a scope. Dispose the return value to set the API keys back to null.
        /// </summary>
        /// <param name="publicApiKey">Public api key (string or SecureString)</param>
        /// <param name="privateApiKey">Private api key (string or SecureString)</param>
        /// <returns>IDisposable, when disposed, api keys are set to null. Ignore the return value to set keys permanently.</returns>
        public IDisposable SetKeys(object publicApiKey, object privateApiKey)
        {
            if (publicApiKey is null || privateApiKey is null)
            {
                return new SetKeysScopedDisposable(this, null, null);
            }
            else if (publicApiKey is string s1)
            {
                publicApiKey = s1.ToSecureString();
            }
            if (publicApiKey is not SecureString ss1)
            {
                throw new ArgumentException("Public api key must be string or SecureString");
            }
            if (privateApiKey is string s2)
            {
                privateApiKey = s2.ToSecureString();
            }
            if (privateApiKey is not SecureString ss2)
            {
                throw new ArgumentException("Private api key must be string or SecureString");
            }
            return new SetKeysScopedDisposable(this, ss1, ss2);
        }

        private static readonly JsonSerializerOptions jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };
    }
}
