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
                    await ExecuteAsync(cancellationToken);
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

    private Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Beginning name sorting");

        var filePath = GetFilePath();
        var results = fileProcessingService.ProcessFile(filePath);
        fileProcessingService.PrintFileContentToConsole(results);
        
        return Task.CompletedTask;
    }

    private string GetFilePath()
    {
        var projectRoot = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.Parent?.FullName;
        var assetsPath = Path.Combine(projectRoot, "Assets");
        return Path.Combine(assetsPath, "unsorted-names-list.txt");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Stopping application");
        return Task.CompletedTask;
    }
}