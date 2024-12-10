namespace ACG_AUDIT_2._0.Services.RegCollector;
public class AuditInfo
{
    public Dictionary<string, string> BitLocker { get; set; }
    public Dictionary<string, string> AuditPolicy { get; set; }
    public string NtpServer { get; set; }

    public AuditInfo()
    {
        BitLocker = new Dictionary<string, string>();
        AuditPolicy = new Dictionary<string, string>();
        NtpServer = string.Empty;
    }
}