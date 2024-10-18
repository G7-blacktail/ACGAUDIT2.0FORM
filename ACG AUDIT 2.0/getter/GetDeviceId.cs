using System;
using System.Management;

namespace ACG_AUDIT_2._0.getter;

internal class GetDeviceId
{
    public static void GetUUID()
    {
        ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystemProduct");
        Console.WriteLine("---------------------------------------------- Informações da placamãe -------------------------------------------");
        foreach (ManagementObject share in searcher.Get())
        {
            Console.WriteLine("UUID do dispositivo: " + share["UUID"]);
        }
        searcher = new ManagementObjectSearcher("SELECT * FROM Win32_BIOS");
        
        foreach (ManagementObject share in searcher.Get())
        {
            Console.WriteLine("Número de série do hardware: " + share["SerialNumber"]);
        }
        Console.WriteLine("-------------------------------------------------------------------------------------------------------------------");
    }
}