namespace DyeDurhamAssessment.Application.Interfaces;

public interface IFileProcessingService
{
    List<string> ProcessFile(string filePath);
    void PrintFileContentToConsole(List<string> results);
    Task SaveFileContentAsync(string filePath, List<string> results);
}