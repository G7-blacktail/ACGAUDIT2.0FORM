using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace ACG_AUDIT.Services
{
    internal class UserGroupInfo
    {
        public void ExibirUsuariosEGrupos()
        {
            Console.WriteLine("Listando todos os usuários e seus grupos:");

            // Cria um objeto de pesquisa para a classe Win32_UserAccount
            var userSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_UserAccount");

            // Cria um objeto de pesquisa para a classe Win32_GroupUser   
            var groupSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_GroupUser  ");

            // Armazena as associações de grupos em um dicionário
            var userGroups = new Dictionary<string, List<string>>();

            // Preenche o dicionário com os grupos de cada usuário
            foreach (ManagementObject groupUser in groupSearcher.Get())
            {
                string partComponent = groupUser["PartComponent"].ToString()!;
                string groupComponent = groupUser["GroupComponent"].ToString()!;

                // Extrai o nome do usuário e do grupo
                string userName = partComponent!.Substring(partComponent.IndexOf("Name=\"") + 6);
                userName = userName.Substring(0, userName.IndexOf("\""));
                string groupName = groupComponent!.Substring(groupComponent.IndexOf("Name=\"") + 6);
                groupName = groupName.Substring(0, groupName.IndexOf("\""));

                // Adiciona o grupo ao dicionário
                if (!userGroups.ContainsKey(userName))
                {
                    userGroups[userName] = new List<string>();
                }
                userGroups[userName].Add(groupName);
            }

            // Exibe as informações dos usuários e seus grupos
            foreach (ManagementObject user in userSearcher.Get())
            {
                if (user["Disabled"].Equals(false))
                {
                    Console.WriteLine("-----------------------------------");
                    Console.WriteLine("Usuário: {0}", user["Name"]);
                    Console.WriteLine("Descrição: {0}", user["Description"]);
                    Console.WriteLine("Domínio: {0}", user["Domain"]);
                    if (user["localAccount"].Equals(true)) Console.WriteLine("Conta Local: {0}", "Sim");
                    else Console.WriteLine("Conta Local: {0}", "Não");

                    // Verifica se o usuário tem grupos associados
                    if (userGroups.ContainsKey(user["Name"].ToString()!))
                    {
                        Console.WriteLine("Pertence aos grupos:");
                        foreach (var group in userGroups[user["Name"].ToString()!])
                        {
                            Console.WriteLine(" - {0}", group);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Não pertence a nenhum grupo.");
                    }
                }
            }
        }
    }
}
