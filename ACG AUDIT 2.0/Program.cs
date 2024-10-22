using System;
using System.IO;
using ACG_AUDIT_2._0.getter;
using ACG_AUDIT_2._0.getter.RegistryPolReader;

namespace RegistryPolReader
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("\nInformações coletadas\n");
                GetDeviceId.GetUUID();
                Console.WriteLine();

                Console.WriteLine("------------------------------------------ Informações do sistema -------------------------------------------------");
                SystemInformation systemInfo = new SystemInformation();
                Console.WriteLine("Informações do sistema:");
                Console.WriteLine($"Nome do host: {SystemInformation.GetHostName()}");
                Console.WriteLine($"Sistema operacional: {SystemInformation.GetOperatingSystemInfo()}");
                Console.WriteLine($"Fabricante do sistema: {SystemInformation.GetSystemManufacturer()}");
                Console.WriteLine($"Modelo do sistema: {SystemInformation.GetSystemModel()}");
                Console.WriteLine($"Tipo de sistema: {SystemInformation.GetSystemType()}");
                Console.WriteLine($"Processador(es): {SystemInformation.GetProcessorInfo()}");
                Console.WriteLine($"Versão do BIOS: {SystemInformation.GetBIOSVersion()}");
                Console.WriteLine($"Pasta do Windows: {SystemInformation.GetWindowsFolder()}");
                Console.WriteLine($"Pasta do sistema: {SystemInformation.GetSystemFolder()}");
                Console.WriteLine($"Inicializar dispositivo: {SystemInformation.GetBootDevice()}");
                Console.WriteLine($"Localidade do sistema: {SystemInformation.GetSystemLocale()}");
                Console.WriteLine($"Localidade de entrada: {SystemInformation.GetInputLocale()}");
                Console.WriteLine($"Fuso horário: {SystemInformation.GetTimeZone()}");
                Console.WriteLine($"Memória física: {SystemInformation.GetMemoryInfo()}");
                Console.WriteLine($"Memória Virtual: {systemInfo.GetVirtualMemoryInfo()}");
                Console.WriteLine($"Local do arquivo de paginação: {SystemInformation.GetPageFileLocation()}");
                Console.WriteLine($"Domínio: {SystemInformation.GetDomainName()}");
                Console.WriteLine($"Servidor de Logon: {SystemInformation.GetLogonServer()}");
                Console.WriteLine($"Hotfixes: {SystemInformation.GetHotfixes()}");
                Console.WriteLine($"Rede: {SystemInformation.GetNetworkInfo()}");
                Console.WriteLine("--------------------------------------------------------------------------------------------------------------");

                SoftwareCollector.CollectAndDisplayInstalledSoftwares();

                // Caminho relativo do arquivo Registry.pol
                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "GroupPolicy", "User ", "Registry.pol");
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("O arquivo Registry.pol não foi encontrado.");
                    return;
                }

                PolicyExtractor extractor = new PolicyExtractor();
                extractor.ExtractPolicyValues(filePath);

                // Verificar o status do BitLocker para todas as unidades
                Console.WriteLine();
                BitLockerInfo.CheckBitLockerStatusForAllDisks();

                GrupoAdministradores grupoAdministradores = new GrupoAdministradores();
                grupoAdministradores.ExibirMembrosGrupos();

                // Criar uma instância de AuditPolicy e exibir as políticas
                string auditFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "audit_policies.inf");
                AuditPolicy auditPolicy = new AuditPolicy(auditFilePath);
                auditPolicy.DisplayPolicy();

                // Exibir informações do firewall
                FirewallInfo.DisplayFirewallInfo();

                // Chama o método para coletar informações sobre antivírus
                string antivirusInfo = AntivirusInfo.GetAntivirusInfo();
                Console.WriteLine(antivirusInfo);

                // Chama o método para coletar informações sobre acesso remoto
                string remoteAccessInfo = RemoteAccessInfo.GetRemoteAccessInfo();
                Console.WriteLine(remoteAccessInfo);

                // Chama o método para coletar informações sobre o relógio
                string timeInfo = TimeInfo.GetTimeInfo();
                Console.WriteLine(timeInfo);
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                Console.WriteLine("Erro: A operação requer elevação. Execute o programa como administrador.");
                Console.WriteLine($"Detalhes: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro inesperado: {ex.Message}");
            }
            finally
            {
                Console.ReadLine(); // Stop console
            }
        }
    }
}