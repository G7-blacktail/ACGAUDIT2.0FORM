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
        ExecuteSecedit();
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

        // Filtrar e renomear as políticas relevantes
        PolicyValues = FilterAndRenamePolicyValues(PolicyValues);
    }

    private Dictionary<string, string> FilterAndRenamePolicyValues(Dictionary<string, string> policyValues)
    {
        // Mapeamento dos eventos de auditoria
        var eventDisplayNames = new Dictionary<string, string>
        {
            { "AuditLogonEvents", "Auditoria de eventos de logon" },
            { "AuditSystemEvents", "Auditoria de eventos de sistema" },
            { "AuditObjectAccess", "Auditoria de acesso a objetos" },
            { "AuditPrivilegeUse", "Auditoria de uso de privilégios" },
            { "AuditPolicyChange", "Auditoria de alteração de políticas" },
            { "AuditAccountManage", "Auditoria de gerenciamento de conta" },
            { "AuditProcessTracking", "Auditoria de acompanhamento de processos" },
            { "AuditDSAccess", "Auditoria de acesso ao serviço de diretório" },
            { "AuditAccountLogon", "Auditoria de eventos de logon de conta" }
        };

        // Mapeamento dos resultados
        var resultMapping = new Dictionary<string, string>
        {
            { "0", "Sem auditoria" },
            { "1", "Exito" },
            { "2", "Falha" },
            { "3", "Exito, Falha" }
        };

        var filteredPolicyValues = new Dictionary<string, string>();

        foreach (var eventName in eventDisplayNames.Keys)
        {
            if (policyValues.TryGetValue(eventName, out string value))
            {
                string displayName = eventDisplayNames[eventName];
                string resultDescription = resultMapping.ContainsKey(value) ? resultMapping[value] : value;
                filteredPolicyValues[displayName] = resultDescription; // Renomear chave e valor
            }
        }

        return filteredPolicyValues;
    }
}