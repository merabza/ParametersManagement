using LibParameters;

namespace LibDatabaseParameters;

public class DatabaseFoldersSet : ItemData
{
    public string? Backup { get; set; }
    public string? Data { get; set; }
    public string? DataLog { get; set; }
}