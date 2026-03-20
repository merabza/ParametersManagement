namespace ParametersManagement.LibParameters;

public interface IToolCommandFactoryStrategy
{
    string ToolCommandName { get; }
    IToolCommand CreateToolCommand(IParametersManager parametersManager, string projectName);
}
