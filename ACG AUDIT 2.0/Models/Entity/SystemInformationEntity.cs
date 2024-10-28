namespace ACG_AUDIT_2._0.Models.Entity
{
    public class SystemInformationEntity
    {
        public required string HostName { get; set; }
        public required string OperatingSystem { get; set; }
        public required string Manufacturer { get; set; }
        public required string Model { get; set; }
        public required string SystemType { get; set; }
        public required string Processor { get; set; }
        public required string BIOSVersion { get; set; }
        public required string WindowsFolder { get; set; }
        public required string SystemFolder { get; set; }
        public required string BootDevice { get; set; }
        public required string SystemLocale { get; set; }
        public required string InputLocale { get; set; }
        public required string TimeZone { get; set; }
        public required string MemoryInfo { get; set; }
        public required string VirtualMemoryInfo { get; set; }
        public required string PageFileLocation { get; set; }
        public required string DomainName { get; set; }
        public required string LogonServer { get; set; }
        public required string Hotfixes { get; set; }
        public required string NetworkInfo { get; set; }
    }
}