using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using ACG_AUDIT.ClassCollections;
using ACG_AUDIT.Services;
using System.Text.Json;

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

            // Coletar e salvar informações do dispositivo e do sistema
            Task.Run(async () =>
            {
                try
                {
                    await Task.Delay(2000);

                    // Coletar informações do dispositivo
                    DeviceInfo deviceInfo = DeviceInfoCollector.CollectDeviceInfo();
                    loadingForm.Invoke((MethodInvoker)delegate
                    {
                        loadingForm.UpdateStatus("Informações do dispositivo coletadas.");
                    });

                    await Task.Delay(2000); // Atraso de 2 segundos

                    // Coletar informações do sistema
                    SystemInfo systemInfo = SystemInfoService.CollectSystemInfo();
                    loadingForm.Invoke((MethodInvoker)delegate
                    {
                        loadingForm.UpdateStatus("Informações do sistema coletadas.");
                    });

                    await Task.Delay(2000);

                    // Coletar informações dos softwares instalados
                    InstalledSoftwareList installedSoftwareList = InstalledSoftwareService.CollectInstalledSoftware();
                    loadingForm.Invoke((MethodInvoker)delegate
                    {
                        loadingForm.UpdateStatus("Informações dos softwares instalados coletadas.");
                    });

                    await Task.Delay(2000); // Atraso de 2 segundos

                    // Coletar informações dos grupos de administradores
                    AdminGroupInfo adminGroupInfo = AdminGroupService.CollectAdminGroupInfo();
                    loadingForm.Invoke((MethodInvoker)delegate
                    {
                        loadingForm.UpdateStatus("Informações do Grupo de administradores Coletadas.");
                    });

                    await Task.Delay(2000);

                    // Coletar informações dos usuários e seus grupos
                    UserGroupList userGroupList = UserGroupService.CollectUserGroupInfo();
                    loadingForm.Invoke((MethodInvoker)delegate
                    {
                        loadingForm.UpdateStatus("Informações dos usuários e seus grupos coletadas.");
                    });

                    await Task.Delay(2000); // Atraso de 2 segundos

                    // Coletar informações dos perfis do firewall
                    FirewallProfileList firewallProfileList = FirewallService.GetFirewallProfiles();
                    loadingForm.Invoke((MethodInvoker)delegate
                    {
                        loadingForm.UpdateStatus("Informações dos perfis do firewall coletadas.");
                    });

                    await Task.Delay(2000); // Atraso de 2 segundos

                    // Coletar informações do antivírus
                    AntivirusProductList antivirusProductList = AntivirusService.GetAntivirusInfo();
                    loadingForm.Invoke((MethodInvoker)delegate
                    {
                        loadingForm.UpdateStatus("Informações do antivírus coletadas.");
                    });

                    await Task.Delay(2000); // Atraso de 2 segundos

                    // Coletar informações de acesso remoto
                    RemoteAccess remoteAccessInfo = RemoteAccessService.GetRemoteAccessInfo();
                    loadingForm.Invoke((MethodInvoker)delegate
                    {
                        loadingForm.UpdateStatus("Informações de acesso remoto coletadas.");
                    });

                    await Task.Delay(2000); // Atraso de 2 segundos

                    // Coletar informações de data e hora
                    TimeInfo timeInfo = TimeService.GetTimeInfo();
                    loadingForm.Invoke((MethodInvoker)delegate
                    {
                        loadingForm.UpdateStatus("Informações de data e hora coletadas.");
                    });

                    await Task.Delay(2000); // Atraso de 2 segundos

                    // Coletar informações de proteção de tela
                    string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "GroupPolicy", "User", "Registry.pol"); // Substitua pelo caminho correto
                    ScreenSaverSettings screenSaverSettings = ScreenSaverService.GetScreenSaverSettings(filePath);
                    loadingForm.Invoke((MethodInvoker)delegate
                    {
                        loadingForm.UpdateStatus("Informações de proteção de tela coletadas.");
                    });

                    await Task.Delay(2000); // Atraso de 2 segundos

                    // Criar um objeto que combine as informações do dispositivo e do sistema
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

                    // Salvar informações em JSON com tratamento de caracteres acentuados
                    var options = new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping // Permite caracteres acentuados
                    };
                    JsonFileService.SaveToJson(combinedInfo, "Program_info.json", options);

                    loadingForm.Invoke((MethodInvoker)delegate
                    {
                        loadingForm.UpdateStatus("Informações salvas em Program_info.json.");
                    });

                    await Task.Delay(2000); // Atraso de 2 segundos

                    // Solicitar ao usuário se deseja continuar
                    loadingForm.Invoke((MethodInvoker)delegate
                    {
                        DialogResult result = ShowConfirmationDialog();
                        if (result == DialogResult.Yes)
                        {
                            // Executar o outro programa antes de finalizar
                            string executablePath = @"C:\Users\gustavo.fernandes\Documents\Lidersis\Modelos\ACG AUDIT 2.0\bin\Debug\net8.0\ACG AUDIT 2.0.exe";

                            if (File.Exists(executablePath))
                            {
                                loadingForm.UpdateStatus("Abrindo o coletor avançado...");

                                ProcessStartInfo startInfo = new ProcessStartInfo
                                {
                                    FileName = executablePath,
                                    UseShellExecute = true, // Necessário para execução com privilégios
                                    Verb = "runas" // Solicita privilégios administrativos
                                };

                                Process process = Process.Start(startInfo);
                                if (process != null)
                                {
                                    process.WaitForExit(); // Aguardar a conclusão do processo
                                    loadingForm.UpdateStatus("Coletor avançado finalizado.");
                                }

                                // Ler o arquivo gerado no log e adicionar ao JSON
                                string logFilePath = Path.Combine(@"C:\Logs\acg audit files", "audit_info.json"); // Caminho do arquivo gerado
                                if (File.Exists(logFilePath))
                                {
                                    // Ler o conteúdo do log
                                    string logContent = File.ReadAllText(logFilePath);

                                    // Deserializar o conteúdo do log para um objeto
                                    var logInfo = JsonSerializer.Deserialize<Dictionary<string, object>>(logContent);

                                    var finalCombinedInfo = new
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
                                        ScreenSaverSettings = screenSaverSettings,
                                        LogInfo = logInfo // Adicionando o objeto deserializado
                                    };
                                    // Caminho para salvar o JSON final
                                    string finalJsonPath = Path.Combine(@"C:\Logs\acg audit files", "Program_info.json");

                                    // Salvar o JSON final com tratamento de caracteres acentuados
                                    JsonFileService.SaveToJson(finalCombinedInfo, finalJsonPath, options);

                                    // Excluir o arquivo anterior, se existir
                                    if (File.Exists(logFilePath))
                                    {
                                        File.Delete(logFilePath);
                                    }

                                }
                                else
                                {
                                    loadingForm.UpdateStatus("Arquivo de log não encontrado.");
                                }
                            }
                            else
                            {
                                loadingForm.UpdateStatus("Coletor avançado não encontrado.");
                            }
                        }
                        else
                        {
                            // Mostrar mensagem de aviso
                            ShowWarningDialog();
                        }
                    });

                    await Task.Delay(2000); // Atraso de 2 segundos

                    // Fechar a tela de carregamento no thread da interface do usuário
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

            // Mantém a aplicação em execução
            Application.Run(loadingForm);

            // Finaliza a aplicação após o fechamento do formulário
            Application.Exit();
        }

        private static DialogResult ShowConfirmationDialog()
        {
            return MessageBox.Show(
                "Deseja continuar e executar o coletor avançado? Algumas informações críticas só serão coletadas com esta etapa.",
                "Coletor Avançado",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
        }

        private static void ShowWarningDialog()
        {
            MessageBox.Show(
                "Ao não realizar a coleta completa algumas informações do comodato importantes não estarão no relatório do inventário. Caso não seja administrador do sistema, entre em contato com ele ou conosco para coletar as informações completas e evitar bloqueio no sistema.",
                "Aviso Importante",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

    }
}