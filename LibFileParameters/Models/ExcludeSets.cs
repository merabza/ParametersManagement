using System.Collections.Generic;

namespace LibFileParameters.Models;

public sealed class ExcludeSets
{
    private readonly Dictionary<string, ExcludeSet> _excludeSet;

    // ReSharper disable once ConvertToPrimaryConstructor
    public ExcludeSets(Dictionary<string, ExcludeSet> excludeSet)
    {
        _excludeSet = excludeSet;
    }

    public ExcludeSet? GetExcludeSetByKey(string key)
    {
        return _excludeSet.ContainsKey(key) ? _excludeSet[key] : null;
    }
}