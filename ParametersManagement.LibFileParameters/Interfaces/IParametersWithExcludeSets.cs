using System.Collections.Generic;
using ParametersManagement.LibFileParameters.Models;
using ParametersManagement.LibParameters;

namespace ParametersManagement.LibFileParameters.Interfaces;

public interface IParametersWithExcludeSets : IParameters
{
    Dictionary<string, ExcludeSet> ExcludeSets { get; }
}
