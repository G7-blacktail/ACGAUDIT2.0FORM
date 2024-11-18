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

//            // Executar o execut�vel em segundo plano
//            Task.Run(() =>
//            {
//                // Iniciar o execut�vel
//                Process process = new Process();
//                process.StartInfo.FileName = @"C:\Users\gustavo.fernandes\Documents\Lidersis\Modelos\ACG AUDIT 2.0\bin\Release\net8.0\win-x64\publish\ACG AUDIT 2.0.exe"; // Altere para o caminho do seu execut�vel
//                process.Start();

//                // Atualizar o status na tela de carregamento
//                loadingForm.Invoke((MethodInvoker)delegate
//                {
//                    loadingForm.UpdateStatus("Execut�vel iniciado. Aguardando fechamento...");
//                });

//                // Aguardar o fechamento do execut�vel
//                process.WaitForExit();

//                // Atualizar o status ap�s o fechamento
//                loadingForm.Invoke((MethodInvoker)delegate
//                {
//                    loadingForm.UpdateStatus("Execut�vel fechado. Finalizando...");
//                });

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

                await Task.Delay(2000); // Atraso de 2 segundos

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

                // Salvar todas as informa��es em um �nico arquivo JSON
                JsonFileService.SaveToJson(combinedInfo, "Program_info.json");

                await Task.Delay(2000); // Atraso de 2 segundos

                loadingForm.Invoke((MethodInvoker)delegate
                {
                    loadingForm.UpdateStatus("Informa��es salvas em Program_info.json.");
                });

                await Task.Delay(2000); // Atraso de 2 segundos


                // Fechar a tela de carregamento no thread da interface do usu�rio
                loadingForm.Invoke((MethodInvoker)delegate
                {
                    loadingForm.Close();
                });
            });

            // Mant�m a aplica��o em execu��o
            Application.Run(loadingForm); // Mantenha a aplica��o em execu��o at� que o formul�rio de carregamento seja fechado

            // Finaliza a aplica��o ap�s o fechamento do formul�rio
            Application.Exit();
        }
    }
}