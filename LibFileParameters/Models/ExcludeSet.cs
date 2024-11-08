using System.Collections.Generic;
using System.Linq;
using LibParameters;
using SystemToolsShared;

namespace LibFileParameters.Models;

public sealed class ExcludeSet : ItemData
{
    //გამონაკლისების სია
    public List<string> FolderFileMasks { get; set; } = [];

    public bool NeedExclude(string name)
    {
        return !string.IsNullOrWhiteSpace(name) && FolderFileMasks is { Count: > 0 } &&
               FolderFileMasks.Any(name.FitsMask);
    }
}