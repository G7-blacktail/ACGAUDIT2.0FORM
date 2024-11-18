using ACG_AUDIT.ClassCollections;
using System;
using System.Diagnostics;
using System.IO;

namespace ACG_AUDIT.Services
{
    internal class AdminGroupService
    {
        public static AdminGroupInfo CollectAdminGroupInfo()
        {
            AdminGroupInfo adminGroupInfo = new AdminGroupInfo();

            try
            {
                // Coletar membros do grupo de administradores local
                adminGroupInfo.LocalAdmins = ExecuteCommand("net localgroup Administradores");

                // Coletar membros do grupo Domain Admins
                adminGroupInfo.DomainAdmins = ExecuteCommand("net group \"Domain Admins\" /domain");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao coletar informações do grupo de administradores: " + ex.Message);
            }

            return adminGroupInfo;
        }

        private static List<string> ExecuteCommand(string command)
        {
            List<string> outputLines = new List<string>();

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c {command}",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using Process process = Process.Start(startInfo);
            using StreamReader reader = process.StandardOutput;
            string result = reader.ReadToEnd();

            // Processar a saída para extrair os nomes dos usuários
            using (StringReader stringReader = new StringReader(result))
            {
                string line;
                while ((line = stringReader.ReadLine()) != null)
                {
                    // Adicione lógica para filtrar e adicionar os nomes dos usuários
                    if (!string.IsNullOrWhiteSpace(line) && !line.StartsWith("Membros do grupo") && !line.StartsWith("----"))
                    {
                        outputLines.Add(line.Trim());
                    }
                }
            }

            return outputLines;
        }
    }
}