using System;
using System.IO;
using System.Text;
using System.Text.Json;
using ACG_AUDIT_2._0.Services.InfoCreator;

namespace ACG_AUDIT_2._0.Services.RegCollector;
    internal class PolicyPasswordInfo
    {
        public void ExtractPolicyValues(string filePath)
        {
            // Ler o arquivo Registry.pol
            string policyData = File.ReadAllText(filePath, Encoding.Unicode);

            // Remover caracteres especiais
            policyData = policyData.Replace("\u0001\0", "").Replace("\u0002\0", "").Replace("\u0004\0", "").Replace("\b\0", "");

            // Separar os registros com base no caractere ]
            string[] policyLines = policyData.Split(new[] { ']' }, StringSplitOptions.RemoveEmptyEntries);

            // Extraia os valores desejados
            ExtractDesiredValues(policyLines);

            // Extraia os valores desejados
            var settings = ExtractDesiredValues(policyLines);

            // Salvar as configurações em um arquivo JSON
            SaveSettingsToJson(settings);
        }

        private ScreenSaverSettings ExtractDesiredValues(string[] policyLines)
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

            // Imprimir os valores extraídos
            // Console.WriteLine("\nConfiguração do Usuário\n-> Modelos administrativos\n-> Painel de Controle\n-> Personalização\n");
            Console.WriteLine("------------------------------ Configurações de suspensão de tela --------------------------------------");
            Console.WriteLine($"Habilitar a proteção de tela: {GetBooleanValue(screenSaveActive)}");
            Console.WriteLine($"Proteger com senha a proteção de tela: {GetBooleanValue(screenSaverIsSecure)}");
            Console.WriteLine($"Tempo limite de Proteção de tela: {GetTimeOutValue(screenSaveTimeOut)}");
            Console.WriteLine($"Forçar proteção de tela específica: {screenSaverExe}");
            Console.WriteLine("-------------------------------------------------------------------------------------------------------");

            return new ScreenSaverSettings(GetBooleanValue(screenSaveActive), GetBooleanValue(screenSaverIsSecure), GetTimeOutValue(screenSaveTimeOut), screenSaverExe);

        }

    private void SaveSettingsToJson(ScreenSaverSettings settings)
    {
        string json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText("screen_saver_settings.json", json);
        Console.WriteLine("As configurações de proteção de tela foram salvas em screen_saver_settings.json");
    }

    private string ExtractValue(string line)
    {
        // Extrai o valor após o último ;
        string[] parts = line.Split(new[] { ';' }, StringSplitOptions.None);
        return parts.Length > 1 ? parts[^1].Trim() : string.Empty;
    }

    private string GetBooleanValue(string value)
    {
        if (int.TryParse(value, out int intValue) && intValue == 1)
        {
            return "Habilitado";
        }
        else if (string.IsNullOrEmpty(value))
        {
            return "Não configurado";
        }
        else
        {
            return "Desabilitado";
        }
    }

    private string GetTimeOutValue(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return "Não configurado";
        }
        else
        {
            return value + " Segundos";
        }
    }


    }
