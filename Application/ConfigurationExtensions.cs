using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<AssemblyInfo>());

        return services;
    }
}