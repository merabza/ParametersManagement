using DbTools;
using DbTools.Models;
using LibParameters;

namespace LibDatabaseParameters;

public sealed class DatabaseServerConnectionDataDomain : ItemData
{
    // ReSharper disable once ConvertToPrimaryConstructor
    public DatabaseServerConnectionDataDomain(EDataProvider dataProvider, string serverAddress,
        DbAuthSettingsBase dbAuthSettings, bool trustServerCertificate, string backupFolderName, string dataFolderName,
        string dataLogFolderName)
    {
        DataProvider = dataProvider;
        ServerAddress = serverAddress;
        DbAuthSettings = dbAuthSettings;
        BackupFolderName = backupFolderName;
        DataFolderName = dataFolderName;
        DataLogFolderName = dataLogFolderName;
        TrustServerCertificate = trustServerCertificate;
    }

    public EDataProvider DataProvider { get; set; }
    public string ServerAddress { get; set; }
    public DbAuthSettingsBase DbAuthSettings { get; }
    public bool TrustServerCertificate { get; }
    public string BackupFolderName { get; set; }
    public string DataFolderName { get; set; }
    public string DataLogFolderName { get; set; }
}