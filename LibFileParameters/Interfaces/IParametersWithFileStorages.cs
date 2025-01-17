using LibFileParameters.Models;
using LibParameters;
using System.Collections.Generic;

namespace LibFileParameters.Interfaces;

public interface IParametersWithFileStorages : IParameters
{
    Dictionary<string, FileStorageData> FileStorages { get; }
}