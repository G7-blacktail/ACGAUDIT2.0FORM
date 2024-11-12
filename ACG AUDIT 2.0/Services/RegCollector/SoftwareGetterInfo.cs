// using Microsoft.Win32;
// using System;
// using System.Collections.Generic;

// namespace ACG_AUDIT_2._0.Services.RegCollector;

//     internal class SoftwareCollectorInfo
//     {
//         public static void CollectAndDisplayInstalledSoftwares()
//         {
//             // Abre a chave do Registro do Windows que contém as informações sobre os softwares instalados
//             RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall")!;

//             // Cria uma lista para armazenar as informações sobre os softwares instalados
//             List<Software> softwares = new List<Software>();

//             // Percorre as subchaves da chave do Registro do Windows que contém as informações sobre os softwares instalados
//             foreach (string subkey in key.GetSubKeyNames())
//             {
//                 // Abre a subchave do Registro do Windows que contém as informações sobre o software instalado
//                 RegistryKey subkeyKey = key.OpenSubKey(subkey)!;

//                 // Obtem as informações sobre o software instalado
//                 string nome = (string)subkeyKey.GetValue("DisplayName")!;
//                 string versao = (string)subkeyKey.GetValue("DisplayVersion")!;

//                 // Cria um objeto Software para armazenar as informações sobre o software instalado
//                 Software software = new Software(nome, versao);

//                 // Adiciona o objeto Software à lista de softwares
//                 softwares.Add(software);
//             }

//             // Fecha a chave do Registro do Windows
//             key.Close();
//             Console.WriteLine();
//             Console.WriteLine("--------------------------------------- Softwares Instalados ------------------------------------------------------");
//             // Exibe as informações sobre os softwares instalados
//             foreach (Software software in softwares)
//             {
//                 Console.WriteLine("Nome: " + software.Nome);
//                 Console.WriteLine("Versão: " + software.Versao);
//             }
//             Console.WriteLine("--------------------------------------------------------------------------------------------------------------------");
//         }
//     }

// class Software
// {
//     public string Nome { get; set; }
//     public string Versao { get; set; }

//     public Software(string nome, string versao)
//     {
//         Nome = nome;
//         Versao = versao;
//     }
// }


using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ACG_AUDIT_2._0.Services.RegCollector;

internal class SoftwareCollectorInfo
{
    public static List<Software> CollectInstalledSoftwares()
    {
        // Abre a chave do Registro do Windows que contém as informações sobre os softwares instalados
        RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall")!;

        // Cria uma lista para armazenar as informações sobre os softwares instalados
        List<Software> softwares = new List<Software>();

        // Percorre as subchaves da chave do Registro do Windows que contém as informações sobre os softwares instalados
        foreach (string subkey in key.GetSubKeyNames())
        {
            // Abre a subchave do Registro do Windows que contém as informações sobre o software instalado
            RegistryKey subkeyKey = key.OpenSubKey(subkey)!;

            // Obtem as informações sobre o software instalado
            string nome = (string)subkeyKey.GetValue("DisplayName")!;
            string versao = (string)subkeyKey.GetValue("DisplayVersion")!;

            // Cria um objeto Software para armazenar as informações sobre o software instalado
            Software software = new Software(nome, versao);

            // Adiciona o objeto Software à lista de softwares
            softwares.Add(software);
        }

        // Fecha a chave do Registro do Windows
        key.Close();
        return softwares;
    }

    public static void SaveInstalledSoftwaresToJson(List<Software> softwares)
    {
        // Salvar as informações em um arquivo JSON
        string json = JsonSerializer.Serialize(softwares, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText("installed_softwares.json", json);
        Console.WriteLine("As informações dos softwares instalados foram salvas em installed_softwares.json");
    }
}

class Software
{
    public string Nome { get; set; }
    public string Versao { get; set; }

    public Software(string nome, string versao)
    {
        Nome = nome;
        Versao = versao;
    }
}