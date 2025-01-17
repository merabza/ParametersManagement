using LibFileParameters.Models;
using LibParameters;
using System.Collections.Generic;

namespace LibFileParameters.Interfaces;

public interface IParametersWithSmartSchemas : IParameters
{
    public Dictionary<string, SmartSchema> SmartSchemas { get; }
}