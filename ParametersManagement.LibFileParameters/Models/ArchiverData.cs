using SystemTools.SystemToolsShared;

namespace ParametersManagement.LibFileParameters.Models;

public sealed class ArchiverData : ItemData
{
    public EArchiveType Type { get; set; }
    public string? CompressProgramPatch { get; set; }
    public string? DecompressProgramPatch { get; set; }
    public string? FileExtension { get; set; }

    public override string GetItemKey()
    {
        return $"{CompressProgramPatch} {DecompressProgramPatch} {FileExtension}";
    }
}
