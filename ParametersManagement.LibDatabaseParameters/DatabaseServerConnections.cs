using System.Collections.Generic;

namespace ParametersManagement.LibDatabaseParameters;

public sealed class DatabaseServerConnections
{
    private readonly Dictionary<string, DatabaseServerConnectionData> _databaseServerConnections;

    // ReSharper disable once ConvertToPrimaryConstructor
    public DatabaseServerConnections(Dictionary<string, DatabaseServerConnectionData> databaseServerConnections)
    {
        _databaseServerConnections = databaseServerConnections;
    }

    public DatabaseServerConnectionData? GetDatabaseServerConnectionByKey(string key)
    {
        return _databaseServerConnections.GetValueOrDefault(key);
    }
}
