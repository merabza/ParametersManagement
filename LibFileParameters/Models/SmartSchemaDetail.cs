using LibParameters;
using SystemToolsShared;

namespace LibFileParameters.Models;

public sealed class SmartSchemaDetail : ItemData
{
    public EPeriodType PeriodType { get; set; }
    public int PreserveCount { get; set; }
}