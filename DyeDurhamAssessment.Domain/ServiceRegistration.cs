using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DyeDurhamAssessment.Domain;

public static class ServiceRegistration
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }
}