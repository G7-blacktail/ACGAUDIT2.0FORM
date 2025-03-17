using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using ACG_AUDIT.ClassCollections;
using ACG_AUDIT.Services;
using System.Text.Json;
using System.ComponentModel;
using System.Security.Principal;
using ACG_AUDIT_2._0.Services.RegCollector;

namespace ACG_AUDIT
{
    internal static class Program
    {

        private static readonly string appdata = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ACG Audit");
        private static readonly string logsSubDirectoryAppData = Path.Combine(appdata, "acg audit files");
        private static readonly string logsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        private static readonly string logsSubDirectory = Path.Combine(logsDirectory, "acg audit files");
        private static readonly string agendadorPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Acg Audit", "lib", "Subsystem", "Agendador.exe");
        private static readonly string configFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), ".proprieties", ".acg_config", "config.json");
        private static readonly string finalJsonPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ACG Audit", "Inventario.json");
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
                loadingForm.Invoke((MethodInvoker)delegate
                {
                    if (!File.Exists(agendadorPath))
                    {
                        MessageBox.Show($"Erro ao tentar abrir o agendador", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        loadingForm.Close();
                    }
                    Process.Start(agendadorPath);
                });
                return;
            }

            // START //

            // Coletar e salvar informações do dispositivo e do sistema
            Task.Run(async () =>
            {
                try
                {
                    await Task.Delay(timeDelay);

                    // Coletar informações do dispositivo
                    DeviceInfo deviceInfo = DeviceInfoCollector.CollectDeviceInfo();
                    await UpdateStatusWithDelay("Coletando informações do dispositivo", timeDelay, loadingForm);

                    // Coletar informações do sistema
                    SystemInfo systemInfo = SystemInfoService.CollectSystemInfo();
                    await UpdateStatusWithDelay("Informações do sistema coletadas", timeDelay, loadingForm);

                    // Coletar informações dos softwares instalados
                    InstalledSoftwareList installedSoftwareList = InstalledSoftwareService.CollectInstalledSoftware();
                    await UpdateStatusWithDelay("Informações dos softwares instalados coletadas", timeDelay, loadingForm);

                    // Coletar informações dos grupos de administradores
                    AdminGroupInfo adminGroupInfo = AdminGroupService.CollectAdminGroupInfo();
                    await UpdateStatusWithDelay("Informações do Grupo de administradores Coletadas", timeDelay, loadingForm);

                    // Coletar informações dos usu�rios e seus grupos
                    UserGroupList userGroupList = UserGroupService.CollectUserGroupInfo();
                    await UpdateStatusWithDelay("Informações dos usuários e seus grupos coletadas", timeDelay, loadingForm);

                    // Coletar informações dos perfis do firewall
                    FirewallProfileList firewallProfileList = FirewallService.GetFirewallProfiles();
                    await UpdateStatusWithDelay("Informações dos perfis do firewall coletadas", timeDelay, loadingForm);

                    // Coletar informações do antiv�rus
                    AntivirusProductList antivirusProductList = AntivirusService.GetAntivirusInfo();
                    await UpdateStatusWithDelay("Informações do antivírus coletadas", timeDelay, loadingForm);

                    // Coletar informações de acesso remoto
                    RemoteAccess remoteAccessInfo = RemoteAccessService.GetRemoteAccessInfo();
                    await UpdateStatusWithDelay("Informações de acesso remoto coletadas", timeDelay, loadingForm);

                    // Coletar informações de proteçõo de tela
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
                        FirewallProfilesList = firewallProfileList,
                        AntivirusProductsList = antivirusProductList,
                        RemoteAccessInfo = remoteAccessInfo,
                        ScreenSaverSettings = screenSaverSettings
                    };


                    await JsonCreator.CreateInitialInventoryJson(collectedData, finalJsonPath, null);
                    // Definir o caminho para salvar o JSON na pasta AppData do usuário

                    //Solicitar ao usuário se deseja continuar
                    loadingForm.Invoke((MethodInvoker)async delegate
                    {
                        await CollectAndMergeData(loadingForm, finalJsonPath);
                    });

                    await Task.Delay(timeDelay * 10);

                    await UpdateStatusWithDelay("Etapa de envio das informações.", timeDelay * 2, loadingForm);

                    /* Após salvar o JSON
                     * Remover linhas referentes a ambientes de teste local e acgdev
                     */

                    var jsonSender = new JsonFileSenderService("https://acg.certificadoranacional.com/api/v1.0/pub/inventario/device-info");
                    //var jsonSender = new JsonFileSenderService("https://acgdev.certificadoranacional.com/api/v1.0/pub/inventario/device-info");
                    //var jsonSender = new JsonFileSenderService("http://localhost:18194/api/v1.0/pub/inventario/device-info");
                    bool envioBemSucedido = false; // Controle de estado

                    try
                    {
                        await jsonSender.SendJsonFileAsync(Path.Combine(finalJsonPath));
                        envioBemSucedido = true; // Marca como bem-sucedido se não houver exceções

                        await UpdateStatusWithDelay("Envio executado com sucesso.", timeDelay, loadingForm);

                        try
                        {
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
                            MessageBox.Show($"Erro ao reescrever o arquivo de configuração: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Exibe uma caixa de mensagem com o erro
                        MessageBox.Show($"Falha ao enviar as informações: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        // Fechar a tela de carregamento no thread da interface do usuário
                        loadingForm.Invoke((MethodInvoker)delegate
                        {
                            loadingForm.Close();
                        });
                        return;
                    }

                    if (envioBemSucedido)
                    {
                        await Task.Delay(timeDelay);
                    }

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
            if (!Directory.Exists(appdata))
            {
                Directory.CreateDirectory(appdata);
            }
            if (!Directory.Exists(logsSubDirectoryAppData))
            {
                Directory.CreateDirectory(logsSubDirectoryAppData);
            }
        }

        private static async Task CollectAndMergeData(TelaInicial loadingForm, string finalJsonPath)
        {
            // Verificar se o programa está sendo executado com privilégios elevados
            bool isElevated = new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);

            if (isElevated)
            {
                await UpdateStatusWithDelay("Coletando informações adicionais...", timeDelay * 3, loadingForm);

                // Coletar informações do BitLocker
                var bitLockerInfo = BitLockerInfo.CheckBitLockerStatusForAllDisks();

                // Coletar informações de políticas de auditoria
                var auditPolicyInfo = new AuditPolicyInfo("audit_policies.inf");

                // Coletar informações do servidor NTP
                string ntpServer = TimeInfo.GetTargetNtpServer();

                // Ler o conteúdo existente do Inventario.json
                var existingData = new
                {
                    BitLocker = bitLockerInfo,
                    AuditPolicy = auditPolicyInfo.PolicyValues,
                    NtpServer = ntpServer
                };

                // Verificar se o arquivo já existe
                if (File.Exists(finalJsonPath))
                {
                    // Ler o conteúdo existente
                    string existingJson = await File.ReadAllTextAsync(finalJsonPath);
                    var existingInventory = JsonSerializer.Deserialize<Dictionary<string, object>>(existingJson);

                    // Mesclar as novas informações
                    existingInventory["BitLocker"] = existingData.BitLocker;
                    existingInventory["AuditPolicy"] = existingData.AuditPolicy;
                    existingInventory["NtpServer"] = existingData.NtpServer;

                    // Serializar novamente e salvar
                    string mergedJson = JsonSerializer.Serialize(existingInventory, new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping // Permite caracteres acentuados
                    });
                    await File.WriteAllTextAsync(finalJsonPath, mergedJson);
                }
                else
                {
                    // Se o arquivo não existir, criar um novo
                    string newJson = JsonSerializer.Serialize(existingData, new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping // Permite caracteres acentuados
                    });
                    await File.WriteAllTextAsync(finalJsonPath, newJson);
                }

                await UpdateStatusWithDelay("Coleta de informações adicionais finalizada.", timeDelay, loadingForm);
            }
            else
            {
                await UpdateStatusWithDelay("Executando sem privilégios elevados.", timeDelay, loadingForm);
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

    }
}