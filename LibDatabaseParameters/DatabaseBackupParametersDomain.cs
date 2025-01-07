using DbTools;
using LibParameters;
using SystemToolsShared;

namespace LibDatabaseParameters;

public sealed class DatabaseBackupParametersDomain : IParameters
{
    // ReSharper disable once ConvertToPrimaryConstructor
    //public გამოიყენება WebAgent-ში
    //, string? dbServerSideBackupPath
    public DatabaseBackupParametersDomain(string backupNamePrefix, string dateMask, string backupFileExtension,
        string backupNameMiddlePart, bool compress, bool verify, EBackupType backupType)
    {
        BackupNamePrefix = backupNamePrefix;
        DateMask = dateMask;
        BackupFileExtension = backupFileExtension;
        BackupNameMiddlePart = backupNameMiddlePart;
        Compress = compress;
        Verify = verify;
        BackupType = backupType;
        //DbServerSideBackupPath = dbServerSideBackupPath;
    }

    public string BackupNamePrefix { get; }
    public string DateMask { get; }
    public string BackupFileExtension { get; }
    public string BackupNameMiddlePart { get; }

    public bool Compress { get; }
    public bool Verify { get; }
    public EBackupType BackupType { get; }

    //public string? DbServerSideBackupPath { get; }

    public bool CheckBeforeSave()
    {
        return true;
    }

    //string? dbServerSideBackupPath
    public static DatabaseBackupParametersDomain? Create(DatabaseBackupParametersModel? dbBackupParameters)
    {
        if (dbBackupParameters is null)
        {
            StShared.WriteErrorLine("dbBackupParameters is null", true);
            return null;
        }

        if (string.IsNullOrWhiteSpace(dbBackupParameters.BackupNamePrefix))
        {
            StShared.WriteErrorLine("dbBackupParameters.BackupNamePrefix does not specified", true);
            return null;
        }

        if (string.IsNullOrWhiteSpace(dbBackupParameters.BackupFileExtension))
        {
            StShared.WriteErrorLine("dbBackupParameters.BackupFileExtension does not specified", true);
            return null;
        }

        if (string.IsNullOrWhiteSpace(dbBackupParameters.BackupNameMiddlePart))
        {
            StShared.WriteErrorLine("dbBackupParameters.BackupNameMiddlePart does not specified", true);
            return null;
        }

        return new DatabaseBackupParametersDomain(dbBackupParameters.BackupNamePrefix,
            string.IsNullOrWhiteSpace(dbBackupParameters.DateMask) ? "yyyyMMddHHmmss" : dbBackupParameters.DateMask,
            dbBackupParameters.BackupFileExtension, dbBackupParameters.BackupNameMiddlePart,
            dbBackupParameters.Compress, dbBackupParameters.Verify, dbBackupParameters.BackupType
            //, //dbServerName,
            //dbServerSideBackupPath
            );
    }

    public string GetPrefix(string databaseName)
    {
        return BackupNamePrefix + databaseName + BackupNameMiddlePart;
    }

    public string GetSuffix()
    {
        return BackupFileExtension.AddNeedLeadPart(".");
    }
}