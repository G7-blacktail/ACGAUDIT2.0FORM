using System;
using System.IO;
using ACG_AUDIT_2._0.Services.RegCollector;
using ACG_AUDIT_2._0.Database;
using ACG_AUDIT_2._0.Models.Entity;

namespace ACG_AUDIT_2._0;

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\nInformações coletadas\n");
            GetDeviceIdInfo.GetUUID();
            Console.WriteLine();

            SystemInformationEntity systemInfo = SystemInformationInfo.CollectSystemInformation();

            DatabaseManager databaseManager = new DatabaseManager();
            databaseManager.SaveSystemInformation(systemInfo);

            Console.WriteLine("------------------------------------------ Informações do sistema -------------------------------------------------");
            Console.WriteLine("Informações do sistema:");
            Console.WriteLine($"Nome do host: {SystemInformationInfo.GetHostName()}");
            Console.WriteLine($"Sistema operacional: {SystemInformationInfo.GetOperatingSystemInfo()}");
            Console.WriteLine($"Fabricante do sistema: {SystemInformationInfo.GetSystemManufacturer()}");
            Console.WriteLine($"Modelo do sistema: {SystemInformationInfo.GetSystemModel()}");
            Console.WriteLine($"Tipo de sistema: {SystemInformationInfo.GetSystemType()}");
            Console.WriteLine($"Processador(es): {SystemInformationInfo.GetProcessorInfo()}");
            Console.WriteLine($"Versão do BIOS: {SystemInformationInfo.GetBIOSVersion()}");
            Console.WriteLine($"Pasta do Windows: {SystemInformationInfo.GetWindowsFolder()}");
            Console.WriteLine($"Pasta do sistema: {SystemInformationInfo.GetSystemFolder()}");
            Console.WriteLine($"Inicializar dispositivo: {SystemInformationInfo.GetBootDevice()}");
            Console.WriteLine($"Localidade do sistema: {SystemInformationInfo.GetSystemLocale()}");
            Console.WriteLine($"Localidade de entrada: {SystemInformationInfo.GetInputLocale()}");
            Console.WriteLine($"Fuso horário: {SystemInformationInfo.GetTimeZone()}");
            Console.WriteLine($"Memória física: {SystemInformationInfo.GetMemoryInfo()}");
            Console.WriteLine($"Memória Virtual: {SystemInformationInfo.GetVirtualMemoryInfo()}");
            Console.WriteLine($"Local do arquivo de paginação: {SystemInformationInfo.GetPageFileLocation()}");
            Console.WriteLine($"Domínio: {SystemInformationInfo.GetDomainName()}");
            Console.WriteLine($"Servidor de Logon: {SystemInformationInfo.GetLogonServer()}");
            Console.WriteLine($"Hotfixes: {SystemInformationInfo.GetHotfixes()}");
            Console.WriteLine($"Rede: {SystemInformationInfo.GetNetworkInfo()}");
            Console.WriteLine();
            Console.WriteLine("--------------------------------------------------------------------------------------------------------------");

             SoftwareCollectorInfo.CollectAndDisplayInstalledSoftwares();
             
            // Criar uma instância de PolicyExtractor e executar a extração

             // Caminho relativo do arquivo Registry.pol
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "GroupPolicy", "User", "Registry.pol");
            // Console.WriteLine($"\nArquivo Pesquisado\nFile path: {filePath}");
            if (!File.Exists(filePath))
            {
                Console.WriteLine("O arquivo Registry.pol não foi encontrado.");
                return;
            }

            PolicyPasswordInfo extractor = new();
            extractor.ExtractPolicyValues(filePath);

            // Verificar o status do BitLocker para todas as unidades
            Console.WriteLine();
            BitLockerInfo.CheckBitLockerStatusForAllDisks();

            AdministradorGroupInfo grupoAdministradores = new();
            grupoAdministradores.ExibirMembrosGrupos();

            UserGroupInfo grupoUsuarios = new();
            grupoUsuarios.ExibirUsuariosEGrupos();

            // Criar uma instância de AuditPolicy e exibir as políticas
            string auditFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "audit_policies.inf");
            AuditPolicyInfo auditPolicy = new(auditFilePath);
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

            Console.WriteLine("Digite qualquer tecla para finalizar o programa");
            Console.ReadLine(); // Stop console
        }
    }
