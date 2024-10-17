// using System;
// using System.IO;
// using getter;

// namespace RegistryPolReader
// {
//     class Program
//     {
//         static void Main(string[] args)
//         {

//             GetDeviceId.GetUUID();


//             SystemInformation systemInfo = new SystemInformation();
//             Console.WriteLine("\nInformações do sistema:");
//             Console.WriteLine($"Sistema operacional: {systemInfo.GetOperatingSystemInfo()}");
//             Console.WriteLine($"Processador: {systemInfo.GetProcessorInfo()}");
//             Console.WriteLine($"Memória: {systemInfo.GetMemoryInfo()}");
//             Console.WriteLine($"Armazenamento: {systemInfo.GetStorageInfo()}");
//             Console.WriteLine($"Rede: {systemInfo.GetNetworkInfo()}");
//             Console.WriteLine();

//              // SoftwareCollector.CollectAndDisplayInstalledSoftwares();
//             // Criar uma instância de PolicyExtractor e executar a extração

//              // Caminho relativo do arquivo Registry.pol
//             string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "GroupPolicy", "User", "Registry.pol");
//             Console.WriteLine($"\nArquivo Pesquisado\nFile path: {filePath}");
//             if (!File.Exists(filePath))
//             {
//                 Console.WriteLine("O arquivo Registry.pol não foi encontrado.");
//                 return;
//             }

//             PolicyExtractor extractor = new PolicyExtractor();
//             extractor.ExtractPolicyValues(filePath);

//             // Verificar o status do BitLocker para todas as unidades
//             BitLockerInfo.CheckBitLockerStatusForAllDisks();

//             // Coletar e exibir informações sobre softwares instalados

//             Console.ReadLine(); // Stop console
//         }
//     }
// }


using System;

class Program
{
    static void Main()
    {
        // Informações do sistema operacional
        Console.WriteLine("Informações do sistema operacional Windows:");
        Console.WriteLine("----------------------------");

        // Obtendo informações do sistema operacional
        string osVersion = Environment.OSVersion.ToString();
        string machineName = Environment.MachineName;
        string userName = Environment.UserName;
        string CurrentDirectory = Environment.CurrentDirectory;
        bool Is64BitOperatingSystem = Environment.Is64BitOperatingSystem;
        bool Is64BitProcess = Environment.Is64BitProcess;
        bool IsPrivilegedProcess = Environment.IsPrivilegedProcess;
        
        int processadorCount = Environment.ProcessorCount;
        string processPath = Environment.ProcessPath!;
        string SystemDirectory = Environment.SystemDirectory;
        string UserDomainName = Environment.UserDomainName.ToString();

        string version = Environment.Version.ToString();
        long workingSet = Environment.WorkingSet;

        // Exibindo as informações
        Console.WriteLine($"Versão do SO: {osVersion}");
        Console.WriteLine($"Nome da máquina: {machineName}");
        Console.WriteLine($"Nome do usuário: {userName}");
        Console.WriteLine(CurrentDirectory);
        Console.WriteLine(Is64BitOperatingSystem);
        Console.WriteLine(Is64BitProcess);
        Console.WriteLine(IsPrivilegedProcess);
        Console.WriteLine(processadorCount);
        Console.WriteLine(processPath);
        Console.WriteLine(SystemDirectory);
        Console.WriteLine(UserDomainName);
        Console.WriteLine(version);
        Console.WriteLine(workingSet);
    }
}