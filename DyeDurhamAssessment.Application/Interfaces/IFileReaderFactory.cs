namespace DyeDurhamAssessment.Application.Interfaces;

public interface IFileReaderFactory
{
    IFileReader CreateReader(string filePath);
    void RegisterReader(IFileReader reader);
}