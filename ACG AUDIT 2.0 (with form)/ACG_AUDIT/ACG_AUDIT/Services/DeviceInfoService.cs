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

                string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ACG Audit", "acg audit files");
                string filePath = Path.Combine(appDataPath, "Etiqueta.txt");

                try
                {
                    if (File.Exists(filePath))
                    {
                        string[] lines = File.ReadAllLines(filePath);
                        if (lines.Length > 0) // Considera a primeira linha como o UUIDACG
                        {
                            deviceInfo.UUIDACG = lines[0].Trim(); // Garante que não haja espaços extras
                        }
                        else
                        {
                            Console.WriteLine("Arquivo de etiqueta está vazio.");
                            deviceInfo.UUIDACG = string.Empty; // Define um valor padrão
                        }
                    }
                    else
                    {
                        Console.WriteLine("Arquivo de etiqueta não foi encontrado.");
                        deviceInfo.UUIDACG = string.Empty; // Define um valor padrão
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao ler o arquivo de etiqueta: {ex.Message}");
                    deviceInfo.UUIDACG = string.Empty; // Garante que o campo esteja presente no JSON
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