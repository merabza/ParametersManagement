using DbTools;
using LibParameters;
using SystemToolsShared;

namespace LibDatabaseParameters;

public sealed class DatabaseBackupParametersModel : IParameters
{
    public string? BackupNamePrefix { get; set; }
    public string? DateMask { get; set; }
    public string? BackupFileExtension { get; set; }
    public string? BackupNameMiddlePart { get; set; }

    public bool Compress { get; set; }
    public bool Verify { get; set; }
    public EBackupType BackupType { get; set; }

    public bool CheckBeforeSave()
    {
        return true;
    }


    public string GetPrefix(string databaseName)
    {
        return BackupNamePrefix + databaseName + BackupNameMiddlePart;
    }

    public string GetSuffix()
    {
        return (BackupFileExtension ?? string.Empty).AddNeedLeadPart(".");
    }
}