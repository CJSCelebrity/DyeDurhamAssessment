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
        
        var filePath = string.Empty;
        var results = fileProcessingService.ProcessFile(filePath);
        
        //TODO: implement file save logic here for sorted results. Move logic to the fileProcessingService
        
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Termination name sorting");
        return Task.CompletedTask;
    }
}