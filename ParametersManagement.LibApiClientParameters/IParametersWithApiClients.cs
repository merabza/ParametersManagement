using System.Collections.Generic;
using ParametersManagement.LibParameters;

namespace ParametersManagement.LibApiClientParameters;

public interface IParametersWithApiClients : IParameters
{
    Dictionary<string, ApiClientSettings> ApiClients { get; }
}