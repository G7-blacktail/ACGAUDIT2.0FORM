namespace ACG_AUDIT_2._0.Services.InfoCreator;
public class ScreenSaverSettings
{
    public string ScreenSaverActive { get; set; }
    public string ScreenSaverIsSecure { get; set; }
    public string ScreenSaverTimeOut { get; set; }
    public string ScreenSaverExe { get; set; }

    public ScreenSaverSettings(string active, string isSecure, string timeOut, string exe)
    {
        ScreenSaverActive = active;
        ScreenSaverIsSecure = isSecure;
        ScreenSaverTimeOut = timeOut;
        ScreenSaverExe = exe;
    }
}