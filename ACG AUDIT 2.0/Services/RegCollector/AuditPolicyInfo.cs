using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

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
                Console.WriteLine("Arquivo audit_policies.inf não encontrado. Criando/Atualizando o arquivo...");
                ExecuteSecedit();
            }
            else
            {
                Console.WriteLine("Arquivo audit_policies.inf encontrado. Atualizando o arquivo...");
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
                Console.WriteLine("O arquivo audit_policies.inf não foi encontrado.");
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
        }

        public void DisplayPolicy() 
        {
            Console.WriteLine("\nPolíticas de Auditoria:");

            // Mapeamento das chaves para nomes amigáveis
            var displayNames = new Dictionary<string, string>
            {
                { "MaximumPasswordAge", "Tempo de vida máximo da senha" },
                { "MinimumPasswordLength", "Comprimento mínimo da senha" },
                { "PasswordComplexity", "A senha deve satisfazer a requisitos de complexidade" },
                { "PasswordHistorySize", "Aplicar histórico de senhas" },
                { "LockoutBadCount", "Limite de bloqueio de conta" },
                { "ResetLockoutCount", "Zerar contador de bloqueios de conta após" },
                { "LockoutDuration", "Duração do bloqueio de conta" }
            };

            // Exibir as políticas de senha
            foreach (var kvp in displayNames)
            {
                #pragma warning disable CS8600 // Conversão de literal nula ou possível valor nulo em tipo não anulável.
                if (PolicyValues.TryGetValue(kvp.Key, out string value))
                {
                    // Para PasswordComplexity, exibir Habilitado ou Desabilitado
                    if (kvp.Key == "PasswordComplexity")
                    {
                        Console.WriteLine($"{kvp.Value}: {(value == "1" ? "Habilitado" : "Desabilitado")}");
                    }
                    else
                    {
                        Console.WriteLine($"{kvp.Value}: {value}");
                    }
                }
#pragma         warning restore CS8600 // Conversão de literal nula ou possível valor nulo em tipo não anulável.
            }

            // Exibir as políticas de auditoria de eventos
            Console.WriteLine("\n[Event Audit]");
            
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

            // Exibir os eventos de auditoria
            foreach (var kvp in eventDisplayNames)
            {
#pragma warning disable CS8600 // Conversão de literal nula ou possível valor nulo em tipo não anulável.
                if (PolicyValues.TryGetValue(kvp.Key, out string value))
                {
                    // Mapeia o valor para a descrição correspondente
                    string resultDescription = resultMapping.ContainsKey(value) ? resultMapping[value] : value;
                    Console.WriteLine($"{kvp.Value}: {resultDescription}");
                }
#pragma warning restore CS8600 // Conversão de literal nula ou possível valor nulo em tipo não anulável.
            }
        }
    }