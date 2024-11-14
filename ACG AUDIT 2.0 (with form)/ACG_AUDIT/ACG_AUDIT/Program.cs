//using ACG_AUDIT.Services;

//namespace ACG_AUDIT
//{
//    internal static class Program
//    {
//        /// <summary>
//        ///  The main entry point for the application.
//        /// </summary>
//        [STAThread]
//        static void Main()
//        {
//            Application.EnableVisualStyles();
//            Application.SetCompatibleTextRenderingDefault(false);

//            // Criar e mostrar a tela de carregamento
//            TelaInicial loadingForm = new TelaInicial();
//            loadingForm.Show();

//            // Executar o trabalho em segundo plano
//            Task.Run(() =>
//            {
//                // Simulando várias etapas de um processo
//                for (int i = 0; i < 5; i++)
//                {
//                    loadingForm.Invoke((MethodInvoker)delegate
//                    {
//                        loadingForm.UpdateStatus($"Executando etapa {i + 1}...");
//                    });

//                    // Simula trabalho em cada etapa
//                    Thread.Sleep(2000); // Simula 2 segundos de trabalho
//                }

//                // Fechar a tela de carregamento no thread da interface do usuário
//                loadingForm.Invoke((MethodInvoker)delegate
//                {
//                    loadingForm.Close();
//                });
//            });

//            // Mantém a aplicação em execução
//            Application.Run();
//        }
//    }
//}



using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

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

            // Executar o executável em segundo plano
            Task.Run(() =>
            {
                // Iniciar o executável
                Process process = new Process();
                process.StartInfo.FileName = @"C:\Users\gustavo.fernandes\Documents\Lidersis\Modelos\ACG AUDIT 2.0\bin\Release\net8.0\win-x64\publish\ACG AUDIT 2.0.exe"; // Altere para o caminho do seu executável
                process.Start();

                // Atualizar o status na tela de carregamento
                loadingForm.Invoke((MethodInvoker)delegate
                {
                    loadingForm.UpdateStatus("Executável iniciado. Aguardando fechamento...");
                });

                // Aguardar o fechamento do executável
                process.WaitForExit();

                // Atualizar o status após o fechamento
                loadingForm.Invoke((MethodInvoker)delegate
                {
                    loadingForm.UpdateStatus("Executável fechado. Finalizando...");
                });

                // Fechar a tela de carregamento no thread da interface do usuário
                loadingForm.Invoke((MethodInvoker)delegate
                {
                    loadingForm.Close();
                });
            });

            // Mantém a aplicação em execução
            Application.Run();
        }
    }
}