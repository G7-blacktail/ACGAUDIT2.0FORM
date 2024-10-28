using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ACG_AUDIT_2._0.RegCollector
{
    public class FirewallInfo
    {
        public string? Nome { get; set; }
        public string? Estado { get; set; }

        public static List<FirewallInfo> GetFirewallProfiles()
        {
            List<FirewallInfo> firewallInfoList = new List<FirewallInfo>();

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
                using (var reader = process!.StandardOutput)
                {
                    string line;
                    while ((line = reader.ReadLine()!) != null)
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

                        string ativado = parts[1] == "True" ? "Habilitado" : "Desabilitado";

                        firewallInfoList.Add(new FirewallInfo
                        {
                            Nome = nameProp,
                            Estado = ativado
                        });
                    }
                }
            }

            return firewallInfoList;
        }

        public static void DisplayFirewallInfo()
        {
            var firewallProfiles = GetFirewallProfiles();
            Console.WriteLine("\nFirewall:");
            Console.WriteLine("----------------------------");
            foreach (var profile in firewallProfiles)
            {
                Console.WriteLine($"Nome: {profile.Nome}, Estado: {profile.Estado}");
            }
        }
    }
}