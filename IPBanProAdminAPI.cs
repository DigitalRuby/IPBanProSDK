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

#region Imports

using System;
using System.Threading.Tasks;

#endregion Imports

namespace DigitalRuby.IPBanProSDK
{
    /// <summary>
    /// IPBanPro admin API wrapper
    /// </summary>
    public class IPBanProAdminAPI : IPBanProBaseAPI
    {
        /// <summary>
        /// API uri
        /// </summary>
        public override Uri BaseUri { get; set; } = new Uri("http://localhost:52664");

        /// <summary>
        /// Constructor
        /// </summary>
        public IPBanProAdminAPI()
        {
        }

        /// <summary>
        /// Get current settings
        /// </summary>
        /// <returns>Settings</returns>
        public Task<SettingsModel> GetSettings()
        {
            return MakeRequestAsync<SettingsModel>("/api/Settings");
        }

        /// <summary>
        /// Update settings
        /// </summary>
        /// <param name="settings">New settings</param>
        /// <returns>Updated settings</returns>
        public Task<SettingsModel> UpdateSettings(SettingsUpdateModel settings)
        {
            return MakeRequestAsync<SettingsModel>("/api/Settings", settings);
        }
    }
}
