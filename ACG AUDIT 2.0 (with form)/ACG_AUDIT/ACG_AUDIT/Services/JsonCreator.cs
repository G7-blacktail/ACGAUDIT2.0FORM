using ACG_AUDIT.ClassCollections;
using ACG_AUDIT.Services;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

public class JsonCreator
{
    /// <summary>
    /// Cria o JSON inicial sem LogInfo (Primeira etapa)
    /// </summary>
    public static async Task CreateInitialInventoryJson(
        object collectedData,
        string finalJsonPath,
        string jsonFilePath)
    {
        var initialInfo = new
        {
            DeviceInfo = ((dynamic)collectedData).DeviceInfo,
            SystemInfo = ((dynamic)collectedData).SystemInfo,
            InstalledSoftware = ((dynamic)collectedData).InstalledSoftware,
            AdminGroupInfo = ((dynamic)collectedData).AdminGroupInfo,
            UserGroupInfo = ((dynamic)collectedData).UserGroupInfo,
            FirewallProfiles = ((dynamic)collectedData).FirewallProfilesList,
            AntivirusProducts = ((dynamic)collectedData).AntivirusProductsList,
            RemoteAccessInfo = ((dynamic)collectedData).RemoteAccessInfo,
            TimeInfo = ((dynamic)collectedData).TimeInfo,
            ScreenSaverSettings = ((dynamic)collectedData).ScreenSaverSettings
        };

        JsonFileService.SaveToJson(initialInfo, finalJsonPath, new JsonSerializerOptions { WriteIndented = true });

        // Exclui JSON temporário se existir
        if (File.Exists(jsonFilePath))
        {
            File.Delete(jsonFilePath);
        }
    }

    /// <summary>
    /// Atualiza o JSON existente incluindo LogInfo (Segunda etapa)
    /// </summary>
    public static async Task UpdateInventoryWithLogInfo(
        object collectedData,
        string finalJsonPath,
        string systemLogPath)
    {
        if (!File.Exists(finalJsonPath))
        {
            throw new FileNotFoundException("Arquivo JSON inicial não encontrado.");
        }

        Dictionary<string, object> logInfo = null;
        if (File.Exists(systemLogPath))
        {
            string logContent = await File.ReadAllTextAsync(systemLogPath);
            logInfo = JsonSerializer.Deserialize<Dictionary<string, object>>(logContent);
        }

        var updatedInfo = new
        {
            DeviceInfo = ((dynamic)collectedData).DeviceInfo,
            SystemInfo = ((dynamic)collectedData).SystemInfo,
            InstalledSoftware = ((dynamic)collectedData).InstalledSoftware,
            AdminGroupInfo = ((dynamic)collectedData).AdminGroupInfo,
            UserGroupInfo = ((dynamic)collectedData).UserGroupInfo,
            FirewallProfiles = ((dynamic)collectedData).FirewallProfilesList,
            AntivirusProducts = ((dynamic)collectedData).AntivirusProductsList,
            RemoteAccessInfo = ((dynamic)collectedData).RemoteAccessInfo,
            TimeInfo = ((dynamic)collectedData).TimeInfo,
            ScreenSaverSettings = ((dynamic)collectedData).ScreenSaverSettings,
            LogInfo = logInfo // Agora só adicionamos LogInfo na segunda etapa
        };

        JsonFileService.SaveToJson(updatedInfo, finalJsonPath, new JsonSerializerOptions { WriteIndented = true });

        // Exclui o arquivo systemLogPath após uso
        if (File.Exists(systemLogPath))
        {
            File.Delete(systemLogPath);
        }
    }
}
