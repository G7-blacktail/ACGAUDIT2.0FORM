using System;
using Microsoft.Win32;

namespace ACG_AUDIT_2._0.getter
{
    public class RemoteAccessInfo
    {
        public static string GetRemoteAccessInfo()
        {
            string result = "Acesso remoto:\n----------------------------\n";

            // Verifica a Assistência Remota
            string allowRemoteAssistance = GetRegistryValue(@"SYSTEM\CurrentControlSet\Control\Remote Assistance", "fAllowToGetHelp");
            allowRemoteAssistance = allowRemoteAssistance == "1" ? "Ativo" : "Inativo";

            // Verifica a Área de Trabalho Remota
            string allowRemoteDesktop = GetRegistryValue(@"SYSTEM\CurrentControlSet\Control\Terminal Server", "fDenyTSConnections");
            string requireNetworkLevelAuthentication = GetRegistryValue(@"SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp", "SecurityLayer");

            if (allowRemoteDesktop == "0")
            {
                allowRemoteDesktop = "Permitido";
                requireNetworkLevelAuthentication = requireNetworkLevelAuthentication == "1" ? "Sim" : "Não";
            }
            else
            {
                allowRemoteDesktop = "Não permitido";
                requireNetworkLevelAuthentication = "Desabilitado";
            }

            // Monta o resultado
            result += $"Assistência Remota: {allowRemoteAssistance}\n";
            result += $"Área de Trabalho Remota: {allowRemoteDesktop}\n";
            result += $"Requer autenticação no nível da rede: {requireNetworkLevelAuthentication}\n";

            return result;
        }

        private static string GetRegistryValue(string path, string valueName)
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(path)!)
                {
                    if (key != null)
                    {
                        object value = key.GetValue(valueName)!;
                        return value?.ToString() ?? "Desconhecido";
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Erro ao acessar o registro: {ex.Message}";
            }
            return "Desconhecido";
        }
    }
}