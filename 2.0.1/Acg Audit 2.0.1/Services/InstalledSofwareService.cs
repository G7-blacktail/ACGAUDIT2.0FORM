using ACG_AUDIT.ClassCollections;
using Microsoft.Win32;
using System;
using System.Collections.Generic;

namespace ACG_AUDIT.Services
{
    internal class InstalledSoftwareService
    {
        public static InstalledSoftwareList CollectInstalledSoftware()
        {
            InstalledSoftwareList installedSoftwareList = new InstalledSoftwareList();

            // Verifica as chaves de registro para softwares instalados
            string[] registryKeys = new string[]
            {
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
                @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall"
            };

            foreach (string key in registryKeys)
            {
                using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(key))
                {
                    if (registryKey != null)
                    {
                        foreach (string subKeyName in registryKey.GetSubKeyNames())
                        {
                            using (RegistryKey subKey = registryKey.OpenSubKey(subKeyName))
                            {
                                if (subKey != null)
                                {
                                    string name = subKey.GetValue("DisplayName") as string;
                                    string version = subKey.GetValue("DisplayVersion") as string;
                                    string vendor = subKey.GetValue("Publisher") as string; // Coletando o fornecedor
                

                                    if (!string.IsNullOrEmpty(name))
                                    {
                                        InstalledSoftware software = new InstalledSoftware
                                        {
                                            Name = name,
                                            Version = version ?? "Desconhecida",
                                            Vendor = vendor ?? "Desconhecido",
                                        };
                                        installedSoftwareList.SoftwareList.Add(software);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // Ordena a lista de softwares em ordem alfabética pelo nome
            installedSoftwareList.SoftwareList.Sort((x, y) => string.Compare(x.Name, y.Name, StringComparison.OrdinalIgnoreCase));

            return installedSoftwareList;
        }
    }
}