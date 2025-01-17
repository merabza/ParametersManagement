using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace LibFileParameters.Models;

public sealed class Archivers
{
    private readonly Dictionary<string, ArchiverData> _archivers;


    // ReSharper disable once ConvertToPrimaryConstructor
    public Archivers(Dictionary<string, ArchiverData> archivers)
    {
        _archivers = archivers;
    }

    public static Archivers Create(IConfiguration configuration)
    {
        var archiversSettings = configuration.GetSection("Archivers");
        var archivers = archiversSettings.Get<Dictionary<string, ArchiverData>>();
        return new Archivers(archivers ?? new Dictionary<string, ArchiverData>());
    }

    public ArchiverData? GetArchiverDataByKey(string key)
    {
        //_logger.LogError($"File storage with name {key} not found");
        return _archivers.TryGetValue(key, out var value) ? value : null;
    }
}