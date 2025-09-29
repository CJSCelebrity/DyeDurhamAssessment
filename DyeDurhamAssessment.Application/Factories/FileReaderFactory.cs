using DyeDurhamAssessment.Application.Interfaces;
using DyeDurhamAssessment.Application.Services;

namespace DyeDurhamAssessment.Application.Factories;

public class FileReaderFactory
{
    private readonly List<IFileReader> _readers;

    public FileReaderFactory()
    {
        _readers = new List<IFileReader>
        {
            new TextFileReader()
        };
    }
    
    public IFileReader CreateReader(string filePath)
    {
        var extension = Path.GetExtension(filePath);
        var reader = _readers.FirstOrDefault(x => x.CanHandle(extension));

        return reader ?? throw new NotSupportedException($"No reader available for file extension: {extension}");
    }

    public void RegisterReader(IFileReader reader)
    {
        _readers.Add(reader);
    }
}