using LibParameters;
using System.Collections.Generic;

namespace LibDatabaseParameters;

public interface IParametersWithDatabaseServerConnections : IParameters
{
    Dictionary<string, DatabaseServerConnectionData> DatabaseServerConnections { get; }
}