using System.IO;
using System.Text;
using System.Text.Json;
using ACG_AUDIT.ClassCollections;

namespace ACG_AUDIT.Services
{
    internal class ScreenSaverService
    {
        public static ScreenSaverSettings GetScreenSaverSettings(string filePath)
        {

            if (!File.Exists(filePath))
            {
                string[] policyDataFail = [];
                return ExtractDesiredValues(policyDataFail); ;
            }
            // Ler o arquivo Registry.pol
            string policyData = File.ReadAllText(filePath, Encoding.Unicode);

            // Remover caracteres especiais
            policyData = policyData.Replace("\u0001\0", "").Replace("\u0002\0", "").Replace("\u0004\0", "").Replace("\b\0", "");

            // Separar os registros com base no caractere ]
            string[] policyLines = policyData.Split(new[] { ']' }, StringSplitOptions.RemoveEmptyEntries);

            // Extraia os valores desejados
            return ExtractDesiredValues(policyLines);
        }

        private static ScreenSaverSettings ExtractDesiredValues(string[] policyLines)
        {
            string screenSaverIsSecure = "";
            string screenSaveActive = "";
            string screenSaveTimeOut = "";
            string screenSaverExe = "";

            foreach (string line in policyLines)
            {
                if (line.Contains("ScreenSaverIsSecure"))
                {
                    screenSaverIsSecure = ExtractValue(line);
                }
                else if (line.Contains("ScreenSaveActive"))
                {
                    screenSaveActive = ExtractValue(line);
                }
                else if (line.Contains("ScreenSaveTimeOut"))
                {
                    screenSaveTimeOut = ExtractValue(line);
                }

                if (line.Contains("**del.SCRNSAVE.EXE"))
                {
                    screenSaverExe = "Desativado";
                }
                else if (line.Contains("SCRNSAVE.EXE"))
                {
                    screenSaverExe = "Ativado";
                }
                else
                {
                    screenSaverExe = "Não configurado";
                }
            }

            return new ScreenSaverSettings(
                GetBooleanValue(screenSaveActive),
                GetBooleanValue(screenSaverIsSecure),
                GetTimeOutValue(screenSaveTimeOut),
                screenSaverExe
            );
        }

        private static string ExtractValue(string line)
        {
            // Extrai o valor após o último ;
            string[] parts = line.Split(new[] { ';' }, StringSplitOptions.None);
            return parts.Length > 1 ? parts[^1].Trim() : string.Empty;
        }

        private static bool GetBooleanValue(string value)
        {
            return int.TryParse(value, out int intValue) && intValue == 1;
        }

        private static string GetTimeOutValue(string value)
        {
            return string.IsNullOrEmpty(value) ? "Não configurado" : value + " Segundos";
        }

        public static void SaveSettingsToJson(ScreenSaverSettings settings)
        {
            string json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("screen_saver_settings.json", json);
            Console.WriteLine("As configurações de proteção de tela foram salvas em screen_saver_settings.json");
        }
    }
}