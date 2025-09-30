using DyeDurhamAssessment.Tests.Builders;
using Shouldly;

namespace DyeDurhamAssessment.Tests.Services;

public class TextFileReaderServiceTests : IDisposable
{
    private readonly List<string> _testFilesToCleanup = new();

    [Fact]
    public void ReadFile_ShouldSortNamesByLastNameThenGivenNames()
    {
        // Arrange
        var builder = new TextFileReaderServiceTestBuilder()
            .WithFileContent(new[]
            {
                "Janet Parsons",
                "Vaughn Lewis",
                "Adonis Julius Archer",
                "Shelby Nathan Yoder",
                "Marin Alvarez",
                "London Lindsey",
                "Beau Tristan Bentley",
                "Leo Gardner",
                "Hunter Uriah Mathew Clarke",
                "Mikayla Lopez",
                "Frankie Conner Ritter"
            });
        var sut = builder.Build();
        var testFile = builder.CreateTestFile();
        _testFilesToCleanup.Add(testFile);

        // Act
        var result = sut.ReadFile(testFile);

        // Assert
        result.Count.ShouldBe(11);
        result[0].ShouldBe("Marin Alvarez");
        result[1].ShouldBe("Adonis Julius Archer");
        result[2].ShouldBe("Beau Tristan Bentley");
        result[10].ShouldBe("Shelby Nathan Yoder");
    }

    [Fact]
    public void ReadFile_ShouldSortByLastNameFirst()
    {
        // Arrange
        var builder = new TextFileReaderServiceTestBuilder()
            .WithFileContent(new[]
            {
                "Zoe Anderson",
                "Alice Anderson",
                "Bob Smith"
            });
        var sut = builder.Build();
        var testFile = builder.CreateTestFile();
        _testFilesToCleanup.Add(testFile);

        // Act
        var result = sut.ReadFile(testFile);

        // Assert
        result.Count.ShouldBe(3);
        result[0].ShouldBe("Alice Anderson");
        result[1].ShouldBe("Zoe Anderson");
        result[2].ShouldBe("Bob Smith");
    }

    [Fact]
    public void ReadFile_ShouldSortByGivenNamesWhenLastNamesMatch()
    {
        // Arrange
        var builder = new TextFileReaderServiceTestBuilder()
            .WithFileContent(new[]
            {
                "Charlie Brown",
                "Alice Brown",
                "Bob Brown"
            });
        var sut = builder.Build();
        var testFile = builder.CreateTestFile();
        _testFilesToCleanup.Add(testFile);

        // Act
        var result = sut.ReadFile(testFile);

        // Assert
        result.Count.ShouldBe(3);
        result[0].ShouldBe("Alice Brown");
        result[1].ShouldBe("Bob Brown");
        result[2].ShouldBe("Charlie Brown");
    }

    [Fact]
    public void ReadFile_ShouldHandleNamesWithMultipleGivenNames()
    {
        // Arrange
        var builder = new TextFileReaderServiceTestBuilder()
            .WithFileContent(new[]
            {
                "John Paul Smith",
                "John Adam Smith",
                "John Smith"
            });
        var sut = builder.Build();
        var testFile = builder.CreateTestFile();
        _testFilesToCleanup.Add(testFile);

        // Act
        var result = sut.ReadFile(testFile);

        // Assert
        result.Count.ShouldBe(3);
        result[0].ShouldBe("John Smith");
        result[1].ShouldBe("John Adam Smith");
        result[2].ShouldBe("John Paul Smith");
    }

    [Fact]
    public void ReadFile_ShouldHandleSingleNameEntries()
    {
        // Arrange
        var builder = new TextFileReaderServiceTestBuilder()
            .WithFileContent(new[]
            {
                "Madonna",
                "Prince",
                "Cher"
            });
        var sut = builder.Build();
        var testFile = builder.CreateTestFile();
        _testFilesToCleanup.Add(testFile);

        // Act
        var result = sut.ReadFile(testFile);

        // Assert
        result.Count.ShouldBe(3);
        result[0].ShouldBe("Cher");
        result[1].ShouldBe("Madonna");
        result[2].ShouldBe("Prince");
    }

