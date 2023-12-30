using System;
using LibParameters;
using SystemToolsShared;

namespace LibFileParameters.Models;

public sealed class FileStorageData : ItemData
{
    public string? FileStoragePath { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public int FileNameMaxLength { get; set; }
    public int FileSizeSplitPositionInRow { get; set; }
    public int FtpSiteLsFileOffset { get; set; }


    //public bool IsSameToLocal(string localPath)
    //{
    //  //ან თუ წყაროს ფაილსაცავი ლოკალურია და მისი ფოლდერი ემთხვევა პარამეტრების ლოკალურ ფოლდერს.
    //  //   მაშინ მოქაჩვა საჭირო აღარ არის
    //  if (!FileStat.IsFileSchema(FileStoragePath))
    //    return true;

    //  return FileStat.NormalizePath(localPath) != FileStat.NormalizePath(FileStoragePath);
    //}

    public bool? IsFileSchema()
    {
        if (string.IsNullOrWhiteSpace(FileStoragePath))
            return null;
        return FileStat.IsFileSchema(FileStoragePath);
    }

    public bool? IsFtp()
    {
        if (string.IsNullOrWhiteSpace(FileStoragePath))
            return null;
        if (!Uri.TryCreate(FileStoragePath, UriKind.Absolute, out var uri))
            return false;
        return uri.Scheme.ToLower() == "ftp";
    }

    public static bool IsSameToLocal(FileStorageData forFileStorage, string localPath)
        //,IMessagesDataManager? messagesDataManager, string? userName)
    {
        var fileStoragePath = forFileStorage.FileStoragePath;
        //ან თუ წყაროს ფაილსაცავი ლოკალურია და მისი ფოლდერი ემთხვევა პარამეტრების ლოკალურ ფოლდერს.
        //   მაშინ მოქაჩვა საჭირო აღარ არის
        if (fileStoragePath is null || string.IsNullOrWhiteSpace(localPath))
        {
            return false;
        }

        if (FileStat.IsFileSchema(fileStoragePath))
            return FileStat.NormalizePath(localPath) == FileStat.NormalizePath(fileStoragePath);
        return false;
    }
}