using DyeDurhamAssessment.Application.Factories;
using DyeDurhamAssessment.Application.Interfaces;
using DyeDurhamAssessment.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DyeDurhamAssessment.Application;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<FileReaderFactory>();
        services.AddTransient<IFileProcessingService, FileProcessingService>();
        services.AddHostedService<NameSorterHostedService>();
        
        return services;
    }
}