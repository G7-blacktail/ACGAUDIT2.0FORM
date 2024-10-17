using System;
using System.IO;
using getter;

namespace RegistryPolReader
{
    class Program
    {
        static void Main(string[] args)
        {

            GetDeviceId.GetUUID();


            SystemInformation systemInfo = new SystemInformation();
            Console.WriteLine("\nInformações do sistema:");
            Console.WriteLine($"Sistema operacional: {systemInfo.GetOperatingSystemInfo()}");
            Console.WriteLine($"Processador: {systemInfo.GetProcessorInfo()}");
            Console.WriteLine($"Memória: {systemInfo.GetMemoryInfo()}");
            Console.WriteLine($"Armazenamento: {systemInfo.GetStorageInfo()}");
            Console.WriteLine($"Rede: {systemInfo.GetNetworkInfo()}");
            Console.WriteLine();

             // SoftwareCollector.CollectAndDisplayInstalledSoftwares();
            // Criar uma instância de PolicyExtractor e executar a extração

             // Caminho relativo do arquivo Registry.pol
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "GroupPolicy", "User", "Registry.pol");
            Console.WriteLine($"\nArquivo Pesquisado\nFile path: {filePath}");
            if (!File.Exists(filePath))
            {
                Console.WriteLine("O arquivo Registry.pol não foi encontrado.");
                return;
            }

            PolicyExtractor extractor = new PolicyExtractor();
            extractor.ExtractPolicyValues(filePath);

            // Verificar o status do BitLocker para todas as unidades
            BitLockerInfo.CheckBitLockerStatusForAllDisks();

            // Coletar e exibir informações sobre softwares instalados

            Console.ReadLine(); // Stop console
        }
    }
}