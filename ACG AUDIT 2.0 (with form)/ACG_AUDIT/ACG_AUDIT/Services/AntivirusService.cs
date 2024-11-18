using ACG_AUDIT.ClassCollections;
using System;
using System.Management;

namespace ACG_AUDIT.Services
{
    internal class AntivirusService
    {
        public static AntivirusProductList GetAntivirusInfo()
        {
            AntivirusProductList antivirusProductList = new AntivirusProductList();

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

                    antivirusProductList.Products.Add(new AntivirusProduct
                    {
                        DisplayName = displayName,
                        ProductState = productState
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao coletar informações do antivírus: {ex.Message}");
            }

            return antivirusProductList;
        }
    }
}