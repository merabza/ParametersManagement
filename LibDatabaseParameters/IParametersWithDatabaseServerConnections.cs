using System.Collections.Generic;
using LibParameters;

namespace LibDatabaseParameters;

public interface IParametersWithDatabaseServerConnections : IParameters
{
    Dictionary<string, DatabaseServerConnectionData> DatabaseServerConnections { get; }
}