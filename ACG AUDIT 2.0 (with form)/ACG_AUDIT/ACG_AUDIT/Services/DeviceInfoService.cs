using ACG_AUDIT.ClassCollections;
using System;
using System.Management;

namespace ACG_AUDIT.Services
{
    public static class DeviceInfoCollector
    {
        public static DeviceInfo CollectDeviceInfo()
        {
            DeviceInfo deviceInfo = new DeviceInfo();

            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystemProduct");
                foreach (ManagementObject share in searcher.Get())
                {
                    deviceInfo.UUID = share["UUID"]?.ToString() ?? string.Empty;
                }

                searcher = new ManagementObjectSearcher("SELECT * FROM Win32_BIOS");
                foreach (ManagementObject share in searcher.Get())
                {
                    deviceInfo.SerialNumber = share["SerialNumber"]?.ToString() ?? string.Empty;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao coletar informações do dispositivo: " + ex.Message);
            }

            return deviceInfo;
        }
    }
}