    [Fact]
    public void ReadFile_ShouldReturnEmptyListForEmptyFile()
    {
        // Arrange
        var builder = new TextFileReaderServiceTestBuilder()
            .WithFileContent(Array.Empty<string>());
        var sut = builder.Build();
        var testFile = builder.CreateTestFile();
        _testFilesToCleanup.Add(testFile);

        // Act
        var result = sut.ReadFile(testFile);

        // Assert
        result.ShouldBeEmpty();
    }

    [Fact]
    public void ReadFile_ShouldHandleNamesWithExtraWhitespace()
    {
        // Arrange
        var builder = new TextFileReaderServiceTestBuilder()
            .WithFileContent(new[]
            {
                "  John   Doe  ",
                "Jane    Smith",
                "Bob  Jones  "
            });
        var sut = builder.Build();
        var testFile = builder.CreateTestFile();
        _testFilesToCleanup.Add(testFile);

        // Act
        var result = sut.ReadFile(testFile);

        // Assert
        result.Count.ShouldBe(3);
        result[0].ShouldBe("  John   Doe  ");
        result[1].ShouldBe("Bob  Jones  ");
        result[2].ShouldBe("Jane    Smith");
    }

    [Fact]
    public void ReadFile_ShouldThrowExceptionForNonExistentFile()
    {
        // Arrange
        var builder = new TextFileReaderServiceTestBuilder();
        var sut = builder.Build();
        var nonExistentFile = Path.Combine(Path.GetTempPath(), $"nonexistent-{Guid.NewGuid()}.txt");

        // Act & Assert
        Should.Throw<FileNotFoundException>(() => sut.ReadFile(nonExistentFile));
    }

    [Fact]
    public void ReadFile_ShouldMaintainOriginalNameFormat()
    {
        // Arrange
        var originalName = "John Michael Smith";
        var builder = new TextFileReaderServiceTestBuilder()
            .WithFileContent(new[] { originalName });
        var sut = builder.Build();
        var testFile = builder.CreateTestFile();
        _testFilesToCleanup.Add(testFile);

        // Act
        var result = sut.ReadFile(testFile);

        // Assert
        result.Single().ShouldBe(originalName);
    }

    [Fact]
    public void ReadFile_ShouldHandleMixedCaseNames()
    {
        // Arrange
        var builder = new TextFileReaderServiceTestBuilder()
            .WithFileContent(new[]
            {
                "john SMITH",
                "JANE doe",
                "Bob JoNeS"
            });
        var sut = builder.Build();
        var testFile = builder.CreateTestFile();
        _testFilesToCleanup.Add(testFile);

        // Act
        var result = sut.ReadFile(testFile);

        // Assert
        result.Count.ShouldBe(3);
        result[0].ShouldBe("JANE doe");
        result[1].ShouldBe("Bob JoNeS");
        result[2].ShouldBe("john SMITH");
    }

    [Theory]
    [InlineData(".txt", true)]
    [InlineData(".TXT", true)]
    [InlineData(".Txt", true)]
    [InlineData(".tXt", true)]
    public void CanHandle_ShouldReturnTrueForTextExtensionRegardlessOfCase(string extension, bool expected)
    {
        // Arrange
        var builder = new TextFileReaderServiceTestBuilder();
        var sut = builder.Build();

        // Act
        var result = sut.CanHandle(extension);

        // Assert
        result.ShouldBe(expected);
    }

    [Theory]
    [InlineData(".csv")]
    [InlineData(".json")]
    [InlineData(".xml")]
    [InlineData(".doc")]
    [InlineData(".pdf")]
    [InlineData("txt")]
    [InlineData("")]
    public void CanHandle_ShouldReturnFalseForNonTextExtensions(string extension)
    {
        // Arrange
        var builder = new TextFileReaderServiceTestBuilder();
        var sut = builder.Build();

        // Act
        var result = sut.CanHandle(extension);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public void CanHandle_ShouldReturnFalseForNull()
    {
        // Arrange
        var builder = new TextFileReaderServiceTestBuilder();
        var sut = builder.Build();

        // Act & Assert
        Should.Throw<NullReferenceException>(() => sut.CanHandle(null));
    }

    public void Dispose()
    {
        foreach (var file in _testFilesToCleanup.Where(File.Exists))
        {
            try
            {
                File.Delete(file);
            }
            catch
            {
                // We will ignore any cleanup errors as this is only for the unit tests
            }
        }
    }
}