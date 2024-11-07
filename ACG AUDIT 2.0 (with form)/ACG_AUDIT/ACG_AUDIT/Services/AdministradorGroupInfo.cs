using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ACG_AUDIT.Services
{
    internal class AdministradorGroupInfo
    {
        public void ShowMembersGroup()
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

            using Process process = Process.Start(startInfo)!;
            using StreamReader reader = process!.StandardOutput;
            string result = reader.ReadToEnd();
            Console.WriteLine(result);
        }
    }
}
