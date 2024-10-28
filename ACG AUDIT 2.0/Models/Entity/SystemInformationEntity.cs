namespace ACG_AUDIT_2._0.Models.Entity
{
    public class SystemInformationEntity
    {
        public string HostName { get; set; }
        public string OperatingSystem { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string SystemType { get; set; }
        public string Processor { get; set; }
        public string BIOSVersion { get; set; }
        public string WindowsFolder { get; set; }
        public string SystemFolder { get; set; }
        public string BootDevice { get; set; }
        public string SystemLocale { get; set; }
        public string InputLocale { get; set; }
        public string TimeZone { get; set; }
        public string MemoryInfo { get; set; }
        public string VirtualMemoryInfo { get; set; }
        public string PageFileLocation { get; set; }
        public string DomainName { get; set; }
        public string LogonServer { get; set; }
        public string Hotfixes { get; set; }
        public string NetworkInfo { get; set; }
    }
}