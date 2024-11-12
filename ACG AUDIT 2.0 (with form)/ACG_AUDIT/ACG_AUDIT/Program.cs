using ACG_AUDIT.Services;

namespace ACG_AUDIT
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Criar e mostrar a tela de carregamento
            TelaInicial loadingForm = new TelaInicial();
            loadingForm.Show();

            // Executar o trabalho em segundo plano
            Task.Run(() =>
            {
                // Simulando v�rias etapas de um processo
                for (int i = 0; i < 5; i++)
                {
                    loadingForm.Invoke((MethodInvoker)delegate
                    {
                        loadingForm.UpdateStatus($"Executando etapa {i + 1}...");
                    });

                    // Simula trabalho em cada etapa
                    Thread.Sleep(2000); // Simula 2 segundos de trabalho
                }

                // Fechar a tela de carregamento no thread da interface do usu�rio
                loadingForm.Invoke((MethodInvoker)delegate
                {
                    loadingForm.Close();
                });
            });

            // Mant�m a aplica��o em execu��o
            Application.Run();
        }
    }
}