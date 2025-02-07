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
        private static readonly int timeDelay = 2000;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Criar a estrutura de diretórios se não existir
            CreateLogDirectories();

            // Criar e mostrar a tela de carregamento
            TelaInicial loadingForm = new TelaInicial();
            loadingForm.Show();

            // Variável para armazenar o conteúdo do config.json
            string configContent = string.Empty;

            // Caminho relativo para o arquivo de configuração
            string configFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), ".proprieties", ".acg_config", "config.json");

            // Ler o arquivo de configuração
            if (File.Exists(configFilePath))
            {
                try
                {
                    // Ler o conteúdo do arquivo
                    configContent = File.ReadAllText(configFilePath);

                    // Excluir o arquivo após ler o conteúdo
                    File.Delete(configFilePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao ler ou excluir o arquivo de configuração: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    loadingForm.Invoke((MethodInvoker)delegate
                    {
                        loadingForm.Close();
                    });
                    return;
                }
            }
            else
            {
                MessageBox.Show("Arquivo de configuração não encontrado, por gentiliza, \n reinicie o computador para que o agendador exexute normalmente.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                loadingForm.Invoke((MethodInvoker)delegate
                {
                    loadingForm.Close();
                });
                return;
            }

            // START //

            // Coletar e salvar informações do dispositivo e do sistema
            Task.Run(async () =>
            {
                try
                {

                    // Coletar informações do dispositivo
                    await UpdateStatusWithDelay("Coletando informações do dispositivo", timeDelay, loadingForm);
                    DeviceInfo deviceInfo = DeviceInfoCollector.CollectDeviceInfo();

                    // Coletar informações do sistema
                    SystemInfo systemInfo = SystemInfoService.CollectSystemInfo();
                    await UpdateStatusWithDelay("Informações do sistema coletadas", timeDelay, loadingForm);

                    // Coletar informações dos softwares instalados
                    InstalledSoftwareList installedSoftwareList = InstalledSoftwareService.CollectInstalledSoftware();
                    await UpdateStatusWithDelay("Informações dos softwares instalados coletadas", timeDelay, loadingForm);

                    // Coletar informações dos grupos de administradores
                    AdminGroupInfo adminGroupInfo = AdminGroupService.CollectAdminGroupInfo();
                    await UpdateStatusWithDelay("Informações do Grupo de administradores Coletadas", timeDelay, loadingForm);

                    // Coletar informações dos usuários e seus grupos
                    UserGroupList userGroupList = UserGroupService.CollectUserGroupInfo();
                    await UpdateStatusWithDelay("Informações dos usuários e seus grupos coletadas", timeDelay, loadingForm);

                    // Coletar informações dos perfis do firewall
                    FirewallProfileList firewallProfileList = FirewallService.GetFirewallProfiles();
                    await UpdateStatusWithDelay("Informações dos perfis do firewall coletadas", timeDelay, loadingForm);

                    // Coletar informações do antivírus
                    AntivirusProductList antivirusProductList = AntivirusService.GetAntivirusInfo();
                    await UpdateStatusWithDelay("Informações do antivírus coletadas", timeDelay, loadingForm);

                    // Coletar informações de acesso remoto
                    RemoteAccess remoteAccessInfo = RemoteAccessService.GetRemoteAccessInfo();
                    await UpdateStatusWithDelay("Informações de acesso remoto coletadas", timeDelay, loadingForm);

                    TimeInfo timeInfo = TimeService.GetTimeInfo();
                    await UpdateStatusWithDelay("Informações de data e hora coletadas", timeDelay, loadingForm);

                    // Coletar informações de proteção de tela
                    string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "GroupPolicy", "User", "Registry.pol"); // Caminho relativo ao dispositivo consultado
                    //string filePath = @"C:\Windows\System32\GroupPolicy\User\Registry.pol"; // caminho absoluto na minha maquina

                    ScreenSaverSettings screenSaverSettings = ScreenSaverService.GetScreenSaverSettings(filePath);
                    await UpdateStatusWithDelay("Informações de proteção de tela coletadas", timeDelay, loadingForm);
                    // END //

                    // Criar um objeto que combine as informações do dispositivo e do sistema
                    var collectedData = new
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

                    // Definir caminhos para os arquivos JSON
                    string finalJsonPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ACG Audit", "Inventario.json");
                    string systemLogPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ACG Audit", "acg audit files", "audit_info.json");

                    // Solicitar ao usuário se deseja continuar
                    loadingForm.Invoke((MethodInvoker)async delegate
                    {
                        DialogResult result = ShowConfirmationDialog();
                        if (result == DialogResult.Yes)
                        {
                            // Executar o outro programa antes de finalizar
                            string executablePath = @"c:\users\gustavo.fernandes\documents\lidersis\modelos\acg audit 2.0\bin\release\net8.0\publish\win-x86\ACG AUDIT 2.0.exe";

                            if (File.Exists(executablePath))
                            {
                                await UpdateStatusWithDelay("Abrindo o coletor avançado...", 5000, loadingForm);

                                ProcessStartInfo startInfo = new ProcessStartInfo
                                {
                                    FileName = executablePath,
                                    UseShellExecute = true,
                                    Verb = "runas" // Executar como administrador
                                };

                                await UpdateStatusWithDelay("Executando...", timeDelay, loadingForm);

                                try
                                {
                                    Process process = Process.Start(startInfo);
                                    if (process != null)
                                    {
                                        process.WaitForExit();
                                        await UpdateStatusWithDelay("Coletor avançado finalizado", timeDelay, loadingForm);
                                    }

                                    // Verificar se o audit_info.json foi gerado
                                    if (File.Exists(systemLogPath))
                                    {
                                        // Criar o Inventario.json com os dados da primeira e segunda etapas
                                        await JsonCreator.CreateInventoryJson(collectedData, finalJsonPath, systemLogPath, null);
                                    }
                                    else
                                    {
                                        // Criar o Inventario.json apenas com os dados da primeira etapa
                                        await JsonCreator.CreateInventoryJson(collectedData, finalJsonPath, null, null);
                                    }
                                }
                                catch (Win32Exception ex)
                                {
                                    await UpdateStatusWithDelay($"Erro ao iniciar o coletor: {ex.Message}", timeDelay, loadingForm);
                                }
                                catch (Exception ex)
                                {
                                    await UpdateStatusWithDelay($"Ocorreu um erro inesperado: {ex.Message}", timeDelay, loadingForm);
                                }
                            }
                            else
                            {
                                await UpdateStatusWithDelay("O executável não foi encontrado.", timeDelay, loadingForm);
                            }
                        }
                        else
                        {
                            ShowWarningDialog();
                            await UpdateStatusWithDelay("Operação cancelada pelo usuário.", timeDelay, loadingForm);
                        }
                    });

                    await Task.Delay(20000);

                    await UpdateStatusWithDelay("Etapa de envio das informações.", 5000, loadingForm);

                    // Envio do Inventario.json
                    var jsonSender = new JsonFileSenderService("http://localhost:18194/api/v1.0/pub/inventario/device-info");
                    bool envioBemSucedido = false; // Controle de estado

                    try
                    {
                        await jsonSender.SendJsonFileAsync(finalJsonPath);
                        envioBemSucedido = true; // Marca como bem-sucedido se não houver exceções
                        await UpdateStatusWithDelay("Envio executado com sucesso.", timeDelay, loadingForm);

                        if (string.IsNullOrEmpty(configContent))
                        {
                            MessageBox.Show("O conteúdo do arquivo de configuração estava vazio.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            File.WriteAllText(configFilePath, configContent);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Falha ao enviar as informações: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private static async Task UpdateStatusWithDelay(string message, int delay, TelaInicial loadingForm)
        {
            loadingForm.Invoke((MethodInvoker)delegate
            {
                loadingForm.UpdateStatus(message);
            });
            await Task.Delay(delay);
        }

        private static DialogResult ShowConfirmationDialog()
        {
            return MessageBox.Show("Deseja continuar com a execução?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        private static void ShowWarningDialog()
        {
            MessageBox.Show("A operação foi cancelada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}