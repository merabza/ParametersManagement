using System.Collections.Generic;

namespace ParametersManagement.LibFileParameters.Models;

public sealed class SmartSchemas
{
    private readonly Dictionary<string, SmartSchema> _smartSchemas;

    // ReSharper disable once ConvertToPrimaryConstructor
    public SmartSchemas(Dictionary<string, SmartSchema> smartSchemas)
    {
        _smartSchemas = smartSchemas;
    }

    public SmartSchema? GetSmartSchemaByKey(string? key)
    {
        return string.IsNullOrWhiteSpace(key) || !_smartSchemas.TryGetValue(key, out SmartSchema? schema)
            ? null
            : schema;
    }
}
