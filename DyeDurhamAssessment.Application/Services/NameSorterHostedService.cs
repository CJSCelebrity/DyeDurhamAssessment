using DyeDurhamAssessment.Application.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DyeDurhamAssessment.Application.Services;

public class NameSorterHostedService(
    IFileProcessingService fileProcessingService,
    ILogger<NameSorterHostedService> logger,
    IHostApplicationLifetime applicationLifetime) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        applicationLifetime.ApplicationStarted.Register(() =>
        {
            Task.Run(async () =>
            {
                try
                {
                    await ExecuteAsync();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while processing the file");
                    Console.WriteLine(ex);
                }
                finally
                {
                    applicationLifetime.StopApplication();
                }
            }, cancellationToken);
        });
        return Task.CompletedTask;
    }

    private async Task ExecuteAsync()
    {
        logger.LogInformation("Beginning name sorting");

        var assetsFilePath = GetAssetsFilePath();
        var outputFilePath = GetOutputFilePath();
        
        var results = fileProcessingService.ProcessFile(assetsFilePath);
        
        fileProcessingService.PrintFileContentToConsole(results);
        await fileProcessingService.SaveFileContentAsync(outputFilePath, results);
    }

    private string GetAssetsFilePath()
    {
        var projectRoot = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.Parent?.FullName;
        var assetsPath = Path.Combine(projectRoot, "Assets");
        return Path.Combine(assetsPath, "unsorted-names-list.txt");
    }
    
    private string GetOutputFilePath()
    {
        var projectRoot = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.Parent?.FullName;
        return Path.Combine(projectRoot, "Output");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Stopping application");
        return Task.CompletedTask;
    }
}