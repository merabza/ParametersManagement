using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ParametersManagement.LibParameters.Tests;

//Save: ფაილის ატომური ჩანაცვლება, წინა ვერსიების ბექაპები და ფაილის უცვლელი ფორმატი
[Collection(ConsoleCaptureCollection.Name)]
public sealed class ParametersManagerTests : IDisposable
{
    private const string FileName = "Parameters.json";
    private const string PreviousVersion = "previous version";

    private readonly StringWriter _consoleOutput = new(CultureInfo.InvariantCulture);
    private readonly string _filePath;
    private readonly string _folderPath;
    private readonly TextWriter _originalConsoleOutput;

    public ParametersManagerTests()
    {
        _folderPath = Directory.CreateTempSubdirectory("ParametersManagerTests_").FullName;
        _filePath = Path.Combine(_folderPath, FileName);
        _originalConsoleOutput = Console.Out;
        Console.SetOut(_consoleOutput);
    }

    public void Dispose()
    {
        Console.SetOut(_originalConsoleOutput);
        _consoleOutput.Dispose();
        //some tests leave read-only files, Directory.Delete does not remove them
        foreach (string filePath in Directory.EnumerateFiles(_folderPath))
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
        }

        Directory.Delete(_folderPath, true);
    }

    [Fact]
    public void Constructor_WhenOptionsAreGiven_UsesTheirFileNameAndParameters()
    {
        // Arrange
        var parameters = new TestParameters { Name = "FromOptions" };
        IOptions<MainParametersManagerOptions> options =
            Options.Create(new MainParametersManagerOptions { ParametersFileName = _filePath, Par = parameters });

        // Act
        var sut = new ParametersManager(options);

        // Assert
        Assert.Equal(_filePath, sut.ParametersFileName);
        Assert.Same(parameters, sut.Parameters);
    }

    [Fact]
    public async Task Save_WhenMessageIsGiven_PrintsIt()
    {
        // Arrange
        var parameters = new TestParameters { Name = "New" };
        var sut = new ParametersManager(_filePath, parameters);

        // Act
        await sut.Save(parameters, "Parameters were saved");

        // Assert
        Assert.Contains("Parameters were saved", _consoleOutput.ToString(), StringComparison.Ordinal);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task Save_WhenMessageIsEmpty_PrintsNothingAndSaves(string? message)
    {
        // Arrange
        var parameters = new TestParameters { Name = "New" };
        var sut = new ParametersManager(_filePath, parameters);

        // Act
        bool result = await sut.Save(parameters, message);

        // Assert
        Assert.True(result);
        Assert.Equal(string.Empty, _consoleOutput.ToString());
        Assert.True(File.Exists(_filePath));
    }

    //Windows keeps a file that another process holds open without delete sharing: the save itself succeeds
    [Fact]
    public async Task Save_WhenAnOldBackupCannotBeDeleted_WarnsAndKeepsIt()
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        // Arrange
        await File.WriteAllTextAsync(_filePath, PreviousVersion);
        List<string> oldBackupFileNames = await CreateOldBackupFiles(10);
        var parameters = new TestParameters { Name = "New" };
        var sut = new ParametersManager(_filePath, parameters);
        bool result;

        // Act
        await using (new FileStream(Path.Combine(_folderPath, oldBackupFileNames[0]), FileMode.Open, FileAccess.Read,
                         FileShare.Read))
        {
            result = await sut.Save(parameters, "Saved");
        }

        // Assert
        Assert.True(result);
        Assert.Contains(oldBackupFileNames[0], GetFolderFileNames());
        Assert.Contains($"Old backup file {Path.Combine(_folderPath, oldBackupFileNames[0])} was not deleted",
            _consoleOutput.ToString(), StringComparison.Ordinal);
    }

    //Windows does not delete a read-only file (UnauthorizedAccessException): the save itself succeeds
    [Fact]
    public async Task Save_WhenAnOldBackupIsReadOnly_WarnsAndKeepsIt()
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        // Arrange
        await File.WriteAllTextAsync(_filePath, PreviousVersion);
        List<string> oldBackupFileNames = await CreateOldBackupFiles(10);
        string readOnlyBackupFilePath = Path.Combine(_folderPath, oldBackupFileNames[0]);
        File.SetAttributes(readOnlyBackupFilePath, FileAttributes.ReadOnly);
        var parameters = new TestParameters { Name = "New" };
        var sut = new ParametersManager(_filePath, parameters);

        // Act
        bool result = await sut.Save(parameters, "Saved");

        // Assert
        Assert.True(result);
        Assert.True(File.Exists(readOnlyBackupFilePath));
        Assert.Contains($"Old backup file {readOnlyBackupFilePath} was not deleted", _consoleOutput.ToString(),
            StringComparison.Ordinal);
    }

    //only the exact backup name counts: a name whose date part parses but which is longer is a foreign file
    [Fact]
    public async Task Save_WhenAForeignFileLooksLikeAnOldBackup_KeepsIt()
    {
        // Arrange
        await File.WriteAllTextAsync(_filePath, PreviousVersion);
        await CreateOldBackupFiles(10, 2);
        const string foreignFileName = $"{FileName}.20000101-120000-000x.bak";
        await File.WriteAllTextAsync(Path.Combine(_folderPath, foreignFileName), foreignFileName);
        var parameters = new TestParameters { Name = "New" };
        var sut = new ParametersManager(_filePath, parameters);

        // Act
        await sut.Save(parameters, "Saved");

        // Assert
        List<string> folderFileNames = GetFolderFileNames();
        Assert.Contains(foreignFileName, folderFileNames);
        Assert.Equal(10, folderFileNames.Count(IsCreatedBackupFileName));
    }

    //an empty "save as" path is no path: the file is saved where it was
    [Fact]
    public async Task Save_WhenSaveAsFilePathIsEmpty_SavesToTheCurrentFile()
    {
        // Arrange
        var parameters = new TestParameters { Name = "New" };
        var sut = new ParametersManager(_filePath, parameters);

        // Act
        bool result = await sut.Save(parameters, "Saved", string.Empty);

        // Assert
        Assert.True(result);
        Assert.Equal(_filePath, sut.ParametersFileName);
        Assert.Equal(JsonConvert.SerializeObject(parameters, Formatting.Indented),
            await File.ReadAllTextAsync(_filePath));
    }

    //like File.WriteAllText: text that is not valid UTF-16 fails the save instead of being written changed
    [Fact]
    public async Task Save_WhenTextHasALoneSurrogate_ThrowsAndKeepsTheFile()
    {
        // Arrange
        await File.WriteAllTextAsync(_filePath, PreviousVersion);
        var parameters = new TestParameters { Name = "Broken \uD800 text" };
        var sut = new ParametersManager(_filePath, parameters);

        // Act
        await Assert.ThrowsAsync<EncoderFallbackException>(async () => await sut.Save(parameters, "Saved"));

        // Assert
        Assert.Equal(PreviousVersion, await File.ReadAllTextAsync(_filePath));
        Assert.Equal([FileName], GetFolderFileNames());
    }

    //ფორმატი იგივე რჩება: Newtonsoft, Formatting.Indented, UTF-8 BOM-ის გარეშე
    [Fact]
    public async Task Save_WhenFileDoesNotExist_WritesIndentedJsonWithoutBomAndWithoutBackup()
    {
        // Arrange
        var parameters = new TestParameters { Name = "ქართული სახელი" };
        var sut = new ParametersManager(_filePath, parameters);

        // Act
        bool result = await sut.Save(parameters, "Saved");

        // Assert
        Assert.True(result);
        Assert.Equal(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(parameters, Formatting.Indented)),
            await File.ReadAllBytesAsync(_filePath));
        Assert.Equal([FileName], GetFolderFileNames());
    }

    [Fact]
    public async Task Save_WhenFileExists_ReplacesItAndKeepsThePreviousVersionAsBackup()
    {
        // Arrange
        await File.WriteAllTextAsync(_filePath, PreviousVersion);
        var parameters = new TestParameters { Name = "New" };
        var sut = new ParametersManager(_filePath, parameters);

        // Act
        bool result = await sut.Save(parameters, "Saved");

        // Assert
        Assert.True(result);
        Assert.Equal(JsonConvert.SerializeObject(parameters, Formatting.Indented),
            await File.ReadAllTextAsync(_filePath));
        string backupFileName = Assert.Single(GetFolderFileNames(), x => x != FileName);
        Assert.True(IsCreatedBackupFileName(backupFileName), backupFileName);
        Assert.Equal(PreviousVersion, await File.ReadAllTextAsync(Path.Combine(_folderPath, backupFileName)));
    }

    //ერთი ოპერაცია ფაილს რამდენჯერმე ინახავს: უცვლელი შიგთავსი არც ფაილს ეხება და არც ბექაპს ქმნის
    [Fact]
    public async Task Save_WhenContentIsUnchanged_DoesNotRewriteTheFileOrCreateABackup()
    {
        // Arrange
        var parameters = new TestParameters { Name = "Same" };
        await File.WriteAllTextAsync(_filePath, JsonConvert.SerializeObject(parameters, Formatting.Indented));
        var lastWriteTime = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        File.SetLastWriteTimeUtc(_filePath, lastWriteTime);
        var sut = new ParametersManager(_filePath, parameters);

        // Act
        bool result = await sut.Save(parameters, "Saved");

        // Assert
        Assert.True(result);
        Assert.Equal(lastWriteTime, File.GetLastWriteTimeUtc(_filePath));
        Assert.Equal([FileName], GetFolderFileNames());
    }

    //ბოლო 10 ბექაპი რჩება. ხელით გაკეთებულ ასლებსა და სხვა ფაილის ბექაპებს წაშლა არ ეხება
    [Fact]
    public async Task Save_WhenTenBackupsExist_DeletesOnlyTheOldestCreatedBackup()
    {
        // Arrange
        await File.WriteAllTextAsync(_filePath, PreviousVersion);
        List<string> oldBackupFileNames =
        [
            .. Enumerable.Range(1, 10).Select(day =>
                $"{FileName}.200001{day.ToString("00", CultureInfo.InvariantCulture)}-120000-000.bak")
        ];
        string[] otherFileNames =
        [
            $"{FileName}.bak", $"{FileName}.bak-20260819", $"{FileName}.20260919-142615.bak",
            "Other.json.20000101-120000-000.bak"
        ];
        foreach (string fileName in oldBackupFileNames.Concat(otherFileNames))
        {
            await File.WriteAllTextAsync(Path.Combine(_folderPath, fileName), fileName);
        }

        var parameters = new TestParameters { Name = "New" };
        var sut = new ParametersManager(_filePath, parameters);

        // Act
        await sut.Save(parameters, "Saved");

        // Assert
        List<string> folderFileNames = GetFolderFileNames();
        Assert.Equal(10, folderFileNames.Count(IsCreatedBackupFileName));
        Assert.DoesNotContain(oldBackupFileNames[0], folderFileNames);
        Assert.All(oldBackupFileNames.Skip(1), x => Assert.Contains(x, folderFileNames));
        Assert.All(otherFileNames, x => Assert.Contains(x, folderFileNames));
    }

    [Fact]
    public async Task Save_WhenCheckBeforeSaveFails_ReturnsFalseAndKeepsTheFile()
    {
        // Arrange
        await File.WriteAllTextAsync(_filePath, PreviousVersion);
        var parameters = new TestParameters { Name = "Invalid", IsValid = false };
        var sut = new ParametersManager(_filePath, parameters);

        // Act
        bool result = await sut.Save(parameters, "Saved");

        // Assert
        Assert.False(result);
        Assert.Equal(PreviousVersion, await File.ReadAllTextAsync(_filePath));
        Assert.Equal([FileName], GetFolderFileNames());
        string console = _consoleOutput.ToString();
        Assert.Contains("Something wrong with data for save", console, StringComparison.Ordinal);
        Assert.DoesNotContain("Saved", console, StringComparison.Ordinal);
    }

    [Fact]
    public async Task Save_WhenFileNameIsNotKnown_ReturnsFalse()
    {
        // Arrange
        var currentParameters = new TestParameters { Name = "Current" };
        var sut = new ParametersManager(null, currentParameters);

        // Act
        bool result = await sut.Save(new TestParameters { Name = "New" }, "Saved");

        // Assert
        Assert.False(result);
        Assert.Empty(GetFolderFileNames());
        Assert.Same(currentParameters, sut.Parameters);
        Assert.Contains("filePathForSave is empty, cannot save", _consoleOutput.ToString(), StringComparison.Ordinal);
    }

    [Fact]
    public async Task Save_WhenSaveAsFilePathIsGiven_WritesThereAndRemembersIt()
    {
        // Arrange
        string saveAsFilePath = Path.Combine(_folderPath, "SavedAs.json");
        var parameters = new TestParameters { Name = "New" };
        var sut = new ParametersManager(_filePath, parameters);

        // Act
        bool result = await sut.Save(parameters, "Saved", saveAsFilePath);

        // Assert
        Assert.True(result);
        Assert.Equal(saveAsFilePath, sut.ParametersFileName);
        Assert.Equal(["SavedAs.json"], GetFolderFileNames());
    }

    [Fact]
    public async Task Save_WhenCalled_RemembersTheSavedParameters()
    {
        // Arrange
        var sut = new ParametersManager(_filePath, new TestParameters { Name = "Old" });
        var newParameters = new TestParameters { Name = "New" };

        // Act
        await sut.Save(newParameters, "Saved");

        // Assert
        Assert.Same(newParameters, sut.Parameters);
    }

    //ფაილის ჩანაცვლება ვერ ხერხდება (Windows-ზე ფაილი სხვა პროცესს აქვს გახსნილი წაშლის უფლების გარეშე):
    //ძველი შიგთავსი ხელუხლებელი რჩება და დროებითი ფაილი არ რჩება
    [Fact]
    public async Task Save_WhenFileCannotBeReplaced_KeepsItUnchangedAndLeavesNoTemporaryFile()
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        // Arrange
        await File.WriteAllTextAsync(_filePath, PreviousVersion);
        var parameters = new TestParameters { Name = "New" };
        var sut = new ParametersManager(_filePath, parameters);

        // Act
        await using (new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            await Assert.ThrowsAnyAsync<SystemException>(async () => await sut.Save(parameters, "Saved"));
        }

        // Assert
        Assert.Equal(PreviousVersion, await File.ReadAllTextAsync(_filePath));
        Assert.DoesNotContain(GetFolderFileNames(), x => x.EndsWith(".tmp", StringComparison.OrdinalIgnoreCase));
    }

    //backups made by Save on 2000-01-{firstDay}, 2000-01-{firstDay + 1}, ... at noon, the oldest first
    private async Task<List<string>> CreateOldBackupFiles(int count, int firstDay = 1)
    {
        List<string> backupFileNames =
        [
            .. Enumerable.Range(firstDay, count).Select(day =>
                $"{FileName}.200001{day.ToString("00", CultureInfo.InvariantCulture)}-120000-000.bak")
        ];
        foreach (string backupFileName in backupFileNames)
        {
            await File.WriteAllTextAsync(Path.Combine(_folderPath, backupFileName), backupFileName);
        }

        return backupFileNames;
    }

    private List<string> GetFolderFileNames()
    {
        return [.. Directory.GetFiles(_folderPath).Select(x => Path.GetFileName(x)).Order(StringComparer.Ordinal)];
    }

    //Save-ის შექმნილი ბექაპის სახელი: Parameters.json.yyyyMMdd-HHmmss-fff.bak
    private static bool IsCreatedBackupFileName(string fileName)
    {
        const string prefix = FileName + ".";
        const string suffix = ".bak";
        return fileName.Length > prefix.Length + suffix.Length &&
               fileName.StartsWith(prefix, StringComparison.Ordinal) &&
               fileName.EndsWith(suffix, StringComparison.Ordinal) &&
               DateTime.TryParseExact(fileName[prefix.Length..^suffix.Length], "yyyyMMdd-HHmmss-fff",
                   CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
    }

    private sealed class TestParameters : IParameters
    {
        public string? Name { get; init; }
        public List<string> Items { get; init; } = ["First", "Second"];

        [JsonIgnore]
        public bool IsValid { get; init; } = true;

        public bool CheckBeforeSave()
        {
            return IsValid;
        }
    }
}
