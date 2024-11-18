using ACG_AUDIT.ClassCollections;
using System;
using System.Management;

namespace ACG_AUDIT.Services
{
    internal class InstalledSoftwareService
    {
        public static InstalledSoftwareList CollectInstalledSoftware()
        {
            InstalledSoftwareList installedSoftwareList = new InstalledSoftwareList();

            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Product");
                foreach (ManagementObject obj in searcher.Get())
                {
                    InstalledSoftware software = new InstalledSoftware
                    {
                        Name = obj["Name"]?.ToString() ?? "Desconhecido",
                        Version = obj["Version"]?.ToString() ?? "Desconhecido",
                        Vendor = obj["Vendor"]?.ToString() ?? "Desconhecido"
                    };
                    installedSoftwareList.SoftwareList.Add(software);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao coletar informações dos softwares instalados: " + ex.Message);
            }

            return installedSoftwareList;
        }
    }
}