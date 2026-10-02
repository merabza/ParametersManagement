using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SystemTools.SystemToolsShared;

namespace ParametersManagement.LibParameters;

public class ParametersManager : IParametersManager
{
    //ბექაპის სახელში ჩასმული თარიღის ფორმატი, მაგალითად SupportTools.json.20261001-213015-123.bak
    private const string BackupDateMask = "yyyyMMdd-HHmmss-fff";
    private const string BackupExtension = ".bak";

    //ფაილის რამდენი წინა ვერსია ინახება. უფრო ძველი ბექაპები იშლება
    private const int MaxBackupFilesCount = 10;

    //იგივე კოდირება, რასაც File.WriteAllText იყენებს: UTF-8 BOM-ის გარეშე
    private static readonly UTF8Encoding Utf8NoBom = new(false, true);

    public ParametersManager(IOptions<MainParametersManagerOptions> options)
    {
        ParametersFileName = options.Value.ParametersFileName;
        Parameters = options.Value.Par;
    }

    public ParametersManager(string? parametersFileName, IParameters parameters)
    {
        ParametersFileName = parametersFileName;
        Parameters = parameters;
    }

    public string? ParametersFileName { get; private set; }

    public IParameters Parameters { get; set; }

    public async ValueTask<bool> Save(IParameters parameters, string? message, string? saveAsFilePath = null,
        CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(saveAsFilePath))
        {
            ParametersFileName = saveAsFilePath;
        }

        if (!parameters.CheckBeforeSave())
        {
            StShared.WriteWarningLine("Something wrong with data for save", true, null, true);
            return false;
        }

        string paramsJsonText = JsonConvert.SerializeObject(parameters, Formatting.Indented);

        //იშიფრება მთლიანი json ტექსტი. ამის გამო შეუძლებელია მანქანებს შორის სეთინგების გადატანა
        //შესაბამისად შესაძლებელი უნდა იყოს გაშიფრული ვარიანტის შენახვა
        //if (_encKey != null)
        //{
        //    paramsJsonText = EncryptDecrypt.EncryptString(paramsJsonText, _encKey);
        //}

        string? filePathForSave = !string.IsNullOrWhiteSpace(saveAsFilePath) ? saveAsFilePath : ParametersFileName;

        if (string.IsNullOrWhiteSpace(filePathForSave))
        {
            StShared.WriteWarningLine("filePathForSave is empty, cannot save", true, null, true);
            return false;
        }

        //შევინახოთ პარამეტრების ფაილი. წინა ვერსია ბექაპად რჩება
        await WriteFileWithBackup(filePathForSave, paramsJsonText, cancellationToken);

        Parameters = parameters;
        if (string.IsNullOrWhiteSpace(message))
        {
            return true;
        }

        StShared.WriteSuccessMessage(message);
        return true;
    }

    private static async ValueTask WriteFileWithBackup(string filePath, string text,
        CancellationToken cancellationToken)
    {
        bool fileExists = File.Exists(filePath);

        //შიგთავსი არ შეცვლილა, ამიტომ ფაილი თავიდან აღარ იწერება. ერთი ოპერაცია ფაილს რამდენჯერმე ინახავს და
        //ამის გარეშე ბექაპები მიმდინარე ფაილის ასლებით გაივსებოდა
        if (fileExists && await File.ReadAllTextAsync(filePath, cancellationToken) == text)
        {
            return;
        }

        //ახალი შიგთავსი ჯერ იმავე ფოლდერის დროებით ფაილში იწერება და მერე ერთი ოპერაციით ანაცვლებს მთავარ ფაილს.
        //ასე ჩაწერის შუაში შეწყვეტა მთავარ ფაილს ვერ დააზიანებს
        string tempFilePath = $"{filePath}.{Guid.NewGuid():N}.tmp";
        try
        {
            await WriteToDisk(tempFilePath, text, cancellationToken);

            //მიმდინარე ფაილის შეცვლამდე მისი ასლი ბექაპად ინახება
            if (fileExists)
            {
                File.Copy(filePath, GetBackupFilePath(filePath), true);
            }

            File.Move(tempFilePath, filePath, true);
        }
        finally
        {
            //შეცდომისას დროებითი ფაილი არ უნდა დარჩეს
            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }
        }

        if (fileExists)
        {
            DeleteOldBackupFiles(filePath);
        }
    }

    //WriteThrough: მთავარი ფაილის ჩანაცვლებამდე მონაცემი დისკზე უნდა იყოს და არა მხოლოდ სისტემის ქეშში,
    //რომ კვების გათიშვის შემდეგ ჩანაცვლებული ფაილი ცარიელი არ აღმოჩნდეს
    private static async ValueTask WriteToDisk(string filePath, string text, CancellationToken cancellationToken)
    {
        // ReSharper disable once using
        await using var stream = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write, FileShare.None, 4096,
            FileOptions.WriteThrough | FileOptions.Asynchronous);
        await stream.WriteAsync(Utf8NoBom.GetBytes(text), cancellationToken);
    }

    private static string GetBackupFilePath(string filePath)
    {
        return $"{filePath}.{DateTime.Now.ToString(BackupDateMask, CultureInfo.InvariantCulture)}{BackupExtension}";
    }

    //იშლება მხოლოდ ამ კლასის შექმნილი ბექაპები, რომელთა სახელშიც ზუსტად BackupDateMask ფორმატის თარიღია.
    //ხელით გაკეთებულ ასლებს სხვა სახელები აქვს და ისინი ხელუხლებელი რჩება
    private static void DeleteOldBackupFiles(string filePath)
    {
        string fullFilePath = Path.GetFullPath(filePath);
        //ფაილის სრულ გზას ფოლდერი ყოველთვის აქვს: null მხოლოდ დისკის ფესვისთვის ბრუნდება
        string folderPath = Path.GetDirectoryName(fullFilePath)!;
        string backupFileNamePrefix = $"{Path.GetFileName(fullFilePath)}.";

        //თარიღის ფორმატის გამო სახელების დალაგება თარიღების დალაგებასაც ნიშნავს
        foreach (string oldBackupFilePath in Directory
                     .EnumerateFiles(folderPath, $"{backupFileNamePrefix}*{BackupExtension}")
                     .Where(x => IsBackupFileName(Path.GetFileName(x), backupFileNamePrefix))
                     .OrderByDescending(x => x, StringComparer.OrdinalIgnoreCase).Skip(MaxBackupFilesCount))
        {
            try
            {
                File.Delete(oldBackupFilePath);
            }
            catch (IOException e)
            {
                StShared.WriteWarningLine($"Old backup file {oldBackupFilePath} was not deleted: {e.Message}", true);
            }
            catch (UnauthorizedAccessException e)
            {
                StShared.WriteWarningLine($"Old backup file {oldBackupFilePath} was not deleted: {e.Message}", true);
            }
        }
    }

    private static bool IsBackupFileName(string fileName, string backupFileNamePrefix)
    {
        if (fileName.Length != backupFileNamePrefix.Length + BackupDateMask.Length + BackupExtension.Length ||
            !fileName.StartsWith(backupFileNamePrefix, StringComparison.OrdinalIgnoreCase) ||
            !fileName.EndsWith(BackupExtension, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        return DateTime.TryParseExact(fileName.AsSpan(backupFileNamePrefix.Length, BackupDateMask.Length),
            BackupDateMask, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
    }
}
