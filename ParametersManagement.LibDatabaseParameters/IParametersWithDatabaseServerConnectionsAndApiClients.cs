using ParametersManagement.LibApiClientParameters;

namespace ParametersManagement.LibDatabaseParameters;

public interface IParametersWithDatabaseServerConnectionsAndApiClients : IParametersWithDatabaseServerConnections,
    IParametersWithApiClients
{
}
