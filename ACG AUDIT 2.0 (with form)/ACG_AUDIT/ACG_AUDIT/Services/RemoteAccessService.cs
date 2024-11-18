using ACG_AUDIT.ClassCollections;
using Microsoft.Win32;

namespace ACG_AUDIT.Services
{
    internal class RemoteAccessService
    {
        public static RemoteAccess GetRemoteAccessInfo()
        {
            RemoteAccess remoteAccess = new RemoteAccess();

            // Verifica a Assistência Remota
            string allowRemoteAssistance = GetRegistryValue(@"SYSTEM\CurrentControlSet\Control\Remote Assistance", "fAllowToGetHelp");
            remoteAccess.RemoteAssistance = allowRemoteAssistance == "1" ? "Ativo" : "Inativo";

            // Verifica a Área de Trabalho Remota
            string allowRemoteDesktop = GetRegistryValue(@"SYSTEM\CurrentControlSet\Control\Terminal Server", "fDenyTSConnections");
            string requireNetworkLevelAuthentication = GetRegistryValue(@"SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp", "SecurityLayer");

            if (allowRemoteDesktop == "0")
            {
                remoteAccess.RemoteDesktop = "Permitido";
                remoteAccess.RequireNetworkLevelAuthentication = requireNetworkLevelAuthentication == "1" ? "Sim" : "Não";
            }
            else
            {
                remoteAccess.RemoteDesktop = "Não permitido";
                remoteAccess.RequireNetworkLevelAuthentication = "Desabilitado";
            }

            return remoteAccess;
        }

        private static string GetRegistryValue(string path, string valueName)
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(path))
                {
                    if (key != null)
                    {
                        object value = key.GetValue(valueName);
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