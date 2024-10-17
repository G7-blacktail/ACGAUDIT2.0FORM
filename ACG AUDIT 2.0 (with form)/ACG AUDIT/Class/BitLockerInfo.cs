using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace ACG_AUDIT.Class
{
    internal class BitLockerInfo
    {
        public static void CheckBitLockerStatusForAllDisks()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_LogicalDisk");
            ManagementObjectCollection disks = searcher.Get();

            foreach (var disk in disks)
            {
                string driveLetter = disk["DeviceID"].ToString()!;

                if (!string.IsNullOrEmpty(driveLetter))
                {
                    string bitLockerStatus = GetBitLockerStatus(driveLetter);
                    Console.WriteLine($"BitLocker status for {driveLetter}: {bitLockerStatus}");
                }
            }
        }

        public static string GetBitLockerStatus(string driveLetter)
        {
            string bitLockerStatus = "Desativado";

            // Create a WMI query to get the BitLocker volume for the specified drive letter
            string wmiQuery = $"SELECT * FROM Win32_EncryptableVolume WHERE DriveLetter = '{driveLetter}'";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("\\\\.\\root\\cimv2\\Security\\MicrosoftVolumeEncryption", wmiQuery);

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

            return bitLockerStatus;
        }
    }
}
