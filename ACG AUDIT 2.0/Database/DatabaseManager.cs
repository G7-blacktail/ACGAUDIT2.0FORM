using MySql.Data.MySqlClient;
using ACG_AUDIT_2._0.Models.Entity;

namespace ACG_AUDIT_2._0.Database
{
    public class DatabaseManager
    {
        private readonly string connectionString = "Server=localhost;Database=acg_audit_test;UserID=root;Password=root;Charset=utf8mb4;";

        public void SaveSystemInformation(SystemInformationEntity systemInfo)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    INSERT INTO tb_computer_information 
                    (host_name, operation_system, Manufacture, Model, sistem_type, processor, bios_version, 
                    windows_folder, boot_device, system_location, input_locale, time_zone, 
                    memory_info, virtual_memory_info, page_file_location, domain_name, logon_server, hotfixes, network_info) 
                    VALUES 
                    (@HostName, @OperatingSystem, @Manufacturer, @Model, @SystemType, @Processor, @BIOSVersion, 
                    @WindowsFolder, @BootDevice, @SystemLocale, @InputLocale, @TimeZone, 
                    @MemoryInfo, @VirtualMemoryInfo, @PageFileLocation, @DomainName, @LogonServer, @Hotfixes, @NetworkInfo)";

                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@HostName", systemInfo.HostName);
                cmd.Parameters.AddWithValue("@OperatingSystem", systemInfo.OperatingSystem);
                cmd.Parameters.AddWithValue("@Manufacturer", systemInfo.Manufacturer);
                cmd.Parameters.AddWithValue("@Model", systemInfo.Model);
                cmd.Parameters.AddWithValue("@SystemType", systemInfo.SystemType);
                cmd.Parameters.AddWithValue("@Processor", systemInfo.Processor);
                cmd.Parameters.AddWithValue("@BIOSVersion", systemInfo.BIOSVersion);
                cmd.Parameters.AddWithValue("@WindowsFolder", systemInfo.WindowsFolder);
                cmd.Parameters.AddWithValue("@BootDevice", systemInfo.BootDevice);
                cmd.Parameters.AddWithValue("@SystemLocale", systemInfo.SystemLocale);
                cmd.Parameters.AddWithValue("@InputLocale", systemInfo.InputLocale);
                cmd.Parameters.AddWithValue("@TimeZone", systemInfo.TimeZone);
                cmd.Parameters.AddWithValue("@MemoryInfo", systemInfo.MemoryInfo);
                cmd.Parameters.AddWithValue("@VirtualMemoryInfo", systemInfo.VirtualMemoryInfo);
                cmd.Parameters.AddWithValue("@PageFileLocation", systemInfo.PageFileLocation);
                cmd.Parameters.AddWithValue("@DomainName", systemInfo.DomainName);
                cmd.Parameters.AddWithValue("@LogonServer", systemInfo.LogonServer);
                cmd.Parameters.AddWithValue("@Hotfixes", systemInfo.Hotfixes);
                cmd.Parameters.AddWithValue("@NetworkInfo", systemInfo.NetworkInfo);

                cmd.ExecuteNonQuery();
            }
        }
    }
}