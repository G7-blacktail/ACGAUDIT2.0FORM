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


// using System;

// class Program
// {
//     static void Main()
//     {
//         // Informações do sistema operacional
//         Console.WriteLine("Informações do sistema operacional Windows:");
//         Console.WriteLine("----------------------------");

//         // Obtendo informações do sistema operacional
//         string osVersion = Environment.OSVersion.ToString();
//         string machineName = Environment.MachineName;
//         string userName = Environment.UserName;
//         string CurrentDirectory = Environment.CurrentDirectory;
//         bool Is64BitOperatingSystem = Environment.Is64BitOperatingSystem;
//         bool Is64BitProcess = Environment.Is64BitProcess;
//         bool IsPrivilegedProcess = Environment.IsPrivilegedProcess;
        
//         int processadorCount = Environment.ProcessorCount;
//         string processPath = Environment.ProcessPath!;
//         string SystemDirectory = Environment.SystemDirectory;
//         string UserDomainName = Environment.UserDomainName.ToString();

//         string version = Environment.Version.ToString();
//         long workingSet = Environment.WorkingSet;

//         // Exibindo as informações
//         Console.WriteLine($"Versão do SO: {osVersion}");
//         Console.WriteLine($"Nome da máquina: {machineName}");
//         Console.WriteLine($"Nome do usuário: {userName}");
//         Console.WriteLine(CurrentDirectory);
//         Console.WriteLine(Is64BitOperatingSystem);
//         Console.WriteLine(Is64BitProcess);
//         Console.WriteLine(IsPrivilegedProcess);
//         Console.WriteLine(processadorCount);
//         Console.WriteLine(processPath);
//         Console.WriteLine(SystemDirectory);
//         Console.WriteLine(UserDomainName);
//         Console.WriteLine(version);
//         Console.WriteLine(workingSet);
//     }
// }

using System;
using System.Collections;

class Sample
{
    public static void Main()
    {
        string str;
        string nl = Environment.NewLine;
        //
        Console.WriteLine();
        Console.WriteLine("-- Environment members --");

        //  Invoke this sample with an arbitrary set of command line arguments.
        Console.WriteLine("CommandLine: {0}", Environment.CommandLine);

        string[] arguments = Environment.GetCommandLineArgs();
        Console.WriteLine("GetCommandLineArgs: {0}", String.Join(", ", arguments));

        //  <-- Keep this information secure! -->
        Console.WriteLine("CurrentDirectory: {0}", Environment.CurrentDirectory);

        Console.WriteLine("ExitCode: {0}", Environment.ExitCode);

        Console.WriteLine("HasShutdownStarted: {0}", Environment.HasShutdownStarted);

        //  <-- Keep this information secure! -->
        Console.WriteLine("MachineName: {0}", Environment.MachineName);

        Console.WriteLine("NewLine: {0}  first line{0}  second line{0}  third line",
                              Environment.NewLine);

        Console.WriteLine("OSVersion: {0}", Environment.OSVersion.ToString());

        Console.WriteLine("StackTrace: '{0}'", Environment.StackTrace);

        //  <-- Keep this information secure! -->
        Console.WriteLine("SystemDirectory: {0}", Environment.SystemDirectory);

        Console.WriteLine("TickCount: {0}", Environment.TickCount);

        //  <-- Keep this information secure! -->
        Console.WriteLine("UserDomainName: {0}", Environment.UserDomainName);

        Console.WriteLine("UserInteractive: {0}", Environment.UserInteractive);

        //  <-- Keep this information secure! -->
        Console.WriteLine("UserName: {0}", Environment.UserName);

        Console.WriteLine("Version: {0}", Environment.Version.ToString());

        Console.WriteLine("WorkingSet: {0}", Environment.WorkingSet);

        //  No example for Exit(exitCode) because doing so would terminate this example.

        //  <-- Keep this information secure! -->
        string query = "My system drive is %SystemDrive% and my system root is %SystemRoot%";
        str = Environment.ExpandEnvironmentVariables(query);
        Console.WriteLine("ExpandEnvironmentVariables: {0}  {1}", nl, str);

        Console.WriteLine("GetEnvironmentVariable: {0}  My temporary directory is {1}.", nl,
                               Environment.GetEnvironmentVariable("TEMP"));

        Console.WriteLine("GetEnvironmentVariables: ");
        IDictionary environmentVariables = Environment.GetEnvironmentVariables();
        foreach (DictionaryEntry de in environmentVariables)
        {
            Console.WriteLine("  {0} = {1}", de.Key, de.Value);
        }

        Console.WriteLine("GetFolderPath: {0}",
                     Environment.GetFolderPath(Environment.SpecialFolder.System));

        string[] drives = Environment.GetLogicalDrives();
        Console.WriteLine("GetLogicalDrives: {0}", String.Join(", ", drives));
    }
}