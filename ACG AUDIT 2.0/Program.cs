using System;
using System.IO;
using ACG_AUDIT_2._0.Services.RegCollector;
using ACG_AUDIT_2._0.Services.InfoCreator;
using System.Text.Json;

namespace ACG_AUDIT_2._0;

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\nInformações coletadas\n");
            GetDeviceIdInfo.GetUUID();
            Console.WriteLine();

            // Coletar informações do sistema
            SystemInfo systemInfo = SystemInformationInfo.CollectSystemInfo();

            // Exibir informações no console
            Console.WriteLine("------------------------------------------ Informações do sistema -------------------------------------------------");
            Console.WriteLine("Informações do sistema:");
            Console.WriteLine($"Nome do host: {systemInfo.HostName}");
            Console.WriteLine($"Sistema operacional: {systemInfo.OperatingSystem}");
            Console.WriteLine($"Fabricante do sistema: {systemInfo.SystemManufacturer}");
            Console.WriteLine($"Modelo do sistema: {systemInfo.SystemModel}");
            Console.WriteLine($"Tipo de sistema: {systemInfo.SystemType}");
            Console.WriteLine($"Processador(es): {systemInfo.ProcessorInfo}");
            Console.WriteLine($"Versão do BIOS: {systemInfo.BIOSVersion}");
            Console.WriteLine($"Pasta do Windows: {systemInfo.WindowsFolder}");
            Console.WriteLine($"Pasta do sistema: {systemInfo.SystemFolder}");
            Console.WriteLine($"Inicializar dispositivo: {systemInfo.BootDevice}");
            Console.WriteLine($"Localidade do sistema: {systemInfo.SystemLocale}");
            Console.WriteLine($"Localidade de entrada: {systemInfo.InputLocale}");
            Console.WriteLine($"Fuso horário: {systemInfo.TimeZone}");
            Console.WriteLine($"Memória física: {systemInfo.MemoryInfo}");
            Console.WriteLine($"Memória Virtual: {systemInfo.VirtualMemoryInfo}");
            Console.WriteLine($"Local do arquivo de paginação: {systemInfo.PageFileLocation}");
            Console.WriteLine($"Domínio: {systemInfo.DomainName}");
            Console.WriteLine($"Servidor de Logon: {systemInfo.LogonServer}");
            Console.WriteLine($"Hotfixes: {systemInfo.Hotfixes}");
            Console.WriteLine($"Rede: {systemInfo.NetworkInfo}");
            Console.WriteLine("--------------------------------------------------------------------------------------------------------------");

            // Salvar as informações em um arquivo JSON
            string json = JsonSerializer.Serialize(systemInfo, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("system_info.json", json);
            Console.WriteLine("As informações foram salvas em system_info.json");

            // Coletar e exibir informações sobre softwares instalados
            List<Software> softwares = SoftwareCollectorInfo.CollectInstalledSoftwares();
            
            Console.WriteLine();
            Console.WriteLine("--------------------------------------- Softwares Instalados ------------------------------------------------------");
            // Exibe as informações sobre os softwares instalados
            foreach (Software software in softwares)
            {
                Console.WriteLine("Nome: " + software.Nome);
                Console.WriteLine("Versão: " + software.Versao);
            }
            Console.WriteLine("--------------------------------------------------------------------------------------------------------------------");

            // Salvar os softwares instalados em um arquivo JSON
            SoftwareCollectorInfo.SaveInstalledSoftwaresToJson(softwares);

            Console.ReadLine(); // Stop console
    
             
        // Criar uma instância de PolicyExtractor e executar a extração
        // Caminho relativo do arquivo Registry.pol
        string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "GroupPolicy", "User", "Registry.pol");
        if (!File.Exists(filePath))
        {
            Console.WriteLine("O arquivo Registry.pol não foi encontrado.");
            Console.Read();
        }
        else
        {
            PolicyPasswordInfo extractor = new();
            extractor.ExtractPolicyValues(filePath);
            Console.WriteLine("Pressione qualquer tecla para finalizar o programa.");
            Console.Read();
        }

            // Verificar o status do BitLocker para todas as unidades
            Console.WriteLine();
            BitLockerInfo.CheckBitLockerStatusForAllDisks();
            Console.Read();

            AdministradorGroupInfo grupoAdministradores = new();
            grupoAdministradores.ExibirMembrosGrupos();
            Console.Read();

            UserGroupInfo grupoUsuarios = new();
            grupoUsuarios.ExibirUsuariosEGrupos();
            Console.Read();

            // Criar uma instância de AuditPolicy e exibir as políticas
            string auditFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "audit_policies.inf");
            AuditPolicyInfo auditPolicy = new(auditFilePath);
            Console.Read();
            auditPolicy.DisplayPolicy();
            Console.Read();

            // Exibir informações do firewall
            FirewallInfo.DisplayFirewallInfo();
            Console.Read();
            // Chama o método para coletar informações sobre antivírus
            string antivirusInfo = AntivirusInfo.GetAntivirusInfo();
            Console.Read();
            Console.WriteLine(antivirusInfo);

            // Chama o método para coletar informações sobre acesso remoto
            string remoteAccessInfo = RemoteAccessInfo.GetRemoteAccessInfo();
            Console.Read();
            Console.WriteLine(remoteAccessInfo);

            // Chama o método para coletar informações sobre o relógio
            string timeInfo = TimeInfo.GetTimeInfo();
            Console.Read();
            Console.WriteLine(timeInfo);

            Console.WriteLine("Digite qualquer tecla para finalizar o programa");
            Console.ReadLine(); // Stop console
        }
    }
