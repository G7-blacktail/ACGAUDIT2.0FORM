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
        private static readonly string logsDirectory = @"C:\Logs\acg audit files";
        private static readonly string logsSubDirectory = Path.Combine(logsDirectory, "Logs");

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
                MessageBox.Show("Arquivo de configura��o n�o encontrado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    // string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "GroupPolicy", "User", "Registry.pol"); // Substitua pelo caminho correto
                    string filePath = @"C:\Windows\System32\GroupPolicy\User\Registry.pol";
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
                    // Definir o caminho para salvar o JSON na pasta AppData do usu�rio
                    string jsonFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ACG Audit", "Program_info.json");

                    // Criar diret�rio se n�o existir
                    string directoryPath = Path.GetDirectoryName(jsonFilePath);
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    // Salvar informa��es em JSON com tratamento de caracteres acentuados
                    var options = new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping // Permite caracteres acentuados
                    };
                    JsonFileService.SaveToJson(combinedInfo, jsonFilePath, options);

                    loadingForm.Invoke((MethodInvoker)delegate
                    {
                        loadingForm.UpdateStatus("Informa��es salvas em Program_info.json.");
                    });

                    await Task.Delay(5000); // Atraso de 2 segundos

                    // Solicitar ao usu�rio se deseja continuar
                    loadingForm.Invoke((MethodInvoker)async delegate
                    {
                        DialogResult result = ShowConfirmationDialog();
                        if (result == DialogResult.Yes)
                        {
                            // Executar o outro programa antes de finalizar
                            string executablePath = @"C:\Program Files (x86)\ACG\acg\ACG AUDIT 2.0.exe";

                            if (File.Exists(executablePath))
                            {
                                

                                loadingForm.Invoke((MethodInvoker)delegate
                                {
                                    loadingForm.UpdateStatus("Abrindo o coletor avan�ado...");
                                });

                                await Task.Delay(150000);

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
                                    loadingForm.Invoke((MethodInvoker)delegate
                                    {
                                        loadingForm.UpdateStatus("Coletor avan�ado finalizado.");
                                    });

                                    await Task.Delay(5000);

                                }

                                // Ler o arquivo gerado no log e adicionar ao JSON
                                string logFilePath = Path.Combine(@"C:\Logs\acg audit files", "audit_info.json"); // Caminho do arquivo gerado
                                if (File.Exists(logFilePath))
                                {
                                    // Ler o conte�do do log
                                    string logContent = File.ReadAllText(logFilePath);

                                    // Deserializar o conte�do do log para um objeto
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
                                    loadingForm.Invoke((MethodInvoker)delegate
                                    {
                                        loadingForm.UpdateStatus("Arquivo de log n�o encontrado.");
                                    });

                                    await Task.Delay(5000);
                                
                                }
                            }
                            else
                            {
                                loadingForm.Invoke((MethodInvoker)delegate
                                {
                                    loadingForm.UpdateStatus("Coletor avan�ado n�o encontrado.");
                                });

                                await Task.Delay(2000);
                            }
                        }
                        else
                        {
                            // Mostrar mensagem de aviso
                            ShowWarningDialog();
                        }
                    });

                    await Task.Delay(10000); // Atraso de 2 segundos

                    loadingForm.Invoke((MethodInvoker)delegate
                    {
                        loadingForm.UpdateStatus("Etapa de envio das informa��es.");
                    });

                    await Task.Delay(5000); // Atraso de 2 segundos

                    // Ap�s salvar o JSON
                    var jsonSender = new JsonFileSenderService("http://localhost:3000/data"); // Substitua pelo seu endpoint
                    bool envioBemSucedido = false; // Controle de estado

                    try
                    {
                        await jsonSender.SendJsonFileAsync(Path.Combine(logsDirectory, "Program_info.json"));
                        envioBemSucedido = true; // Marca como bem-sucedido se n�o houver exce��es
                        loadingForm.Invoke((MethodInvoker)delegate
                        {
                            loadingForm.UpdateStatus("Envio executado com sucesso.");
                        });

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
                        return; // Impede que o c�digo continue ap�s um erro
                    }

                    // Apenas aguarde o atraso se o envio foi bem-sucedido
                    if (envioBemSucedido)
                    {
                        await Task.Delay(2000); // Atraso de 2 segundos
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

            // O restante do seu c�digo permanece aqui...

            // Mant�m a aplica��o em execu��o
            Application.Run(loadingForm);

            // Finaliza a aplica��o ap�s o fechamento do formul�rio
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