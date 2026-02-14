using System.Collections.Generic;

namespace ParametersManagement.LibFileParameters.Models;

public sealed class SmartSchemas
{
    private readonly Dictionary<string, SmartSchema> _smartSchemas;

    //public static SmartSchemas Create(IConfiguration configuration)
    //{
    //    IConfigurationSection smartSchemasSettings = configuration.GetSection("SmartSchemas");
    //    Dictionary<string, SmartSchema> smartSchemas = smartSchemasSettings.Get<Dictionary<string, SmartSchema>>();
    //    return new SmartSchemas(smartSchemas);
    //}

    // ReSharper disable once ConvertToPrimaryConstructor
    public SmartSchemas(Dictionary<string, SmartSchema> smartSchemas)
    {
        _smartSchemas = smartSchemas;
    }

    public SmartSchema? GetSmartSchemaByKey(string? key)
    {
        //_logger.LogError($"File storage with name {key} not found");
        return string.IsNullOrWhiteSpace(key) || !_smartSchemas.TryGetValue(key, out SmartSchema? schema)
            ? null
            : schema;
    }
}
