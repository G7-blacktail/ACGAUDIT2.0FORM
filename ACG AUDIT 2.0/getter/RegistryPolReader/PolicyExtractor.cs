using System;
using System.IO;
using System.Text;

namespace ACG_AUDIT_2._0.getter.RegistryPolReader
{
    internal class PolicyExtractor
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
        }

        private void ExtractDesiredValues(string[] policyLines)
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
}