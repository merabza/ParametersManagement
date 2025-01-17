using LibFileParameters.Models;
using LibParameters;
using System.Collections.Generic;

namespace LibFileParameters.Interfaces;

public interface IParametersWithExcludeSets : IParameters
{
    Dictionary<string, ExcludeSet> ExcludeSets { get; }
}