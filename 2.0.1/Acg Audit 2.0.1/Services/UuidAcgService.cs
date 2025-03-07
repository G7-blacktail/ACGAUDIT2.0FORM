using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using ACG_AUDIT.ClassCollections;
using System.IO;

namespace ACG_AUDIT.Services
{
    internal class UuidAcgService
    {
        public UuidAcgService() { }

        public void GetPropUuidAcg()
        {
            string serial = GetDiskSerialNumber();
            serial = serial.TrimEnd('.');

            string uuid = GetComputerUUID();

            string hostname = Environment.MachineName;

            string input = $"{serial}{hostname}{uuid}";

            string hash = ComputeSha256Hash(input);

            SaveHashToFile(hash);
        }


        private static string GetDiskSerialNumber()
        {
            string serialNumber = string.Empty;

            try
            {
                // Executar o comando PowerShell para obter o número de série do disco
                var processInfo = new ProcessStartInfo("powershell", "Get-WmiObject Win32_DiskDrive | Select-Object -ExpandProperty SerialNumber")
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(processInfo))
                {
                    using (var reader = process.StandardOutput)
                    {
                        serialNumber = reader.ReadToEnd().Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter o número de série do disco: {ex.Message}");
            }

            return serialNumber;
        }

        private static string GetComputerUUID()
        {
            string uuid = string.Empty;

            try
            {
                // Executar o comando PowerShell para obter o UUID do computador
                var processInfo = new ProcessStartInfo("powershell", "Get-WmiObject Win32_ComputerSystemProduct | Select-Object -ExpandProperty UUID")
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(processInfo))
                {
                    using (var reader = process.StandardOutput)
                    {
                        uuid = reader.ReadToEnd().Trim(); // Remover espaços em branco
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter o UUID do computador: {ex.Message}");
            }

            return uuid;
        }

        private static string ComputeSha256Hash(string rawData)
        {
            // Criar um SHA256
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Computar o hash
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Converter o byte array em uma string hexadecimal
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("X2")); // Usar "X2" para letras maiúsculas
                }
                return builder.ToString();
            }
        }

        private void SaveHashToFile(string hash)
        {
            string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "ACG Audit", "acg audit files");
            string filePath = Path.Combine(appDataPath, "Etiqueta.txt");

            if (!Directory.Exists(appDataPath))
            {
                Directory.CreateDirectory(appDataPath);
            }

            string content = hash;

            File.WriteAllText(filePath, content);

        }

    }
}
