using ACG_AUDIT.ClassCollections;
using ACG_AUDIT.Services;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

public class JsonCreator
{
    public static async Task CreateInventoryJson(
        object collectedData,
        string finalJsonPath,
        string systemLogPath,
        string jsonFilePath)
    {
        Dictionary<string, object> logInfo = null;
        if (File.Exists(systemLogPath))
        {
            string logContent = await File.ReadAllTextAsync(systemLogPath);
            logInfo = JsonSerializer.Deserialize<Dictionary<string, object>>(logContent);
        }

        var finalCombinedInfo = new
        {
            DeviceInfo = ((dynamic)collectedData).DeviceInfo,
            SystemInfo = ((dynamic)collectedData).SystemInfo,
            InstalledSoftware = ((dynamic)collectedData).InstalledSoftwareList,
            AdminGroupInfo = ((dynamic)collectedData).AdminGroupInfo,
            UserGroupInfo = ((dynamic)collectedData).UserGroupList,
            FirewallProfiles = ((dynamic)collectedData).FirewallProfileList,
            AntivirusProducts = ((dynamic)collectedData).AntivirusProductList,
            RemoteAccessInfo = ((dynamic)collectedData).RemoteAccessInfo,
            TimeInfo = ((dynamic)collectedData).TimeInfo,
            ScreenSaverSettings = ((dynamic)collectedData).ScreenSaverSettings,
            LogInfo = logInfo
        };

        JsonFileService.SaveToJson(finalCombinedInfo, finalJsonPath, new JsonSerializerOptions { WriteIndented = true });

        if (File.Exists(systemLogPath))
        {
            File.Delete(systemLogPath);
        }
        if (File.Exists(jsonFilePath))
        {
            File.Delete(jsonFilePath);
        }
    }
}