using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using ACG_AUDIT.ClassCollections;
using ACG_AUDIT.Services;
using System.Text.Json;
using System.ComponentModel;

namespace ACG_AUDIT
{
    internal static class Program
    {
        private static readonly string logsDirectory = @"C:\Logs\acg audit files";
        private static readonly string logsSubDirectory = Path.Combine(logsDirectory, "Logs");
        private static readonly string appdata = @"C:\Users\gustavo.fernandes\AppData\Roaming\ACG Audit";
        private static readonly string logsSubDirectoryAppData = Path.Combine(appdata, "acg audit files");

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Criar a estrutura de diret�rios se n�o existir
            CreateLogDirectories();

            // Criar e mostrar a tela de carregamento
            TelaInicial loadingForm = new TelaInicial();
            loadingForm.Show();

            // Vari�vel para armazenar o conte�do do config.json
            string configContent = string.Empty;

            // Caminho relativo para o arquivo de configura��o
            string configFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), ".proprieties", ".acg_config", "config.json");

            // Ler o arquivo de configura��o
            if (File.Exists(configFilePath))
            {
                try
                {
                    // Ler o conte�do do arquivo
                    configContent = File.ReadAllText(configFilePath);

                    // Excluir o arquivo ap�s ler o conte�do
                    File.Delete(configFilePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao ler ou excluir o arquivo de configura��o: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    loadingForm.Invoke((MethodInvoker)delegate
                    {
                        loadingForm.Close();
                    });
                    return;
                }
            }
            else
            {
                MessageBox.Show("Arquivo de configura��o n�o encontrado, por gentiliza, \n reinicie o computador para que o agendador exexute normalmente.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                loadingForm.Invoke((MethodInvoker)delegate
                {
                    loadingForm.Close();
                });
                return;
            }

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

                    await Task.Delay(2000);

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

                    await Task.Delay(2000);

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

                    await Task.Delay(2000);

                    // Coletar informa��es dos perfis do firewall
                    FirewallProfileList firewallProfileList = FirewallService.GetFirewallProfiles();
                    loadingForm.Invoke((MethodInvoker)delegate
                    {
                        loadingForm.UpdateStatus("Informa��es dos perfis do firewall coletadas.");
                    });

                    await Task.Delay(2000);

                    // Coletar informa��es do antiv�rus
                    AntivirusProductList antivirusProductList = AntivirusService.GetAntivirusInfo();
                    loadingForm.Invoke((MethodInvoker)delegate
                    {
                        loadingForm.UpdateStatus("Informa��es do antiv�rus coletadas.");
                    });

                    await Task.Delay(2000);

                    // Coletar informa��es de acesso remoto
                    RemoteAccess remoteAccessInfo = RemoteAccessService.GetRemoteAccessInfo();
                    loadingForm.Invoke((MethodInvoker)delegate
                    {
                        loadingForm.UpdateStatus("Informa��es de acesso remoto coletadas.");
                    });

                    await Task.Delay(2000);

                    TimeInfo timeInfo = TimeService.GetTimeInfo();
                    loadingForm.Invoke((MethodInvoker)delegate
                    {
                        loadingForm.UpdateStatus("Informa��es de data e hora coletadas.");
                    });

                    await Task.Delay(2000);

                    // Coletar informa��es de prote��o de tela
                    string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "GroupPolicy", "User", "Registry.pol"); // Substitua pelo caminho correto
                    //string filePath = @"C:\Windows\System32\GroupPolicy\User\Registry.pol";
                    ScreenSaverSettings screenSaverSettings = ScreenSaverService.GetScreenSaverSettings(filePath);
                    loadingForm.Invoke((MethodInvoker)delegate
                    {
                        loadingForm.UpdateStatus("Informa��es de prote��o de tela coletadas.");
                    });

                    await Task.Delay(2000);

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
                    // Definir o caminho para salvar o JSON na pasta AppData do usu�rio
                    string jsonFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ACG Audit", "acg audit files", "Program_info.json");

                    // Criar diret�rio se n�o existir
                    string directoryPath = Path.GetDirectoryName(jsonFilePath);
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    var options = new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                    };
                    JsonFileService.SaveToJson(combinedInfo, jsonFilePath, options);

                    loadingForm.Invoke((MethodInvoker)delegate
                    {
                        loadingForm.UpdateStatus("Informa��es salvas no arquivo b�sico, sem privil�gios avan�ados.");
                    });

                    await Task.Delay(5000);

                    // Solicitar ao usu�rio se deseja continuar
                    loadingForm.Invoke((MethodInvoker)async delegate
                    {
                        DialogResult result = ShowConfirmationDialog();
                        if (result == DialogResult.Yes)
                        {
                            // Executar o outro programa antes de finalizar
                            // string executablePath = @"C:\Program Files (x86)\ACG\acg\ACG AUDIT 2.0.exe";
                            string executablePath = @"c:\users\gustavo.fernandes\documents\lidersis\modelos\acg audit 2.0\bin\release\net8.0\publish\win-x86\ACG AUDIT 2.0.exe";

                            if (File.Exists(executablePath))
                            {
                                loadingForm.Invoke((MethodInvoker)delegate
                                {
                                    loadingForm.UpdateStatus("Abrindo o coletor avan�ado...");
                                });

                                await Task.Delay(5000);

                                ProcessStartInfo startInfo = new ProcessStartInfo
                                {
                                    FileName = executablePath,
                                    UseShellExecute = true,
                                    Verb = "runas" // Executar como administrador
                                };

                                loadingForm.Invoke((MethodInvoker)delegate
                                {
                                    loadingForm.UpdateStatus("Executando...");
                                });

                                await Task.Delay(2000);

                                try
                                {
                                    Process process = Process.Start(startInfo);
                                    if (process != null)
                                    {
                                        process.WaitForExit();
                                        loadingForm.Invoke((MethodInvoker)delegate
                                        {
                                            loadingForm.UpdateStatus("Coletor avan�ado finalizado.");
                                        });

                                        await Task.Delay(2000);
                                    }

                                    string systemLogPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ACG Audit", "acg audit files", "audit_info.json");
                                    if (File.Exists(systemLogPath))
                                    {
                                        string logContent = File.ReadAllText(systemLogPath);
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
                                            LogInfo = logInfo
                                        };
                                        //caminho final do arquivo
                                        string finalJsonPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ACG Audit", "Inventario.json");

                                        JsonFileService.SaveToJson(finalCombinedInfo, finalJsonPath, options);
                                        if (File.Exists(finalJsonPath) && File.Exists(jsonFilePath))
                                        {
                                            File.Delete(systemLogPath);
                                            File.Delete(jsonFilePath);
                                        }
                                    }
                                }
                                catch (Win32Exception ex)
                                {
                                    // Tratar a exce��o se o processo n�o puder ser iniciado
                                    loadingForm.Invoke((MethodInvoker)delegate
                                    {
                                        loadingForm.UpdateStatus($"Erro ao iniciar o coletor: {ex.Message}");
                                    });
                                }
                                catch (Exception ex)
                                {
                                    // Tratar outras exce��es
                                    loadingForm.Invoke((MethodInvoker)delegate
                                    {
                                        loadingForm.UpdateStatus($"Ocorreu um erro inesperado: {ex.Message}");
                                    });
                                }
                            }
                            else
                            {
                                loadingForm.Invoke((MethodInvoker)delegate
                                {
                                    loadingForm.UpdateStatus("O execut�vel n�o foi encontrado.");
                                });
                            }
                        }
                        else
                        {
                            ShowWarningDialog();
                            loadingForm.Invoke((MethodInvoker)delegate
                            {
                                loadingForm.UpdateStatus("Opera��o cancelada pelo usu�rio.");
                            });

                            await Task.Delay(2000);
                        }
                    });

                    await Task.Delay(20000);

                    loadingForm.Invoke((MethodInvoker)delegate
                    {
                        loadingForm.UpdateStatus("Etapa de envio das informa��es.");
                    });

                    await Task.Delay(5000);

                    // Ap�s salvar o JSON
                    // var jsonSender = new JsonFileSenderService("http://localhost:3000/data"); // Substitua pelo seu endpoint
                    var jsonSender = new JsonFileSenderService("http://localhost:18194/api/v1.0/pub/inventario/device-info");
                    bool envioBemSucedido = false; // Controle de estado

                    try
                    {
                        await jsonSender.SendJsonFileAsync(Path.Combine(appdata, "Inventario.json"));
                        envioBemSucedido = true; // Marca como bem-sucedido se n�o houver exce��es
                        loadingForm.Invoke((MethodInvoker)delegate
                        {
                            loadingForm.UpdateStatus("Envio executado com sucesso.");
                        });

                        await Task.Delay(2000);

                        try
                        {
                            if (string.IsNullOrEmpty(configContent))
                            {
                                MessageBox.Show("O conte�do do arquivo de configura��o estava vazio.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                File.WriteAllText(configFilePath, configContent);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Erro ao reescrever o arquivo de configura��o: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Exibe uma caixa de mensagem com o erro
                        MessageBox.Show($"Falha ao enviar as informa��es: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        // Fechar a tela de carregamento no thread da interface do usu�rio
                        loadingForm.Invoke((MethodInvoker)delegate
                        {
                            loadingForm.Close();
                        });
                        return; 
                    }

                    if (envioBemSucedido)
                    {
                        await Task.Delay(2000); 
                    }

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

            Application.Run(loadingForm);

            Application.Exit();

        }

        private static void CreateLogDirectories()
        {
            if (!Directory.Exists(logsDirectory))
            {
                Directory.CreateDirectory(logsDirectory);
            }

            if (!Directory.Exists(logsSubDirectory))
            {
                Directory.CreateDirectory(logsSubDirectory);
            }
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