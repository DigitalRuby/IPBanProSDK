using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalRuby.IPBanProSDK
{
    /// <summary>
    /// Handler for settings changes
    /// </summary>
    public interface ISettingsChangedListener
    {
        /// <summary>
        /// Notify that settings have changed
        /// </summary>
        void SettingsChanged(Settings newSettings);
    }
}
