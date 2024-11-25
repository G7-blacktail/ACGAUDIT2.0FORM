using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace ACG_AUDIT_2._0.Services.RegCollector;

public class AuditPolicyInfo
{
    public Dictionary<string, string> PolicyValues { get; private set; }
    private readonly string filePath;

    public AuditPolicyInfo(string filePath)
    {
        this.filePath = filePath;
        PolicyValues = new Dictionary<string, string>();
        UpdatePolicyFile();
        LoadPolicy();
    }

    private void UpdatePolicyFile()
    {
        // Verifica se o arquivo existe e executa o secedit para criar/atualizar
        if (!File.Exists(filePath))
        {
            // Console.WriteLine("Arquivo audit_policies.inf não encontrado. Criando/Atualizando o arquivo...");
            ExecuteSecedit();
        }
        else
        {
            // Console.WriteLine("Arquivo audit_policies.inf encontrado. Atualizando o arquivo...");
            ExecuteSecedit();
        }
    }

    private void ExecuteSecedit()
    {
        // Comando para exportar as políticas de auditoria
        string command = $"secedit /export /areas SECURITYPOLICY /cfg \"{filePath}\" /log \"{filePath}.log\"";
        ProcessStartInfo processInfo = new ProcessStartInfo("cmd.exe", "/c " + command)
        {
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (Process process = Process.Start(processInfo)!)
        {
            process.WaitForExit();
            string output = process.StandardOutput.ReadToEnd();
            Console.WriteLine(output);
        }
    }

    private void LoadPolicy()
    {
        if (!File.Exists(filePath))
        {
            // Console.WriteLine("O arquivo audit_policies.inf não foi encontrado.");
            return;
        }

        string[] lines = File.ReadAllLines(filePath);
        string currentSection = null!;

        foreach (var line in lines)
        {
            // Ignorar linhas vazias e comentários
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith(';'))
                continue;

            // Verificar se a linha é uma seção
            if (line.StartsWith('[') && line.EndsWith(']'))
            {
                currentSection = line.Trim('[', ']');
            }
            else if (currentSection != null)
            {
                var keyValue = line.Split(['='], 2);
                if (keyValue.Length == 2)
                {
                    PolicyValues[keyValue[0].Trim()] = keyValue[1].Trim();
                }
            }
        }

        // Filtrar as políticas relevantes
        PolicyValues = FilterPolicyValues(PolicyValues);
    }

    private Dictionary<string, string> FilterPolicyValues(Dictionary<string, string> policyValues)
    {
        // Mapeamento dos eventos de auditoria relevantes
        var relevantEventNames = new[]
        {
            "AuditLogonEvents",
            "AuditSystemEvents",
            "AuditObjectAccess",
            "AuditPrivilegeUse",
            "AuditPolicyChange",
            "AuditAccountManage",
            "AuditProcessTracking",
            "AuditDSAccess",
            "AuditAccountLogon"
        };

        var filteredPolicyValues = new Dictionary<string, string>();

        foreach (var eventName in relevantEventNames)
        {
            if (policyValues.TryGetValue(eventName, out string value))
            {
                filteredPolicyValues[eventName] = value;
            }
        }

        return filteredPolicyValues;
 }

    public void SaveFilteredPolicyToFile()
    {
        // Definir o caminho para salvar o arquivo filtrado
        string logDirectory = @"C:\Logs\acg audit files";
        Directory.CreateDirectory(logDirectory); // Cria o diretório se não existir

        string filteredFilePath = Path.Combine(logDirectory, "filtered_audit_policy.json");

        // Serializar as políticas filtradas em JSON
        string json = JsonSerializer.Serialize(PolicyValues, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filteredFilePath, json);

        // Console.WriteLine($"As políticas filtradas foram salvas em {filteredFilePath}");
    }
}