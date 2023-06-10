namespace LibParameters;

public interface IToolCommand
{
    IParameters Par { get; }

    //ParametersEditor GetParametersEditor();
    bool Run();
}