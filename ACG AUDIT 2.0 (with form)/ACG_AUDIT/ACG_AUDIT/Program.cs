//using System.Threading.Tasks;
//using System.Windows.Forms;
//using ACG_AUDIT.ClassCollections;
//using ACG_AUDIT.Services;

//namespace ACG_AUDIT
//{
//    internal static class Program
//    {
//        [STAThread]
//        static void Main()
//        {
//            Application.EnableVisualStyles();
//            Application.SetCompatibleTextRenderingDefault(false);

//            // Criar e mostrar a tela de carregamento
//            TelaInicial loadingForm = new TelaInicial();
//            loadingForm.Show();

//            // Coletar e salvar informa��es do dispositivo e do sistema
//            Task.Run(async () =>
//            {

//                await Task.Delay(2000);
//                // Coletar informa��es do dispositivo
//                DeviceInfo deviceInfo = DeviceInfoCollector.CollectDeviceInfo();
//                loadingForm.Invoke((MethodInvoker)delegate
//                {
//                    loadingForm.UpdateStatus("Informa��es do dispositivo coletadas.");
//                });

//                await Task.Delay(2000); // Atraso de 2 segundos

//                // Coletar informa��es do sistema
//                SystemInfo systemInfo = SystemInfoService.CollectSystemInfo();
//                loadingForm.Invoke((MethodInvoker)delegate
//                {
//                    loadingForm.UpdateStatus("Informa��es do sistema coletadas.");
//                });

//                await Task.Delay(2000); // Atraso de 2 segundos

//                // Coletar informa��es dos softwares instalados
//                InstalledSoftwareList installedSoftwareList = InstalledSoftwareService.CollectInstalledSoftware();
//                loadingForm.Invoke((MethodInvoker)delegate
//                {
//                    loadingForm.UpdateStatus("Informa��es dos softwares instalados coletadas.");
//                });

//                await Task.Delay(2000); // Atraso de 2 segundos

//                // Coletar informa��es dos grupos de administradores
//                AdminGroupInfo adminGroupInfo = AdminGroupService.CollectAdminGroupInfo();
//                loadingForm.Invoke((MethodInvoker)delegate
//                {
//                    loadingForm.UpdateStatus("Informa��es do Grupo de administradores Coletadas.");
//                });

//                await Task.Delay(2000);


//                // Coletar informa��es dos usu�rios e seus grupos
//                UserGroupList userGroupList = UserGroupService.CollectUserGroupInfo();
//                loadingForm.Invoke((MethodInvoker)delegate
//                {
//                    loadingForm.UpdateStatus("Informa��es dos usu�rios e seus grupos coletadas.");
//                });

//                await Task.Delay(2000); // Atraso de 2 segundos

//                // Coletar informa��es dos perfis do firewall
//                FirewallProfileList firewallProfileList = FirewallService.GetFirewallProfiles();
//                loadingForm.Invoke((MethodInvoker)delegate
//                {
//                    loadingForm.UpdateStatus("Informa��es dos perfis do firewall coletadas.");
//                });

//                await Task.Delay(2000); // Atraso de 2 segundos

//                // Coletar informa��es do antiv�rus
//                AntivirusProductList antivirusProductList = AntivirusService.GetAntivirusInfo();
//                loadingForm.Invoke((MethodInvoker)delegate
//                {
//                    loadingForm.UpdateStatus("Informa��es do antiv�rus coletadas.");
//                });

//                await Task.Delay(2000); // Atraso de 2 segundos

//                // Coletar informa��es de acesso remoto
//                RemoteAccess remoteAccessInfo = RemoteAccessService.GetRemoteAccessInfo();
//                loadingForm.Invoke((MethodInvoker)delegate
//                {
//                    loadingForm.UpdateStatus("Informa��es de acesso remoto coletadas.");
//                });

//                await Task.Delay(2000); // Atraso de 2 segundos

//                // Coletar informa��es de data e hora
//                TimeInfo timeInfo = TimeService.GetTimeInfo();
//                loadingForm.Invoke((MethodInvoker)delegate
//                {
//                    loadingForm.UpdateStatus("Informa��es de data e hora coletadas.");
//                });

//                await Task.Delay(2000); // Atraso de 2 segundos

//                // Coletar informa��es de prote��o de tela
//                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "GroupPolicy", "User", "Registry.pol"); // Substitua pelo caminho correto
//                ScreenSaverSettings screenSaverSettings = ScreenSaverService.GetScreenSaverSettings(filePath);
//                loadingForm.Invoke((MethodInvoker)delegate
//                {
//                    loadingForm.UpdateStatus("Informa��es de prote��o de tela coletadas.");
//                });

