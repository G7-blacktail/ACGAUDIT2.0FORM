using ACG_AUDIT.ClassCollections;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ACG_AUDIT.Services
{
    internal class FirewallService
    {
        public static FirewallProfileList GetFirewallProfiles()
        {
            FirewallProfileList firewallProfileList = new FirewallProfileList();

            // Executar o comando PowerShell para obter perfis de firewall
            var psi = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                Arguments = "Get-NetFirewallProfile | Select-Object Name, Enabled"
            };

            using (var process = Process.Start(psi))
            {
                using (var reader = process.StandardOutput)
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        // Ignorar cabeçalho
                        if (line.Contains("Name") || string.IsNullOrWhiteSpace(line))
                            continue;

                        var parts = line.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length < 2) continue;

                        string nameProp = parts[0] switch
                        {
                            "Domain" => "Configurações de Rede de Domínio",
                            "Private" => "Configurações de Rede Privada",
                            "Public" => "Configurações de Rede Pública",
                            _ => parts[0]
                        };

                        string status = parts[1] == "True" ? "Habilitado" : "Desabilitado";

                        firewallProfileList.Profiles.Add(new FirewallProfile
                        {
                            Name = nameProp,
                            Status = status
                        });
                    }
                }
            }

            return firewallProfileList;
        }
    }
}