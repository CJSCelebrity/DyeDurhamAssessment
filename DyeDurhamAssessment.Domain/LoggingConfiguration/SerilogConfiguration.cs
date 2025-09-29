using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;

namespace DyeDurhamAssessment.Domain.LoggingConfiguration;

public static class SerilogConfiguration
{
    public static LoggerConfiguration CreateLoggerConfiguration(IConfiguration configuration)
    {
        var applicationName = "CalebSewcharran_DyeAndDurhamCodingAssessment_Console_Application";
        var logFilePath = configuration.GetValue<string>("Logging:FilePath", "logs/app-.txt");
        
        return new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithEnvironmentUserName()
            .Enrich.WithProcessId()
            .Enrich.WithProcessName()
            .Enrich.WithThreadId()
            .Enrich.WithProperty("ApplicationName", applicationName)
            .WriteTo.Console(outputTemplate: 
                "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} " +
                "{MachineName} {ProcessId} {ThreadId} {NewLine}{Exception}")
            .WriteTo.File(
                path: logFilePath,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 30,
                outputTemplate: 
                "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] " +
                "{Message:lj} {Properties} {NewLine}{Exception}");
    }
}