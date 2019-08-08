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
