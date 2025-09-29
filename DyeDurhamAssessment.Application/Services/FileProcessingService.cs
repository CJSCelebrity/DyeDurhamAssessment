using DyeDurhamAssessment.Application.Factories;
using DyeDurhamAssessment.Application.Interfaces;

namespace DyeDurhamAssessment.Application.Services;

public class FileProcessingService(FileReaderFactory factory) : IFileProcessingService
{
    public List<List<string>> ProcessFile(string filePath)
    {
        var reader = factory.CreateReader(filePath);
        return reader.ReadFile(filePath);
    }
}