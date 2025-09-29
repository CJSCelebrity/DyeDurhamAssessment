namespace DyeDurhamAssessment.Application.Interfaces;

public interface IFileProcessingService
{
    List<List<string>> ProcessFile(string filePath);
}