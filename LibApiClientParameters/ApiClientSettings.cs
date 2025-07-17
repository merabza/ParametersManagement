using LibParameters;

namespace LibApiClientParameters;

public sealed class ApiClientSettings : ItemData
{
    public string? Server { get; set; }
    public string? ApiKey { get; set; }

    public override string GetItemKey()
    {
        return $"{Server} {ApiKey}";
    }
}