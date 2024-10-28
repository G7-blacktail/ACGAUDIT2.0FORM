using System;
using System.Management;
using System.Globalization;
using System.DirectoryServices.ActiveDirectory;

    namespace ACG_AUDIT_2._0.RegCollector;
    internal class SystemInformationInfo
    {
    public static string GetHostName()
    {
        return Environment.MachineName;
    }

    private static string query = "SELECT * FROM Win32_OperatingSystem";
    private const string indisponivel = "Não disponível";

    public static string GetOperatingSystemInfo()
    {
        return Environment.OSVersion.VersionString;
    }

    public static string GetOperatingSystemVersion()
    {
        return Environment.OSVersion.Version.ToString();
    }

    public static string GetOperatingSystemPlatform()
    {
        return Environment.OSVersion.Platform.ToString();
    }

    public static string GetOSArchitecture()
    {
        return Environment.Is64BitOperatingSystem ? "x64-based PC" : "x86-based PC";
    }

    public static string GetSystemManufacturer()
    {
        ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
        foreach (ManagementObject obj in searcher.Get())
        {
            return obj["Manufacturer"].ToString() ?? "";
        }
        return indisponivel;
    }

    public static string GetSystemModel()
    {
        ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");
        foreach (ManagementObject obj in searcher.Get())
        {
            return obj["Model"].ToString() ?? "0";
        }
        return indisponivel;
    }

    public static string GetSystemType()
    {
        ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");
        foreach (ManagementObject obj in searcher.Get())
        {
            return obj["SystemType"].ToString() ?? "";
        }
        return indisponivel;
    }

    public static string GetProcessorInfo()
    {
        ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
        foreach (ManagementObject obj in searcher.Get())
        {
            return obj["Name"].ToString() ?? "0";
        }
        return indisponivel;
    }

    public static string GetBIOSVersion()
    {
        ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_BIOS");
        foreach (ManagementObject obj in searcher.Get())
        {
            return obj["Version"].ToString() ?? "";
        }
        return indisponivel;
    }

    public static string GetWindowsFolder()
    {
        return Environment.GetFolderPath(Environment.SpecialFolder.Windows);
    }

    public static string GetSystemFolder()
    {
        return Environment.GetFolderPath(Environment.SpecialFolder.System);
    }

    public static string GetBootDevice()
    {
        ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
        foreach (ManagementObject obj in searcher.Get())
        {
            return obj["BootDevice"].ToString() ?? "";
        }
        return indisponivel;
    }

    public static string GetSystemLocale()
    {
        return CultureInfo.InstalledUICulture.Name;
    }

    public static string GetInputLocale()
    {
        return CultureInfo.CurrentUICulture.Name;
    }

    public static string GetTimeZone()
    {
        return TimeZoneInfo.Local.DisplayName;
    }

    public static string GetMemoryInfo()
    {
        long totalMemory = GetTotalPhysicalMemory();
        long availableMemory = GetAvailablePhysicalMemory();
        return $"Total: {ConvertToGB(totalMemory)} GB, Disponível: {ConvertToGB(availableMemory)} GB";
    }

    public string GetVirtualMemoryInfo()
    {
        long totalVirtualMemory = GetVirtualMemorySize();
        long availableVirtualMemory = GetAvailableVirtualMemory();
        long usedVirtualMemory = GetUsedVirtualMemory();
        return $"Total: {ConvertToGB(totalVirtualMemory)} GB, Disponível: {ConvertToGB(availableVirtualMemory)} GB, Em Uso: {ConvertToGB(usedVirtualMemory)} GB";
    }

    public static string GetPageFileLocation()
    {
        ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PageFileUsage");
    foreach (ManagementObject obj in searcher.Get())
        {
            return obj["Name"].ToString() ?? "";
        }
        return indisponivel;
    }

    public static string GetDomainName()
    {
        return Environment.UserDomainName;
    }

    public static string GetLogonServer()
    {
        try
        {
            Domain domain = Domain.GetCurrentDomain();
            return domain.PdcRoleOwner.ToString();
        }
        catch
        {
            return indisponivel;
        }
    }

    public static string GetHotfixes()
    {
        ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_QuickFixEngineering");
        var hotfixes = searcher.Get();
        string result = $"Hotfix(es): {hotfixes.Count} instalado(s).\n";
        int index = 1;
        foreach (ManagementObject hotfix in hotfixes)
        {
            result += $"[{index++:00}]: {hotfix["HotFixID"]}\n";
        }
        return result;
    }

    public static string GetNetworkInfo()
    {
        string network = "";
        ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapter WHERE NetEnabled = true");
        var adapters = searcher.Get();
        int index = 1;
        foreach ( ManagementObject adapter in adapters)
        {
            network += $"[{index++:00}]: {adapter["Name"]}\n";
            GetNetworkAdapterDetails(adapter["DeviceID"].ToString() ?? "0", ref network);
        }
        return network;
    }

    private static void GetNetworkAdapterDetails(string deviceId, ref string network)
    {
        ManagementObjectSearcher searcher = new ManagementObjectSearcher($"SELECT * FROM Win32_NetworkAdapterConfiguration WHERE Index = {deviceId}");
        foreach (ManagementObject config in searcher.Get())
        {
            network += $"  Nome da conexão: {config["Description"]}\n";
            network += $"  DHCP ativado: {(config["DHCPEnabled"] != null && (bool)config["DHCPEnabled"] ? "Sim" : "Não")}\n";
            network += $"  Endereço(es) IP: \n";
            if (config["IPAddress"] is string[] ipAddresses)
            {
                for (int i = 0; i < ipAddresses.Length; i++)
                {
                    network += $"  [{i + 1:00}]: {ipAddresses[i]}\n";
                }
            }
        }
    }

    private static long GetTotalPhysicalMemory()
    {
        ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
        foreach (ManagementObject obj in searcher.Get())
        {
            return long.Parse(obj["TotalVisibleMemorySize"].ToString() ?? "0");
        }
        return 0;
    }

    private static long GetAvailablePhysicalMemory()
    {
        ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
        foreach (ManagementObject obj in searcher.Get())
        {
            return long.Parse(obj["FreePhysicalMemory"].ToString() ?? "0");
        }
        return 0;
    }

    private static long GetVirtualMemorySize()
    {
        ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
        foreach (ManagementObject obj in searcher.Get())
        {
            return long.Parse(obj["TotalVirtualMemorySize"].ToString() ?? "0");
        }
        return 0;
    }

    private static long GetAvailableVirtualMemory()
    {
        ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
        foreach (ManagementObject obj in searcher.Get())
        {
            return long.Parse(obj["FreeVirtualMemory"].ToString() ?? "0");
        }
        return 0;
    }

    private static long GetUsedVirtualMemory()
    {
        long totalMemory = GetVirtualMemorySize();
        long freeMemory = GetAvailableVirtualMemory();
        return totalMemory - freeMemory; // Calcula a memória usada
    }

    private static long ConvertToGB(long megabytes)
    {
        return megabytes / 1024; // Conversão para GB
    }
}