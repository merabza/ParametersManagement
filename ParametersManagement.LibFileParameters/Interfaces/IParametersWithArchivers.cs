using System.Collections.Generic;
using ParametersManagement.LibFileParameters.Models;
using ParametersManagement.LibParameters;

namespace ParametersManagement.LibFileParameters.Interfaces;

public interface IParametersWithArchivers : IParameters
{
    Dictionary<string, ArchiverData> Archivers { get; }
}
