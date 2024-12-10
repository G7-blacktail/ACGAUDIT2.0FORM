using System;
using System.Diagnostics;

public class TimeInfo
{
    public static string GetTargetNtpServer()
    {
        // Obtém a configuração atual do cliente NTP
        string ntpConfig = ExecuteCommand("w32tm /query /configuration");
        string ntpServer = GetNtpServerFromConfig(ntpConfig);

        // Verifica se o servidor NTP configurado é o desejado
        string targetNtpServer = "a.st1.ntp.br";
        bool isTargetNtpServerConfigured = ntpServer.Contains(targetNtpServer);

        return isTargetNtpServerConfigured ? targetNtpServer : "Não configurado";
    }

    private static string ExecuteCommand(string command)
    {
        try
        {
            ProcessStartInfo processInfo = new ProcessStartInfo("cmd.exe", "/c " + command)
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            using (Process process = Process.Start(processInfo)!)
            {
                using (System.IO.StreamReader reader = process!.StandardOutput)
                {
                    return reader.ReadToEnd();
                }
            }
        }
        catch (Exception ex)
        {
            return $"Erro ao executar o comando: {ex.Message}";
        }
    }

    private static string GetNtpServerFromConfig(string configOutput)
    {
        foreach (var line in configOutput.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
        {
            if (line.Contains("NtpServer"))
            {
                return line.Replace("NtpServer: ", "").Trim();
            }
        }
        return "Desconhecido";
    }
}