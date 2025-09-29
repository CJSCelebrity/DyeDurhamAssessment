using DyeDurhamAssessment.Application.Factories;
using DyeDurhamAssessment.Application.Interfaces;
using DyeDurhamAssessment.Application.Services;
using Moq;

namespace DyeDurhamAssessment.Tests.Builders;

public class FileProcessingServiceTestBuilder
{
    public Mock<IFileReaderFactory> MockFactory { get; private set; }
    public Mock<IFileReader> MockReader { get; private set; }
    public string FilePath { get; private set; }
    private List<string> _fileContent;

    public FileProcessingServiceTestBuilder()
    {
        MockFactory = new Mock<IFileReaderFactory>();
        MockReader = new Mock<IFileReader>();
        FilePath = "default.txt";
        _fileContent = new List<string>();

        MockFactory.Setup(f => f.CreateReader(It.IsAny<string>()))
            .Returns(MockReader.Object);

        MockReader.Setup(r => r.ReadFile(It.IsAny<string>()))
            .Returns(() => _fileContent);
    }

    public FileProcessingServiceTestBuilder WithFilePath(string filePath)
    {
        FilePath = filePath;
        return this;
    }

    public FileProcessingServiceTestBuilder WithFileContent(List<string> content)
    {
        _fileContent = content;
        MockReader.Setup(r => r.ReadFile(It.IsAny<string>()))
            .Returns(_fileContent);
        return this;
    }

    public FileProcessingServiceTestBuilder WithFactoryThatThrows<TException>() where TException : Exception, new()
    {
        MockFactory.Setup(f => f.CreateReader(It.IsAny<string>()))
            .Throws<TException>();
        return this;
    }

    public FileProcessingServiceTestBuilder WithReaderThatThrows<TException>() where TException : Exception, new()
    {
        MockReader.Setup(r => r.ReadFile(It.IsAny<string>()))
            .Throws<TException>();
        return this;
    }

    public FileProcessingService Build()
    {
        return new FileProcessingService(MockFactory.Object);
    }
}