/*
 * The Goal: Name Sorter
Build a name sorter. Given a set of names, order that set first by last name, then by any given names the person may have. A name must have at least 1 given name
and may have up to 3 given names.
 */

using DyeDurhamAssessment.Domain;
using DyeDurhamAssessment.Domain.LoggingConfiguration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

class Program
{
    static async Task Main(string[] args)
    {
        var configuration = BuildConfiguration(args);
        
        ConfigureLogging(configuration);
        
        try
        {
            Log.Information("Application starting up");
            
            var host = CreateHostBuilder(args, configuration).Build();
            
            await host.RunAsync();
            
            Console.WriteLine("Hello, World!");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private static IConfiguration BuildConfiguration(string[] args)
    {
        return new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production"}.json", optional: true)
            .AddEnvironmentVariables()
            .AddCommandLine(args)
            .Build();
    }
    
    private static void ConfigureLogging(IConfiguration configuration)
    {
        Log.Logger = SerilogConfiguration
            .CreateLoggerConfiguration(configuration)
            .CreateLogger();
    }
    
    static IHostBuilder CreateHostBuilder(string[] args, IConfiguration configuration) =>
        Host.CreateDefaultBuilder(args)
            .UseSerilog()
            .ConfigureServices((context, services) =>
            {
                services.AddDomainServices(configuration);
            });
}