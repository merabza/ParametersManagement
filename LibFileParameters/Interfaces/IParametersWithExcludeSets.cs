using System.Collections.Generic;
using LibFileParameters.Models;
using LibParameters;

namespace LibFileParameters.Interfaces;

public interface IParametersWithExcludeSets : IParameters
{
    Dictionary<string, ExcludeSet> ExcludeSets { get; }
}