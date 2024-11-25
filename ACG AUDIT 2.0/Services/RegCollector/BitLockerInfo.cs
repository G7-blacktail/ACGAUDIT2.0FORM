using System;
using System.Management;

namespace ACG_AUDIT_2._0.Services.RegCollector;
internal class BitLockerInfo
{

    public static Dictionary<string, string> CheckBitLockerStatusForAllDisks()
    {
        Dictionary<string, string> bitLockerStatuses = new Dictionary<string, string>();
        ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_LogicalDisk");
        ManagementObjectCollection disks = searcher.Get();

        foreach (var disk in disks)
        {
            string driveLetter = disk["DeviceID"].ToString()!;

            if (!string.IsNullOrEmpty(driveLetter))
            {
                string bitLockerStatus = GetBitLockerStatus(driveLetter);
                bitLockerStatuses[driveLetter] = bitLockerStatus;
            }
        }

        return bitLockerStatuses;
    }

    public static string GetBitLockerStatus(string driveLetter)
    {
        string bitLockerStatus = "Desativado";

        // Create a WMI query to get the BitLocker volume for the specified drive letter
        string wmiQuery = $"SELECT * FROM Win32_EncryptableVolume WHERE DriveLetter = '{driveLetter}'";
        ManagementObjectSearcher searcher = new ManagementObjectSearcher("\\\\.\\root\\cimv2\\Security\\MicrosoftVolumeEncryption", wmiQuery);
        try
        {
            // Execute the query and get the results
            ManagementObjectCollection results = searcher.Get();

            // Check if the BitLocker volume is found
            if (results.Count > 0)
            {
                foreach (ManagementObject bitLockerVolume in results)
                {
                    if ((uint)bitLockerVolume["ProtectionStatus"] == 1)
                    {
                        bitLockerStatus = "Ativado";
                    }
                }
            }
        }
        catch (ManagementException)
        {
            // Return a message indicating that elevated privileges are required
            return "Não foi possível coletar por necessidade de privilégios elevados - Acesso Negado Pelo Sistema";
        }

        return bitLockerStatus;
    }
}