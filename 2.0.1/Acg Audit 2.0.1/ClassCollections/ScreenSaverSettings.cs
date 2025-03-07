namespace ACG_AUDIT.ClassCollections
{
    internal class ScreenSaverSettings
    {
        public bool IsScreenSaveActive { get; set; }
        public bool IsScreenSaverSecure { get; set; }
        public string TimeOut { get; set; }
        public string ScreenSaverExe { get; set; }

        public ScreenSaverSettings(bool isScreenSaveActive, bool isScreenSaverSecure, string timeOut, string screenSaverExe)
        {
            IsScreenSaveActive = isScreenSaveActive;
            IsScreenSaverSecure = isScreenSaverSecure;
            TimeOut = timeOut;
            ScreenSaverExe = screenSaverExe;
        }
    }
}