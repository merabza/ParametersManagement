using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace LibFileParameters.Models;

public sealed class FileStorages
{
    private readonly Dictionary<string, FileStorageData> _fileStorages;

    //public საჭიროა supportTools პროექტისათვის
    public FileStorages(Dictionary<string, FileStorageData> fileStorages)
    {
        _fileStorages = fileStorages;
    }

    public static FileStorages Create(IConfiguration configuration)
    {
        var fileStoragesSettings = configuration.GetSection("FileStorages");
        var fileStorages = fileStoragesSettings.Get<Dictionary<string, FileStorageData>>();
        return new FileStorages(fileStorages ?? new Dictionary<string, FileStorageData>());
    }

    public FileStorageData? GetFileStorageDataByKey(string key)
    {
        return _fileStorages.TryGetValue(key, out var value) ? value : null;
    }
}