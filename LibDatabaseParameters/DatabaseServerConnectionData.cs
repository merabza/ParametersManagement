using DbTools;
using LibParameters;

namespace LibDatabaseParameters;

public sealed class DatabaseServerConnectionData : ItemData
{
    public EDataProvider DataProvider { get; set; }
    public bool WindowsNtIntegratedSecurity { get; set; }
    public string? ServerAddress { get; set; }
    public string? ServerUser { get; set; }
    public string? ServerPass { get; set; }
    public string? BackupFolderName { get; set; }
    public string? DataFolderName { get; set; }
    public string? DataLogFolderName { get; set; }
    public bool TrustServerCertificate { get; set; }
    public int ConnectionTimeOut { get; set; }
    public bool Encrypt { get; set; }
}