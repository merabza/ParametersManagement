using System.Collections.Generic;
using ParametersManagement.LibParameters;

namespace ParametersManagement.LibDatabaseParameters;

public interface IParametersWithDatabaseServerConnections : IParameters
{
    Dictionary<string, DatabaseServerConnectionData> DatabaseServerConnections { get; }
}
