using SystemTools.SystemToolsShared;

namespace ParametersManagement.LibApiClientParameters;

public sealed class ApiClientSettings : ItemData
{
    public string? Server { get; set; }
    public string? ApiKey { get; set; }

    //გასაღები მენიუს სათაურებში ჩანს, ამიტომ მასში ApiKey არ უნდა მოხვდეს
    public override string GetItemKey()
    {
        return Server ?? string.Empty;
    }
}
