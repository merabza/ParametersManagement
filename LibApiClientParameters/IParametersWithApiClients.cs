using LibParameters;
using System.Collections.Generic;

namespace LibApiClientParameters;

public interface IParametersWithApiClients : IParameters
{
    Dictionary<string, ApiClientSettings> ApiClients { get; }
}