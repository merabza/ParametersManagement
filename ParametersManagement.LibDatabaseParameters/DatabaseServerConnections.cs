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

    //public static DatabaseServerConnections Create(IConfiguration configuration)
    //{
    //    var databaseServerConnectionsParameters = configuration.GetSection("DatabaseServerConnections");
    //    var databaseServerConnections =
    //        databaseServerConnectionsParameters.Get<Dictionary<string, DatabaseServerConnectionData>>();
    //    return new DatabaseServerConnections(databaseServerConnections ??
    //                                         new Dictionary<string, DatabaseServerConnectionData>());
    //}

    public DatabaseServerConnectionData? GetDatabaseServerConnectionByKey(string key)
    {
        return _databaseServerConnections.GetValueOrDefault(key);
    }
}