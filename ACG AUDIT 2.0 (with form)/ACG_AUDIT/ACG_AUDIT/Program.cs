//using System.Diagnostics;
//using System.Threading.Tasks;
//using System.Windows.Forms;

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

//            // Executar o executável em segundo plano
//            Task.Run(() =>
//            {
//                // Iniciar o executável
//                Process process = new Process();
//                process.StartInfo.FileName = @"C:\Users\gustavo.fernandes\Documents\Lidersis\Modelos\ACG AUDIT 2.0\bin\Release\net8.0\win-x64\publish\ACG AUDIT 2.0.exe"; // Altere para o caminho do seu executável
//                process.Start();

//                // Atualizar o status na tela de carregamento
//                loadingForm.Invoke((MethodInvoker)delegate
//                {
//                    loadingForm.UpdateStatus("Executável iniciado. Aguardando fechamento...");
//                });

//                // Aguardar o fechamento do executável
//                process.WaitForExit();

//                // Atualizar o status após o fechamento
//                loadingForm.Invoke((MethodInvoker)delegate
//                {
//                    loadingForm.UpdateStatus("Executável fechado. Finalizando...");
//                });

//                // Fechar a tela de carregamento no thread da interface do usuário
//                loadingForm.Invoke((MethodInvoker)delegate
//                {
//                    loadingForm.Close();
//                });
//            });

//            // Mantém a aplicação em execução
//            Application.Run(loadingForm); // Mantenha a aplicação em execução até que o formulário de carregamento seja fechado

//            // Finaliza a aplicação após o fechamento do formulário
//            Application.Exit();
//        }
//    }
//}

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

            // Coletar e salvar informações do dispositivo e do sistema
            Task.Run(async () =>
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

                await Task.Delay(2000); // Atraso de 2 segundos

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

                // Salvar todas as informações em um único arquivo JSON
                JsonFileService.SaveToJson(combinedInfo, "Program_info.json");

                await Task.Delay(2000); // Atraso de 2 segundos

                loadingForm.Invoke((MethodInvoker)delegate
                {
                    loadingForm.UpdateStatus("Informações salvas em Program_info.json.");
                });

                await Task.Delay(2000); // Atraso de 2 segundos


                // Fechar a tela de carregamento no thread da interface do usuário
                loadingForm.Invoke((MethodInvoker)delegate
                {
                    loadingForm.Close();
                });
            });

            // Mantém a aplicação em execução
            Application.Run(loadingForm); // Mantenha a aplicação em execução até que o formulário de carregamento seja fechado

            // Finaliza a aplicação após o fechamento do formulário
            Application.Exit();
        }
    }
}