using DyeDurhamAssessment.Application.Interfaces;

namespace DyeDurhamAssessment.Application.Services;

public class TextFileReader : IFileReader
{
    public List<List<string>> ReadFile(string filePath)
    {
        var lines = File.ReadAllLines(filePath);
        return lines.Select(line => new List<string> { line }).ToList();
    }

    public bool CanHandle(string fileExtension)
    {
        return fileExtension.Equals(".txt", StringComparison.OrdinalIgnoreCase);
    }
}