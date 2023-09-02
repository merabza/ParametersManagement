namespace LibParameters;

public interface IToolCommand
{
    IParameters Par { get; }
    string? Description { get; }
    bool Run();
}