//                await Task.Delay(2000); // Atraso de 2 segundos

//                // Criar um objeto que combine as informa��es do dispositivo e do sistema
//                var combinedInfo = new
//                {
//                    DeviceInfo = deviceInfo,
//                    SystemInfo = systemInfo,
//                    InstalledSoftware = installedSoftwareList,
//                    AdminGroupInfo = adminGroupInfo,
//                    UserGroupInfo = userGroupList,
//                    FirewallProfiles = firewallProfileList,
//                    AntivirusProducts = antivirusProductList,
//                    RemoteAccessInfo = remoteAccessInfo,
//                    TimeInfo = timeInfo,
//                    ScreenSaverSettings = screenSaverSettings
//                };

//                // Salvar todas as informa��es em um �nico arquivo JSON
//                JsonFileService.SaveToJson(combinedInfo, "Program_info.json");

//                await Task.Delay(2000); // Atraso de 2 segundos

//                loadingForm.Invoke((MethodInvoker)delegate
//                {
//                    loadingForm.UpdateStatus("Informa��es salvas em Program_info.json.");
//                });

//                await Task.Delay(2000); // Atraso de 2 segundos


//                // Fechar a tela de carregamento no thread da interface do usu�rio
//                loadingForm.Invoke((MethodInvoker)delegate
//                {
//                    loadingForm.Close();
//                });
//            });

//            // Mant�m a aplica��o em execu��o
//            Application.Run(loadingForm); // Mantenha a aplica��o em execu��o at� que o formul�rio de carregamento seja fechado

//            // Finaliza a aplica��o ap�s o fechamento do formul�rio
//            Application.Exit();
//        }
//    }
//}

//using System;
//using System.Diagnostics;
//using System.IO;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using ACG_AUDIT.ClassCollections;
//using ACG_AUDIT.Services;

//namespace ACG_AUDIT
//{
//    internal static class Program
//    {
//        [STAThread]
//        static void Main()
//        {
//            Application.EnableVisualStyles();
//            Application.SetCompatibleTextRenderingDefault(false);

//            // Criar e mostrar a tela de carregamento
//            TelaInicial loadingForm = new TelaInicial();
//            loadingForm.Show();

//            // Coletar e salvar informa��es do dispositivo e do sistema
//            Task.Run(async () =>
//            {
//                try
//                {
//                    await Task.Delay(2000);

//                    // Coletar informa��es do dispositivo
//                    DeviceInfo deviceInfo = DeviceInfoCollector.CollectDeviceInfo();
//                    loadingForm.Invoke((MethodInvoker)delegate
//                    {
//                        loadingForm.UpdateStatus("Informa��es do dispositivo coletadas.");
//                    });

//                    await Task.Delay(2000); // Atraso de 2 segundos

//                    // Coletar informa��es do sistema
//                    SystemInfo systemInfo = SystemInfoService.CollectSystemInfo();
//                    loadingForm.Invoke((MethodInvoker)delegate
//                    {
//                        loadingForm.UpdateStatus("Informa��es do sistema coletadas.");
//                    });

//                    // Outros passos mantidos do seu c�digo original

//                    await Task.Delay(2000);

//                    // Salvar todas as informa��es em um �nico arquivo JSON
//                    var combinedInfo = new
//                    {
//                        DeviceInfo = deviceInfo,
//                        SystemInfo = systemInfo
//                        // Adicione os outros objetos que foram coletados
//                    };

//                    JsonFileService.SaveToJson(combinedInfo, "Program_info.json");

//                    loadingForm.Invoke((MethodInvoker)delegate
//                    {
//                        loadingForm.UpdateStatus("Informa��es salvas em Program_info.json.");
//                    });

//                    await Task.Delay(2000); // Atraso de 2 segundos

//                    // Executar o outro programa antes de finalizar
//                    string executablePath = @"C:\Users\gustavo.fernandes\Documents\Lidersis\Modelos\ACG AUDIT 2.0\bin\Debug\net8.0\ACG AUDIT 2.0.exe";

//                    if (File.Exists(executablePath))
//                    {
//                        loadingForm.Invoke((MethodInvoker)delegate
//                        {
//                            loadingForm.UpdateStatus("Abrindo o coletor avan�ado...");
//                        });

//                        ProcessStartInfo startInfo = new ProcessStartInfo
//                        {
//                            FileName = executablePath,
//                            UseShellExecute = true, // Necess�rio para execu��o com privil�gios
//                            Verb = "runas" // Solicita privil�gios administrativos
//                        };

