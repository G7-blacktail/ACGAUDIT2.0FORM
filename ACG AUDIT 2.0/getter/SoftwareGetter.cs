using Microsoft.Win32;
using System;
using System.Collections.Generic;

namespace RegistryPolReader
{
    internal class SoftwareCollector
    {
        public static void CollectAndDisplayInstalledSoftwares()
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
            Console.WriteLine();
            // Exibe as informações sobre os softwares instalados
            foreach (Software software in softwares)
            {
                Console.WriteLine("Nome: " + software.Nome);
                Console.WriteLine("Versão: " + software.Versao);
            }
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
}