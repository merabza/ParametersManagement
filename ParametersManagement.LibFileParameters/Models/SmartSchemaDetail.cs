using SystemTools.SystemToolsShared;

namespace ParametersManagement.LibFileParameters.Models;

public sealed class SmartSchemaDetail : ItemData
{
    public EPeriodType PeriodType { get; set; }
    public int PreserveCount { get; set; }
}
