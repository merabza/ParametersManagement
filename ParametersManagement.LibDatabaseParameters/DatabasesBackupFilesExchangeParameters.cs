using ParametersManagement.LibParameters;

namespace ParametersManagement.LibDatabaseParameters;

// ReSharper disable once ClassNeverInstantiated.Global
public sealed class DatabasesBackupFilesExchangeParameters : ParametersWithStatus
{
    public const string DefaultDownloadTempExtension = ".down!";
    public const string DefaultUploadTempExtension = ".up!";

    //ჩამოტვირთვისა და ატვირთვის დროებითი გაფართოებები
    public string? DownloadTempExtension { get; set; }
    public string? UploadTempExtension { get; set; }

    //გაცვლის ფაილსაცავის სახელი
    public string? ExchangeFileStorageName { get; set; }

    //ჭკვიანი სქემის სახელი. გამოიყენება ძველი დასატოვებელი და წასაშლელი ფაილების განსასაზღვრად. (ეს რეზერვაციის ფაილსაცავის მხარეს)
    public string? ExchangeSmartSchemaName { get; set; }

    //ლოკალური ფოლდერი
    public string? LocalPath { get; set; }

    //ჭკვიანი სქემის სახელი. გამოიყენება ძველი დასატოვებელი და წასაშლელი ფაილების განსასაზღვრად. (ეს ლოკალური ფოლდერის მხარეს)
    public string? LocalSmartSchemaName { get; set; }
}
