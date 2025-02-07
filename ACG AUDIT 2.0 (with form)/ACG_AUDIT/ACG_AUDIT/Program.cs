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

            // START //

            // Coletar e salvar informa��es do dispositivo e do sistema
            Task.Run(async () =>
            {
                try
                {

                    // Coletar informa��es do dispositivo
                    await UpdateStatusWithDelay("Coletando informa��es do dispositivo", timeDelay, loadingForm);
                    DeviceInfo deviceInfo = DeviceInfoCollector.CollectDeviceInfo();

                    // Coletar informa��es do sistema
                    SystemInfo systemInfo = SystemInfoService.CollectSystemInfo();
                    await UpdateStatusWithDelay("Informa��es do sistema coletadas", timeDelay, loadingForm);

                    // Coletar informa��es dos softwares instalados
                    InstalledSoftwareList installedSoftwareList = InstalledSoftwareService.CollectInstalledSoftware();
                    await UpdateStatusWithDelay("Informa��es dos softwares instalados coletadas", timeDelay, loadingForm);

                    // Coletar informa��es dos grupos de administradores
                    AdminGroupInfo adminGroupInfo = AdminGroupService.CollectAdminGroupInfo();
                    await UpdateStatusWithDelay("Informa��es do Grupo de administradores Coletadas", timeDelay, loadingForm);

                    // Coletar informa��es dos usu�rios e seus grupos
                    UserGroupList userGroupList = UserGroupService.CollectUserGroupInfo();
                    await UpdateStatusWithDelay("Informa��es dos usu�rios e seus grupos coletadas", timeDelay, loadingForm);

                    // Coletar informa��es dos perfis do firewall
                    FirewallProfileList firewallProfileList = FirewallService.GetFirewallProfiles();
                    await UpdateStatusWithDelay("Informa��es dos perfis do firewall coletadas", timeDelay, loadingForm);

                    // Coletar informa��es do antiv�rus
                    AntivirusProductList antivirusProductList = AntivirusService.GetAntivirusInfo();
                    await UpdateStatusWithDelay("Informa��es do antiv�rus coletadas", timeDelay, loadingForm);

                    // Coletar informa��es de acesso remoto
                    RemoteAccess remoteAccessInfo = RemoteAccessService.GetRemoteAccessInfo();
                    await UpdateStatusWithDelay("Informa��es de acesso remoto coletadas", timeDelay, loadingForm);

                    TimeInfo timeInfo = TimeService.GetTimeInfo();
                    await UpdateStatusWithDelay("Informa��es de data e hora coletadas", timeDelay, loadingForm);

                    // Coletar informa��es de prote��o de tela
                    string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "GroupPolicy", "User", "Registry.pol"); // Caminho relativo ao dispositivo consultado
                    //string filePath = @"C:\Windows\System32\GroupPolicy\User\Registry.pol"; // caminho absoluto na minha maquina

                    ScreenSaverSettings screenSaverSettings = ScreenSaverService.GetScreenSaverSettings(filePath);
                    await UpdateStatusWithDelay("Informa��es de prote��o de tela coletadas", timeDelay, loadingForm);
                    // END //

                    // Criar um objeto que combine as informa��es do dispositivo e do sistema
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

                    // Solicitar ao usu�rio se deseja continuar
                    loadingForm.Invoke((MethodInvoker)async delegate
                    {
                        DialogResult result = ShowConfirmationDialog();
                        if (result == DialogResult.Yes)
                        {
                            // Executar o outro programa antes de finalizar
                            string executablePath = @"c:\users\gustavo.fernandes\documents\lidersis\modelos\acg audit 2.0\bin\release\net8.0\publish\win-x86\ACG AUDIT 2.0.exe";

                            if (File.Exists(executablePath))
                            {
                                await UpdateStatusWithDelay("Abrindo o coletor avan�ado...", 5000, loadingForm);

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
                                        await UpdateStatusWithDelay("Coletor avan�ado finalizado", timeDelay, loadingForm);
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
                                await UpdateStatusWithDelay("O execut�vel n�o foi encontrado.", timeDelay, loadingForm);
                            }
                        }
                        else
                        {
                            ShowWarningDialog();
                            await UpdateStatusWithDelay("Opera��o cancelada pelo usu�rio.", timeDelay, loadingForm);
                        }
                    });

                    await Task.Delay(20000);

                    await UpdateStatusWithDelay("Etapa de envio das informa��es.", 5000, loadingForm);

                    // Envio do Inventario.json
                    var jsonSender = new JsonFileSenderService("http://localhost:18194/api/v1.0/pub/inventario/device-info");
                    bool envioBemSucedido = false; // Controle de estado

                    try
                    {
                        await jsonSender.SendJsonFileAsync(finalJsonPath);
                        envioBemSucedido = true; // Marca como bem-sucedido se n�o houver exce��es
                        await UpdateStatusWithDelay("Envio executado com sucesso.", timeDelay, loadingForm);

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
                        MessageBox.Show($"Falha ao enviar as informa��es: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            return MessageBox.Show("Deseja continuar com a execu��o?", "Confirma��o", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        private static void ShowWarningDialog()
        {
            MessageBox.Show("A opera��o foi cancelada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}