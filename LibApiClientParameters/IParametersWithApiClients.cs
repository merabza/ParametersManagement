using System.Collections.Generic;
using LibParameters;

namespace LibApiClientParameters;

public interface IParametersWithApiClients : IParameters
{
    public Dictionary<string, ApiClientSettings> ApiClients { get; }
}