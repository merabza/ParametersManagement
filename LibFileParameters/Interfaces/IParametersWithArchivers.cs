using LibFileParameters.Models;
using LibParameters;
using System.Collections.Generic;

namespace LibFileParameters.Interfaces;

public interface IParametersWithArchivers : IParameters
{
    Dictionary<string, ArchiverData> Archivers { get; }
}