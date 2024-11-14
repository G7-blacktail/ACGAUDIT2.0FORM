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

            // Executar o execut�vel em segundo plano
            Task.Run(() =>
            {
                // Iniciar o execut�vel
                Process process = new Process();
                process.StartInfo.FileName = @"C:\Users\gustavo.fernandes\Documents\Lidersis\Modelos\ACG AUDIT 2.0\bin\Release\net8.0\win-x64\publish\ACG AUDIT 2.0.exe"; // Altere para o caminho do seu execut�vel
                process.Start();

                // Atualizar o status na tela de carregamento
                loadingForm.Invoke((MethodInvoker)delegate
                {
                    loadingForm.UpdateStatus("Execut�vel iniciado. Aguardando fechamento...");
                });

                // Aguardar o fechamento do execut�vel
                process.WaitForExit();

                // Atualizar o status ap�s o fechamento
                loadingForm.Invoke((MethodInvoker)delegate
                {
                    loadingForm.UpdateStatus("Execut�vel fechado. Finalizando...");
                });

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