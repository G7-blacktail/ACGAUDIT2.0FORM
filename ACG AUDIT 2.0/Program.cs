using System;
using System.IO;
using ACG_AUDIT_2._0.Services.RegCollector;
using System.Text.Json;

namespace ACG_AUDIT_2._0;

class Program
{
    static void Main(string[] args)
    {

        // Coletar informações do BitLocker
        var bitLockerInfo = BitLockerInfo.CheckBitLockerStatusForAllDisks();

        // Coletar informações de políticas de auditoria
        var auditPolicyInfo = new AuditPolicyInfo("audit_policies.inf");

        // Coletar informações do servidor NTP
        string ntpServer = TimeInfo.GetTargetNtpServer();

        // Criar uma instância de AuditInfo
        var auditInfo = new AuditInfo
        {
            BitLocker = bitLockerInfo,
            AuditPolicy = auditPolicyInfo.PolicyValues, // Usar as políticas filtradas
            NtpServer = ntpServer // Adiciona o servidor NTP ao objeto de auditoria
        };

        // Serializar AuditInfo em JSON com UTF-8
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping // Permite caracteres acentuados
        };

        // Serializar AuditInfo em JSON
        string json = JsonSerializer.Serialize(auditInfo, options);

        // Definir o caminho para salvar o arquivo
        string logDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ACG Audit", "acg audit files");
        Directory.CreateDirectory(logDirectory); // Cria o diretório se não existir

        string filePath = Path.Combine(logDirectory, "audit_info.json");
        File.WriteAllText(filePath, json);
    }
}