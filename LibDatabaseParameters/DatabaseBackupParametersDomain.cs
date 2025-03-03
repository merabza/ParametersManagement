using DbTools;
using LibParameters;
using SystemToolsShared;

namespace LibDatabaseParameters;

public sealed class DatabaseBackupParametersDomain : IParameters
{
    // ReSharper disable once ConvertToPrimaryConstructor
    //public გამოიყენება WebAgent-ში
    // ReSharper disable once MemberCanBePrivate.Global
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
    }

    private string BackupNamePrefix { get; }
    public string DateMask { get; }
    private string BackupFileExtension { get; }
    private string BackupNameMiddlePart { get; }
    public bool Compress { get; }
    public bool Verify { get; }
    public EBackupType BackupType { get; }

    public bool CheckBeforeSave()
    {
        return true;
    }

    //public static DatabaseBackupParametersDomain Create(DatabaseBackupParametersModel? dbBackupParameters)
    //{
    //    var computerName = Environment.MachineName;

    //    return new DatabaseBackupParametersDomain(
    //        GetValueOrDefault(dbBackupParameters?.BackupNamePrefix, $"{computerName}_"),
    //        GetValueOrDefault(dbBackupParameters?.DateMask, "yyyyMMddHHmmss"),
    //        GetValueOrDefault(dbBackupParameters?.BackupFileExtension, ".bak"),
    //        GetValueOrDefault(dbBackupParameters?.BackupNameMiddlePart, "_FullDb_"),
    //        dbBackupParameters?.Compress ?? true, dbBackupParameters?.Verify ?? true,
    //        dbBackupParameters?.BackupType ?? EBackupType.Full);
    //}

    private static string GetValueOrDefault(string? val, string def)
    {
        return string.IsNullOrWhiteSpace(val) ? def : val;
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