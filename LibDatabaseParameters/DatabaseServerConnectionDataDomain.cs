using System.Collections.Generic;
using DbTools;
using DbTools.Models;
using LibParameters;

namespace LibDatabaseParameters;

public sealed class DatabaseServerConnectionDataDomain : ItemData
{
    // ReSharper disable once ConvertToPrimaryConstructor
    public DatabaseServerConnectionDataDomain(EDataProvider dataProvider, string serverAddress,
        DbAuthSettingsBase dbAuthSettings, bool trustServerCertificate,
        Dictionary<string, DatabaseFoldersSet> databaseFoldersSets)
    {
        DataProvider = dataProvider;
        ServerAddress = serverAddress;
        DbAuthSettings = dbAuthSettings;
        TrustServerCertificate = trustServerCertificate;
        DatabaseFoldersSets = databaseFoldersSets;
    }

    public EDataProvider DataProvider { get; set; }
    public string ServerAddress { get; set; }
    public DbAuthSettingsBase DbAuthSettings { get; }
    public bool TrustServerCertificate { get; }
    public Dictionary<string, DatabaseFoldersSet> DatabaseFoldersSets { get; set; }
}