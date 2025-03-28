﻿using LibDatabaseParameters;
using LibParameters;

namespace DbContextAnalyzer.Models;

public sealed class SeederParameters : IParameters
{
    public string? JsonFolderName { get; set; }
    public string? SecretDataFolder { get; set; }
    public string? LogFolder { get; set; }
    public EDatabaseProvider DataProvider { get; set; }
    public string? ConnectionStringSeed { get; set; }

    public bool CheckBeforeSave()
    {
        return true;
    }
}