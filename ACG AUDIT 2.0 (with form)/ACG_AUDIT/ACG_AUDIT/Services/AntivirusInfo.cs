using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace ACG_AUDIT.Services
{
    internal class AntivirusInfo
    {
        public static string GetAntivirusInfo()
        {
            string result = "Antivirus:\n----------------------------\n";
            try
            {
                // Cria um objeto para buscar informações sobre produtos antivírus
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(
                    @"root\SecurityCenter2",
                    "SELECT * FROM AntivirusProduct");

                // Executa a busca e coleta os dados
                foreach (ManagementObject antivirus in searcher.Get())
                {
                    string displayName = antivirus["displayName"]?.ToString() ?? "Desconhecido";
                    string productState = antivirus["productState"]?.ToString() ?? "Desconhecido";

                    result += $"Nome: {displayName}, Estado: {productState}\n";
                }
            }
            catch (Exception ex)
            {
                result += $"Erro ao coletar informações: {ex.Message}\n";
            }

            return result;
        }
    }
}
