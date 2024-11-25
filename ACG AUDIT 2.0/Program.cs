﻿using System;
using System.IO;
using ACG_AUDIT_2._0.Services.RegCollector;
using System.Text.Json;

namespace ACG_AUDIT_2._0;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Coletando informações...");

        // Coletar informações do BitLocker
        var bitLockerInfo = BitLockerInfo.CheckBitLockerStatusForAllDisks();

        // Coletar informações de políticas de auditoria
        var auditPolicyInfo = new AuditPolicyInfo("audit_policies.inf");

        // Criar uma instância de AuditInfo
        var auditInfo = new AuditInfo
        {
            BitLocker = bitLockerInfo,
            AuditPolicy = auditPolicyInfo.PolicyValues // Usar as políticas filtradas
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
        string logDirectory = @"C:\Logs\acg audit files";
        Directory.CreateDirectory(logDirectory); // Cria o diretório se não existir

        string filePath = Path.Combine(logDirectory, "audit_info.json");
        File.WriteAllText(filePath, json);

        Console.WriteLine($"As informações foram salvas em {filePath}");
    }
}