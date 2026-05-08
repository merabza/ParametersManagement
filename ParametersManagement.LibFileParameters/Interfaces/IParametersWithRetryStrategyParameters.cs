using System.Collections.Generic;
using ParametersManagement.LibFileParameters.Models;
using ParametersManagement.LibParameters;

namespace ParametersManagement.LibFileParameters.Interfaces;

public interface IParametersWithRetryStrategyParameters : IParameters
{
    Dictionary<string, RetryStrategyParameters> RetryStrategyParameters { get; }
}
