using ACG_AUDIT.ClassCollections;
using System;
using System.IO;
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
                // Coletar UUID e SerialNumber
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

                // Ler o arquivo TXT para obter o hash
                string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ACG Audit", "acg audit files");
                string filePath = Path.Combine(appDataPath, "etiqueta.txt");
                if (File.Exists(filePath))
                {
                    string[] lines = File.ReadAllLines(filePath);
                    if (lines.Length >= 2)
                    {
                        // A segunda linha contém o hash
                        deviceInfo.UUIDACG = lines[1]; // Adiciona o hash ao objeto DeviceInfo
                    }
                }
                else
                {
                    Console.WriteLine("Arquivo de etiqueta não foi encontrado.");
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