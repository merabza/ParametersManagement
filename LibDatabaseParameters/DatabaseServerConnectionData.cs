using System.Collections.Generic;
using DbTools.Models;
using LibParameters;

namespace LibDatabaseParameters;

public sealed class DatabaseServerConnectionData : ItemData
{
    private const string DefaultName = "Default";
    public EDatabaseProvider DatabaseServerProvider { get; set; } = EDatabaseProvider.None;

    //მონაცემთა ბაზასთან დამაკავშირებელი ვებაგენტის სახელი
    public string? DbWebAgentName { get; set; }

    //ვებაგენტის მხარეს მონაცემთა ბაზასთან დამაკავშირებელი სახელი
    public string? RemoteDbConnectionName { get; set; }

    public string? ServerAddress { get; set; }
    public bool WindowsNtIntegratedSecurity { get; set; }
    public string? ServerUser { get; set; }
    public string? ServerPass { get; set; }
    public bool TrustServerCertificate { get; set; }
    public int ConnectionTimeOut { get; set; }

    public bool Encrypt { get; set; }

    //public DatabaseBackupParametersModel? FullDbBackupParameters { get; set; }
    public Dictionary<string, DatabaseFoldersSet> DatabaseFoldersSets { get; set; } = [];

    public void SetDefaultFolders(DbServerInfo dbServerInfo)
    {
        if (!DatabaseFoldersSets.TryGetValue(DefaultName, out var defSet))
        {
            DatabaseFoldersSets.Add(DefaultName,
                new DatabaseFoldersSet
                {
                    Backup = dbServerInfo.BackupDirectory,
                    Data = dbServerInfo.DefaultDataDirectory,
                    DataLog = dbServerInfo.DefaultLogDirectory
                });
        }
        else
        {
            if (string.IsNullOrWhiteSpace(defSet.Backup) || defSet.Backup != dbServerInfo.BackupDirectory)
                defSet.Backup = dbServerInfo.BackupDirectory;

            if (string.IsNullOrWhiteSpace(defSet.Data) || defSet.Data != dbServerInfo.DefaultDataDirectory)
                defSet.Data = dbServerInfo.DefaultDataDirectory;

            if (string.IsNullOrWhiteSpace(defSet.DataLog) || defSet.DataLog != dbServerInfo.DefaultLogDirectory)
                defSet.DataLog = dbServerInfo.DefaultLogDirectory;
        }
    }
}