using System.Collections.Generic;
using LibFileParameters.Models;
using LibParameters;

namespace LibFileParameters.Interfaces;

public interface IParametersWithArchivers : IParameters
{
    Dictionary<string, ArchiverData> Archivers { get; }
}