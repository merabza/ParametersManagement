using System.Collections.Generic;
using LibFileParameters.Models;
using LibParameters;

namespace LibFileParameters.Interfaces;

public interface IParametersWithFileStorages : IParameters
{
    Dictionary<string, FileStorageData> FileStorages { get; }
}