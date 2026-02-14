using System.Collections.Generic;
using ParametersManagement.LibFileParameters.Models;
using ParametersManagement.LibParameters;

namespace ParametersManagement.LibFileParameters.Interfaces;

public interface IParametersWithSmartSchemas : IParameters
{
#pragma warning disable IDE0040
    // ReSharper disable once ArrangeTypeMemberModifiers
    public Dictionary<string, SmartSchema> SmartSchemas { get; }
#pragma warning restore IDE0040
}
