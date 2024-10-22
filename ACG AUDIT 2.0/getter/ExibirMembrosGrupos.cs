using System;
using System.Diagnostics;

namespace ACG_AUDIT_2._0.getter;
public class GrupoAdministradores
{
    public void ExibirMembrosGrupos()
    {
        Console.WriteLine("Membros do grupo Administradores local:");
        ExecutarComando("net localgroup Administradores");

        Console.WriteLine("\n\n");
        Console.WriteLine("Membros do grupo Domain Admins no domínio:");
        ExecutarComando("net group \"Domain Admins\" /domain");
    }

    private void ExecutarComando(string comando)
    {
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = $"/c {comando}",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (Process process = Process.Start(startInfo)!)
        {
            using (System.IO.StreamReader reader = process!.StandardOutput)
            {
                string result = reader.ReadToEnd();
                Console.WriteLine(result);
            }
        }
    }
}