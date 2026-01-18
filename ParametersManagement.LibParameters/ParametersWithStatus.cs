namespace ParametersManagement.LibParameters;

public /*open*/ class ParametersWithStatus : IParameters
{
    public bool CheckBeforeSave()
    {
        return true;
    }

    public virtual string GetStatus()
    {
        return "(some parameters)";
    }
}