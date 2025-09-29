using DyeDurhamAssessment.Application.Factories;
using DyeDurhamAssessment.Application.Interfaces;

namespace DyeDurhamAssessment.Application.Services;

public class FileProcessingService(FileReaderFactory factory) : IFileProcessingService
{
    public List<string> ProcessFile(string filePath)
    {
        var reader = factory.CreateReader(filePath);
        return reader.ReadFile(filePath);
    }

    public void PrintFileContentToConsole(List<string> results)
    {
        foreach (var item in results)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(item);
        }
        Console.ResetColor();
    }

    public async Task SaveFileContentAsync(string filePath, List<string> results)
    {
        await using var outputFile = new StreamWriter(Path.Combine(filePath, "sorted-names-list.txt"));
        foreach (var item in results)
        {
            await outputFile.WriteAsync($"{item}\n");
        }
    }
}