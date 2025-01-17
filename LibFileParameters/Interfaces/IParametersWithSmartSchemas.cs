using System.Collections.Generic;
using LibFileParameters.Models;
using LibParameters;

namespace LibFileParameters.Interfaces;

public interface IParametersWithSmartSchemas : IParameters
{
    public Dictionary<string, SmartSchema> SmartSchemas { get; }
}