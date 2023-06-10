namespace LibParameters;

public interface IParametersManager
{
    IParameters Parameters { get; set; }
    void Save(IParameters parameters, string message, bool pauseAfterMessage = true, string? saveAsFilePath = null);
}