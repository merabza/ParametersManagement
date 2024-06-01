using LibParameters;

namespace LibApiClientParameters;

public sealed class ApiClientSettings : ItemData
{
    public string? Server { get; set; }
    public string? ApiKey { get; set; }
    //public bool WithMessaging { get; set; }
}