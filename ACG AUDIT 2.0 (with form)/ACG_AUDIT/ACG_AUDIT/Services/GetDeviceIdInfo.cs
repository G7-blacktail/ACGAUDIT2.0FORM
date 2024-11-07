using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace ACG_AUDIT.Services
{
    internal class GetDeviceIdInfo
    {
        public static void GetUUID()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystemProduct");
            Console.WriteLine("---------------------------------------------- Informações da placamãe -------------------------------------------");
            foreach (ManagementObject share in searcher.Get())
            {
                Console.WriteLine("UUID do dispositivo: " + share["UUID"]);
            }
            searcher = new ManagementObjectSearcher("SELECT * FROM Win32_BIOS");

            foreach (ManagementObject share in searcher.Get())
            {
                Console.WriteLine("Número de série do hardware: " + share["SerialNumber"]);
            }
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------");
        }
    }
}
