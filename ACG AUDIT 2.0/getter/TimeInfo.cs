using System;
using System.Diagnostics;

namespace ACG_AUDIT_2._0.getter
{
    public class TimeInfo
    {
        public static string GetTimeInfo()
        {
            string result = "Relógio:\n----------------------------\n";

            // Obtém a hora local
            DateTime localTime = DateTime.Now;
            string dateString = $"{localTime:dd/MM/yyyy HH:mm:ss}";

            // Executa o comando w32tm para sincronizar com um servidor NTP
            string ntpServerOut = "pool.ntp.org";
            string command = $"w32tm /stripchart /computer:{ntpServerOut} /samples:1 /dataonly /packetinfo";
            string ntpServerUsed = ExecuteCommand(command);

            // Obtém a configuração atual do cliente NTP
            string ntpConfig = ExecuteCommand("w32tm /query /configuration");
            string ntpServer = GetNtpServerFromConfig(ntpConfig);

            // Monta o resultado
            result += $"Hora atual do dispositivo: {dateString}\n";
            result += $"Servidor de Horário: {ntpServerUsed}\n";
            result += $"Servidor de horário na internet: {ntpServer}\n";

            return result;
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
            foreach (var line in configOutput.Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries))
            {
                if (line.Contains("NtpServer"))
                {
                    return line.Replace("NtpServer: ", "").Trim();
                }
            }
            return "Desconhecido";
        }
    }
}