using DbTools;
using LibParameters;

namespace LibDatabaseParameters;

public sealed class DatabaseParameters : ParametersWithStatus
{
    public const EDatabaseRecoveryModel DefaultDatabaseRecoveryModel = EDatabaseRecoveryModel.Full;
    public const EBackupType DefaultBackupType = EBackupType.Full;

    public const bool DefaultCompress = true;
    public const bool DefaultVerify = true;
    public const string DefaultDateMask = "yyyyMMddHHmmss";
    public const string DefaultBackupFileExtension = ".bak";
    public const string DefaultBackupNameMiddlePart = "_FullDb_";

    //მონაცემთა ბაზასთან კავშირის სახელი
    public string? DbConnectionName { get; set; }
    public EDatabaseRecoveryModel? DatabaseRecoveryModel { get; set; }
    public string? DbServerFoldersSetName { get; set; }
    public string? DatabaseName { get; set; }

    //ჭკვიანი სქემის სახელი. გამოიყენება ძველი დასატოვებელი და წასაშლელი ფაილების განსასაზღვრად. (ბაზის სერვერის მხარეს)
    //ბაზის სერვერის მხარეს არსებული ჭკვიანი სქემის სახელებიუს მოტვირთვა შესაძლებელი უნდა იყოს რედაქტირების დროს
    public string? SmartSchemaName { get; set; }

    //სერვერის მხარეს ფაილსაცავის ნიკი
    //ბაზის ბექაპების გაცვლისათვის გამოსადეგი ფაილსაცავების ნიკების მოტვირთვა შესაძლებელი უნდა იყოს რედაქტირების დროს
    public string? FileStorageName { get; set; }
    public int CommandTimeOut { get; set; }
    public bool SkipBackupBeforeRestore { get; set; }
    public string? BackupNamePrefix { get; set; }
    public string? DateMask { get; set; }
    public string? BackupFileExtension { get; set; }
    public string? BackupNameMiddlePart { get; set; }
    public bool? Compress { get; set; }
    public bool? Verify { get; set; }
    public EBackupType? BackupType { get; set; }
}