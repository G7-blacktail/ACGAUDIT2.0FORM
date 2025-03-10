using System;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Dynamic;
using Newtonsoft.Json;

public class Program
{
    private string jsonFilePath => Path.Combine(configFolderPath, "config.json");
    private static string logFilePath => Path.Combine(configFolderPath, "log.txt");
    private static string configFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), ".proprieties", ".acg_config");
    private static string proprietiesFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), ".proprieties");
    private DateTime ultimaExecucao;
    private DateTime proximaExecucao;
    private DateTime comparacao = DateTime.MinValue;
    private DateTime startTime;
    private readonly TimeSpan maxRuntime = TimeSpan.FromSeconds(20);

    private readonly string ACG_HOME;

    [DllImport("kernel32.dll")]
    static extern IntPtr GetConsoleWindow();

    [DllImport("user32.dll")]
    static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    static int SW_HIDE = 0;

    public Program()
    {
        ACG_HOME = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Acg Audit", "Acg Audit 2.0.1.exe");
        // ACG_HOME = @"C:\Users\Gusta\Documents\Lidersis\ACGAUDIT2.0FORM\2.0.1\Acg Audit 2.0.1\bin\Release\net8.0-windows\win-x64\Acg Audit 2.0.1.exe";
    }

    public void Iniciar()
    {

        CarregarDados();
        startTime = DateTime.Now;

        // Verificar se a data atual é maior ou igual à próxima execução e se o arquivo config.json não existe
        if (DateTime.Now >= proximaExecucao || !File.Exists(jsonFilePath))
        {
            ExecutarAcao(); // Executa o ACG AUDIT
        } 
            CriarPastaConfig();
            CriarArquivos();


        while (DateTime.Now - startTime < maxRuntime)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            
            while (DateTime.Now <= proximaExecucao) 
            {
                System.Threading.Thread.Sleep(100); // Evita alto consumo de CPU
                
                if (DateTime.Now - startTime >= maxRuntime) // Se passou do tempo máximo, encerra o loop
                {
                    return;
                }
            }

            stopwatch.Stop();

            if (stopwatch.Elapsed.TotalSeconds > 30) // Se demorou mais de 30s, interrompe
            {
                return;
            }

            ultimaExecucao = DateTime.Now;
            proximaExecucao = CalcularProximaExecucao();
            SalvarDados();
            EscreverLog();
        }
    }

    private void CarregarDados()
    {
        if (File.Exists(jsonFilePath))
        {
            string json = File.ReadAllText(jsonFilePath);
            dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(json) ?? new ExpandoObject();

            if (data != null)
            {
                ultimaExecucao = data.UltimaExecucao;
                proximaExecucao = data.ProximaExecucao;
                comparacao = proximaExecucao;
            }
        }
        else
        {
            ultimaExecucao = DateTime.MinValue;
            proximaExecucao = DateTime.Now;
        }
    }

    private void SalvarDados()
    {
        dynamic data = new { UltimaExecucao = ultimaExecucao, ProximaExecucao = proximaExecucao };
        string json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
        File.WriteAllText(jsonFilePath, json);
    }

    private void ExecutarAcao()
    {
        Process.Start(ACG_HOME);
    }

    private DateTime CalcularProximaExecucao()
    {
        DateTime dataHoraAtual = DateTime.Now;
        DateTime proximoMes = dataHoraAtual.AddMonths(1);
        proximaExecucao = new DateTime(proximoMes.Year, proximoMes.Month, 1, 0, 5, 0);
        return proximaExecucao;
    }

    private void EscreverLog()
    {
        File.AppendAllText(logFilePath, "Programa iniciado pelo Agendador de Tarefas." + Environment.NewLine);
        string log = $"{DateTime.Now} | {(comparacao < ultimaExecucao ? "  Após o prazo " : "Dentro do prazo")} | {ultimaExecucao} | {proximaExecucao}";
        File.AppendAllText(logFilePath, log + Environment.NewLine);
    }

    private void CriarPastaConfig()
    {
        if (!Directory.Exists(proprietiesFolderPath))
        {
            Directory.CreateDirectory(proprietiesFolderPath);
            new DirectoryInfo(proprietiesFolderPath).Attributes |= FileAttributes.Hidden;
            if (!Directory.Exists(configFolderPath))
            {
                Directory.CreateDirectory(configFolderPath);
            }
        }
    }

    private void CriarArquivos()
    {
        string path1 = Path.Combine(configFolderPath, jsonFilePath);
        string path2 = Path.Combine(configFolderPath, logFilePath);

        if (!File.Exists(path1))
        {
            CalcularProximaExecucao();
            var jsonData = new { UltimaExecucao = DateTime.Now, ProximaExecucao = proximaExecucao };
            string jsonContents = JsonConvert.SerializeObject(jsonData);
            File.WriteAllText(path1, jsonContents);
        }

        if (!File.Exists(path2))
        {
            File.WriteAllText(path2, "");
            EscreverLog();
        }
    }

    public static void Main()
    {
        IntPtr handle = GetConsoleWindow();
        ShowWindow(handle, SW_HIDE);
        Program programa = new Program();
        programa.Iniciar();
    }
}