using System;
using System.Runtime.InteropServices;

public class SystemInformation
{
    public string GetOperatingSystemInfo()
    {
        string operatingSystem = Environment.OSVersion.ToString();
        return operatingSystem;
    }

    public string GetProcessorInfo()
    {
        string processor = RuntimeInformation.ProcessArchitecture.ToString();
        return processor;
    }

    public string GetMemoryInfo()
    {
        string memory = $"{GC.GetTotalMemory(false) / 1024} KB";
        return memory;
    }

    public string GetStorageInfo()
    {
        string storage = "";
        foreach (DriveInfo drive in DriveInfo.GetDrives())
        {
            storage += $"Drive {drive.Name} - {drive.TotalSize} bytes\n";
        }
        return storage;
    }

    public string GetNetworkInfo()
    {
        string network = "";
        foreach (var nic in System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces())
        {
            network += $"Interface {nic.Name} - {nic.GetIPProperties().UnicastAddresses[0].Address}\n";
        }
        return network;
    }
}