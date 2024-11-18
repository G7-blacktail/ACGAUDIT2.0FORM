using ACG_AUDIT.ClassCollections;
using System;
using System.Collections.Generic;
using System.Management;

namespace ACG_AUDIT.Services
{
    internal class UserGroupService
    {
        public static UserGroupList CollectUserGroupInfo()
        {
            UserGroupList userGroupList = new UserGroupList();
            var userGroups = new Dictionary<string, List<string>>();

            try
            {
                // Cria um objeto de pesquisa para a classe Win32_GroupUser 
                var groupSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_GroupUser ");

                // Preenche o dicionário com os grupos de cada usuário
                foreach (ManagementObject groupUser in groupSearcher.Get())
                {
                    string partComponent = groupUser["PartComponent"].ToString()!;
                    string groupComponent = groupUser["GroupComponent"].ToString()!;

                    // Extrai o nome do usuário e do grupo
                    string userName = ExtractName(partComponent);
                    string groupName = ExtractName(groupComponent);

                    // Adiciona o grupo ao dicionário
                    if (!userGroups.ContainsKey(userName))
                    {
                        userGroups[userName] = new List<string>();
                    }
                    userGroups[userName].Add(groupName);
                }

                // Cria um objeto de pesquisa para a classe Win32_UserAccount
                var userSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_UserAccount");

                // Preenche a lista de usuários e seus grupos
                foreach (ManagementObject user in userSearcher.Get())
                {
                    if (user["Disabled"] != null && !(bool)user["Disabled"])
                    {
                        UserGroup userGroup = new UserGroup
                        {
                            UserName = user["Name"].ToString() ?? "Desconhecido",
                            Description = user["Description"]?.ToString() ?? "Sem descrição",
                            Domain = user["Domain"]?.ToString() ?? "Desconhecido",
                            IsLocalAccount = (bool)(user["LocalAccount"] ?? false),
                        };

                        // Verifica se o usuário tem grupos associados
                        if (userGroups.ContainsKey(userGroup.UserName))
                        {
                            userGroup.Groups = userGroups[userGroup.UserName];
                        }

                        userGroupList.Users.Add(userGroup);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao coletar informações dos usuários e grupos: " + ex.Message);
            }

            return userGroupList;
        }

        private static string ExtractName(string component)
        {
            return component.Substring(component.IndexOf("Name=\"") + 6).Split('"')[0];
        }
    }
}