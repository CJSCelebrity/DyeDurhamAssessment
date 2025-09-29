using DyeDurhamAssessment.Tests.Builders;
using Moq;
using Shouldly;

namespace DyeDurhamAssessment.Tests.Services;

public class FileProcessingServiceTests
{
    [Fact]
    public void ProcessFile_ShouldCreateReaderFromFactory()
    {
        // Arrange
        var builder = new FileProcessingServiceTestBuilder()
            .WithFilePath("test.txt")
            .WithFileContent(new List<string> { "Line 1", "Line 2" });
        var sut = builder.Build();

        // Act
        sut.ProcessFile(builder.FilePath);

        // Assert
        builder.MockFactory.Verify(f => f.CreateReader(builder.FilePath), Times.Once);
    }

    [Fact]
    public void ProcessFile_ShouldCallReadFileOnReader()
    {
        // Arrange
        var builder = new FileProcessingServiceTestBuilder()
            .WithFilePath("test.txt")
            .WithFileContent(new List<string> { "Line 1", "Line 2" });
        var sut = builder.Build();

        // Act
        sut.ProcessFile(builder.FilePath);

        // Assert
        builder.MockReader.Verify(r => r.ReadFile(builder.FilePath), Times.Once);
    }

    [Fact]
    public void ProcessFile_ShouldReturnContentFromReader()
    {
        // Arrange
        var expectedContent = new List<string> { "Line 1", "Line 2", "Line 3" };
        var builder = new FileProcessingServiceTestBuilder()
            .WithFilePath("test.txt")
            .WithFileContent(expectedContent);
        var sut = builder.Build();

        // Act
        var result = sut.ProcessFile(builder.FilePath);

        // Assert
        result.ShouldBe(expectedContent);
    }

    [Fact]
    public void ProcessFile_ShouldReturnEmptyListWhenFileIsEmpty()
    {
        // Arrange
        var builder = new FileProcessingServiceTestBuilder()
            .WithFilePath("empty.txt")
            .WithFileContent(new List<string>());
        var sut = builder.Build();

        // Act
        var result = sut.ProcessFile(builder.FilePath);

        // Assert
        result.ShouldBeEmpty();
    }

    [Fact]
    public void PrintFileContentToConsole_ShouldNotThrowException()
    {
        // Arrange
        var builder = new FileProcessingServiceTestBuilder();
        var sut = builder.Build();
        var testContent = new List<string> { "Test Line 1", "Test Line 2" };

        // Act & Assert
        Should.NotThrow(() => sut.PrintFileContentToConsole(testContent));
    }

    [Fact]
    public void PrintFileContentToConsole_ShouldHandleEmptyList()
    {
        // Arrange
        var builder = new FileProcessingServiceTestBuilder();
        var sut = builder.Build();
        var emptyContent = new List<string>();

        // Act & Assert
        Should.NotThrow(() => sut.PrintFileContentToConsole(emptyContent));
    }

    [Fact]
    public async Task SaveFileContentAsync_ShouldCreateFileWithCorrectPath()
    {
        // Arrange
        var builder = new FileProcessingServiceTestBuilder();
        var sut = builder.Build();
        var testDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(testDirectory);
        var testContent = new List<string> { "Line 1", "Line 2" };

        try
        {
            // Act
            await sut.SaveFileContentAsync(testDirectory, testContent);

            // Assert
            var expectedFilePath = Path.Combine(testDirectory, "sorted-names-list.txt");
            File.Exists(expectedFilePath).ShouldBeTrue();
        }
        finally
        {
            // Cleanup
            Directory.Delete(testDirectory, true);
        }
    }

    [Fact]
    public async Task SaveFileContentAsync_ShouldWriteAllContentToFile()
    {
        // Arrange
        var builder = new FileProcessingServiceTestBuilder();
        var sut = builder.Build();
        var testDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(testDirectory);
        var testContent = new List<string> { "Alice", "Bob", "Charlie" };

        try
        {
            // Act
            await sut.SaveFileContentAsync(testDirectory, testContent);

            // Assert
            var expectedFilePath = Path.Combine(testDirectory, "sorted-names-list.txt");
            var fileContent = await File.ReadAllTextAsync(expectedFilePath);
            
            fileContent.ShouldContain("Alice\n");
            fileContent.ShouldContain("Bob\n");
            fileContent.ShouldContain("Charlie\n");
        }
        finally
        {
            // Cleanup
            Directory.Delete(testDirectory, true);
        }
    }

    [Fact]
    public async Task SaveFileContentAsync_ShouldHandleEmptyList()
    {
        // Arrange
        var builder = new FileProcessingServiceTestBuilder();
        var sut = builder.Build();
        var testDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(testDirectory);
        var emptyContent = new List<string>();

        try
        {
            // Act
            await sut.SaveFileContentAsync(testDirectory, emptyContent);

            // Assert
            var expectedFilePath = Path.Combine(testDirectory, "sorted-names-list.txt");
            var fileContent = await File.ReadAllTextAsync(expectedFilePath);
            fileContent.ShouldBeEmpty();
        }
        finally
        {
            // Cleanup
            Directory.Delete(testDirectory, true);
        }
    }

    [Fact]
    public async Task SaveFileContentAsync_ShouldWriteContentInCorrectOrder()
    {
        // Arrange
        var builder = new FileProcessingServiceTestBuilder();
        var sut = builder.Build();
        var testDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(testDirectory);
        var testContent = new List<string> { "First", "Second", "Third" };

        try
        {
            // Act
            await sut.SaveFileContentAsync(testDirectory, testContent);

            // Assert
            var expectedFilePath = Path.Combine(testDirectory, "sorted-names-list.txt");
            var lines = await File.ReadAllLinesAsync(expectedFilePath);
            
            lines.Length.ShouldBe(3);
            lines[0].ShouldBe("First");
            lines[1].ShouldBe("Second");
            lines[2].ShouldBe("Third");
        }
        finally
        {
            // Cleanup
            Directory.Delete(testDirectory, true);
        }
    }
}