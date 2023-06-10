using LibApiClientParameters;

namespace LibDatabaseParameters;

public interface IParametersWithDatabaseServerConnectionsAndApiClients : IParametersWithDatabaseServerConnections,
    IParametersWithApiClients
{
}