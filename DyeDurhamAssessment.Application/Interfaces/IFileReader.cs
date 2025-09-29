namespace DyeDurhamAssessment.Application.Interfaces;

public interface IFileReader
{
    List<string> ReadFile(string filePath); 
    bool CanHandle(string fileExtension); 
}