//                        Process process = Process.Start(startInfo);
//                        if (process != null)
//                        {
//                            await Task.Run(() => process.WaitForExit()); // Aguardar a conclus�o do processo
//                            loadingForm.Invoke((MethodInvoker)delegate
//                            {
//                                loadingForm.UpdateStatus("Coletor avan�ado finalizado.");
//                            });
//                        }
//                    }
//                    else
//                    {
//                        loadingForm.Invoke((MethodInvoker)delegate
//                        {
//                            loadingForm.UpdateStatus("Coletor avan�ado n�o encontrado.");
//                        });
//                    }

//                    await Task.Delay(2000); // Atraso de 2 segundos

//                    // Fechar a tela de carregamento no thread da interface do usu�rio
//                    loadingForm.Invoke((MethodInvoker)delegate
//                    {
//                        loadingForm.Close();
//                    });
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show($"Erro: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                    loadingForm.Invoke((MethodInvoker)delegate
//                    {
//                        loadingForm.Close();
//                    });
//                }
//            });

//            // Mant�m a aplica��o em execu��o
//            Application.Run(loadingForm);

//            // Finaliza a aplica��o ap�s o fechamento do formul�rio
//            Application.Exit();
//        }
//    }
//}


using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using ACG_AUDIT.ClassCollections;
using ACG_AUDIT.Services;

namespace ACG_AUDIT
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Criar e mostrar a tela de carregamento
            TelaInicial loadingForm = new TelaInicial();
            loadingForm.Show();

            // Coletar e salvar informa��es do dispositivo e do sistema
            Task.Run(async () =>
            {
                try
                {
                    await Task.Delay(2000);

                    // Coletar informa��es do dispositivo
                    DeviceInfo deviceInfo = DeviceInfoCollector.CollectDeviceInfo();
                    loadingForm.Invoke((MethodInvoker)delegate
                    {
                        loadingForm.UpdateStatus("Informa��es do dispositivo coletadas.");
                    });

                    await Task.Delay(2000); // Atraso de 2 segundos

                    // Coletar informa��es do sistema
                    SystemInfo systemInfo = SystemInfoService.CollectSystemInfo();
                    loadingForm.Invoke((MethodInvoker)delegate
                    {
                        loadingForm.UpdateStatus("Informa��es do sistema coletadas.");
                    });

                    await Task.Delay(2000);

                    // Coletar informa��es dos softwares instalados
                    InstalledSoftwareList installedSoftwareList = InstalledSoftwareService.CollectInstalledSoftware();
                    loadingForm.Invoke((MethodInvoker)delegate
                    {
                        loadingForm.UpdateStatus("Informa��es dos softwares instalados coletadas.");
                    });

                    await Task.Delay(2000); // Atraso de 2 segundos

                    // Coletar informa��es dos grupos de administradores
                    AdminGroupInfo adminGroupInfo = AdminGroupService.CollectAdminGroupInfo();
                    loadingForm.Invoke((MethodInvoker)delegate
                    {
                        loadingForm.UpdateStatus("Informa��es do Grupo de administradores Coletadas.");
                    });

                    await Task.Delay(2000);


                    // Coletar informa��es dos usu�rios e seus grupos
                    UserGroupList userGroupList = UserGroupService.CollectUserGroupInfo();
                    loadingForm.Invoke((MethodInvoker)delegate
                    {
                        loadingForm.UpdateStatus("Informa��es dos usu�rios e seus grupos coletadas.");
                    });

                    await Task.Delay(2000); // Atraso de 2 segundos

                    // Coletar informa��es dos perfis do firewall
                    FirewallProfileList firewallProfileList = FirewallService.GetFirewallProfiles();
                    loadingForm.Invoke((MethodInvoker)delegate
                    {
                        loadingForm.UpdateStatus("Informa��es dos perfis do firewall coletadas.");
                    });

                    await Task.Delay(2000); // Atraso de 2 segundos

                    // Coletar informa��es do antiv�rus
                    AntivirusProductList antivirusProductList = AntivirusService.GetAntivirusInfo();
                    loadingForm.Invoke((MethodInvoker)delegate
                    {
                        loadingForm.UpdateStatus("Informa��es do antiv�rus coletadas.");
                    });

                    await Task.Delay(2000); // Atraso de 2 segundos

                    // Coletar informa��es de acesso remoto
                    RemoteAccess remoteAccessInfo = RemoteAccessService.GetRemoteAccessInfo();
                    loadingForm.Invoke((MethodInvoker)delegate
                    {
                        loadingForm.UpdateStatus("Informa��es de acesso remoto coletadas.");
                    });

                    await Task.Delay(2000); // Atraso de 2 segundos

                    // Coletar informa��es de data e hora
                    TimeInfo timeInfo = TimeService.GetTimeInfo();
                    loadingForm.Invoke((MethodInvoker)delegate
                    {
                        loadingForm.UpdateStatus("Informa��es de data e hora coletadas.");
                    });

                    await Task.Delay(2000); // Atraso de 2 segundos

                    // Coletar informa��es de prote��o de tela
                    string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "GroupPolicy", "User", "Registry.pol"); // Substitua pelo caminho correto
                    ScreenSaverSettings screenSaverSettings = ScreenSaverService.GetScreenSaverSettings(filePath);
                    loadingForm.Invoke((MethodInvoker)delegate
                    {
                        loadingForm.UpdateStatus("Informa��es de prote��o de tela coletadas.");
                    });

                    await Task.Delay(2000); // Atraso de 2 segundos

                    // Criar um objeto que combine as informa��es do dispositivo e do sistema
                    var combinedInfo = new
                    {
                        DeviceInfo = deviceInfo,
                        SystemInfo = systemInfo,
                        InstalledSoftware = installedSoftwareList,
                        AdminGroupInfo = adminGroupInfo,
                        UserGroupInfo = userGroupList,
                        FirewallProfiles = firewallProfileList,
                        AntivirusProducts = antivirusProductList,
                        RemoteAccessInfo = remoteAccessInfo,
                        TimeInfo = timeInfo,
                        ScreenSaverSettings = screenSaverSettings
                    };

                    JsonFileService.SaveToJson(combinedInfo, "Program_info.json");

                    loadingForm.Invoke((MethodInvoker)delegate
                    {
                        loadingForm.UpdateStatus("Informa��es salvas em Program_info.json.");
                    });

                    await Task.Delay(2000); // Atraso de 2 segundos

                    // Solicitar ao usu�rio se deseja continuar
                    loadingForm.Invoke((MethodInvoker)delegate
                    {
                        DialogResult result = ShowConfirmationDialog();
                        if (result == DialogResult.Yes)
                        {
                            // Executar o outro programa antes de finalizar
                            string executablePath = @"C:\Users\gustavo.fernandes\Documents\Lidersis\Modelos\ACG AUDIT 2.0\bin\Debug\net8.0\ACG AUDIT 2.0.exe";

                            if (File.Exists(executablePath))
                            {
                                loadingForm.UpdateStatus("Abrindo o coletor avan�ado...");

                                ProcessStartInfo startInfo = new ProcessStartInfo
                                {
                                    FileName = executablePath,
                                    UseShellExecute = true, // Necess�rio para execu��o com privil�gios
                                    Verb = "runas" // Solicita privil�gios administrativos
                                };

                                Process process = Process.Start(startInfo);
                                if (process != null)
                                {
                                    process.WaitForExit(); // Aguardar a conclus�o do processo
                                    loadingForm.UpdateStatus("Coletor avan�ado finalizado.");
                                }
                            }
                            else
                            {
                                loadingForm.UpdateStatus("Coletor avan�ado n�o encontrado.");
                            }
                        }
                        else
                        {
                            // Mostrar mensagem de aviso
                            ShowWarningDialog();
                        }
                    });

                    await Task.Delay(2000); // Atraso de 2 segundos

                    // Fechar a tela de carregamento no thread da interface do usu�rio
                    loadingForm.Invoke((MethodInvoker)delegate
                    {
                        loadingForm.Close();
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    loadingForm.Invoke((MethodInvoker)delegate
                    {
                        loadingForm.Close();
                    });
                }
            });

            // Mant�m a aplica��o em execu��o
            Application.Run(loadingForm);

            // Finaliza a aplica��o ap�s o fechamento do formul�rio
            Application.Exit();
        }

        private static DialogResult ShowConfirmationDialog()
        {
            return MessageBox.Show(
                "Deseja continuar e executar o coletor avan�ado? Algumas informa��es cr�ticas s� ser�o coletadas com esta etapa.",
                "Coletor Avan�ado",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
        }

        private static void ShowWarningDialog()
        {
            MessageBox.Show(
                "Ao n�o realizar a coleta completa algumas informa��es do comodato importantes n�o estar�o no relat�rio do invent�rio. Caso n�o seja administrador do sistema, entre em contato com ele ou conosco para coletar as informa��es completas e evitar bloqueio no sistema.",
                "Aviso Importante",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
    }
}
