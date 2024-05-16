using System.Collections.Generic;
using LibParameters;

namespace LibApiClientParameters;

public interface IParametersWithApiClients : IParameters
{
    Dictionary<string, ApiClientSettings> ApiClients { get; }
}