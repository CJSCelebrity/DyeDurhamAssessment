using DyeDurhamAssessment.Application.Services;

namespace DyeDurhamAssessment.Tests.Builders;

public class TextFileReaderServiceTestBuilder
{
    private string[] _fileContent = [];
    private string _fileExtension = ".txt";

    public TextFileReaderServiceTestBuilder WithFileContent(string[] content)
    {
        _fileContent = content;
        return this;
    }

    public TextFileReaderServiceTestBuilder WithFileExtension(string extension)
    {
        _fileExtension = extension;
        return this;
    }

    public string CreateTestFile()
    {
        var testFilePath = Path.Combine(Path.GetTempPath(), $"test-{Guid.NewGuid()}{_fileExtension}");
        File.WriteAllLines(testFilePath, _fileContent);
        return testFilePath;
    }

    public TextFileReaderService Build()
    {
        return new TextFileReaderService();
    }
}