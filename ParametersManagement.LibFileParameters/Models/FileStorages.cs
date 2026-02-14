using System.Collections.Generic;

namespace ParametersManagement.LibFileParameters.Models;

public sealed class FileStorages
{
    private readonly Dictionary<string, FileStorageData> _fileStorages;

    //public საჭიროა supportTools პროექტისათვის
    // ReSharper disable once ConvertToPrimaryConstructor
    // ReSharper disable once MemberCanBePrivate.Global
    public FileStorages(Dictionary<string, FileStorageData> fileStorages)
    {
        _fileStorages = fileStorages;
    }

    public FileStorageData? GetFileStorageDataByKey(string key)
    {
        return _fileStorages.GetValueOrDefault(key);
    }
}
