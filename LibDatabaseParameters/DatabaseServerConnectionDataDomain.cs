using DbTools.Models;
using LibParameters;
using System.Collections.Generic;

namespace LibDatabaseParameters;

public sealed class DatabaseServerConnectionDataDomain : ItemData
{
    // ReSharper disable once ConvertToPrimaryConstructor
    public DatabaseServerConnectionDataDomain(EDatabaseProvider databaseServerProvider, string serverAddress,
        DbAuthSettingsBase dbAuthSettings, bool trustServerCertificate,
        Dictionary<string, DatabaseFoldersSet> databaseFoldersSets)
    {
        DatabaseServerProvider = databaseServerProvider;
        ServerAddress = serverAddress;
        DbAuthSettings = dbAuthSettings;
        TrustServerCertificate = trustServerCertificate;
        DatabaseFoldersSets = databaseFoldersSets;
    }

    public EDatabaseProvider DatabaseServerProvider { get; set; }
    public string ServerAddress { get; set; }
    public DbAuthSettingsBase DbAuthSettings { get; }
    public bool TrustServerCertificate { get; }
    public Dictionary<string, DatabaseFoldersSet> DatabaseFoldersSets { get; set; }
}