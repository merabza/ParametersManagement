﻿using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace LibApiClientParameters;

public sealed class ApiClients
{
    private readonly Dictionary<string, ApiClientSettings> _apiClientSettings;

    // ReSharper disable once ConvertToPrimaryConstructor
    public ApiClients(Dictionary<string, ApiClientSettings> apiClients)
    {
        _apiClientSettings = apiClients;
    }

    public static ApiClients Create(IConfiguration configuration, string sectionName)
    {
        var apiClientSettings = configuration.GetSection(sectionName);
        var apiClients = apiClientSettings.Get<Dictionary<string, ApiClientSettings>>();
        return new ApiClients(apiClients ?? []);
    }

    public ApiClientSettings? GetApiClientByKey(string key)
    {
        return _apiClientSettings.GetValueOrDefault(key);
    }
}