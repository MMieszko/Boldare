using Api.Services;

namespace Api.Extensions;

public static class BreweryInitializationExtensions
{
    public static IServiceCollection AddBreweryInitializationService(this IServiceCollection services)
    {
        services.AddHttpClient(nameof(BreweryInitializationService), client => { client.BaseAddress = new Uri("https://api.openbrewerydb.org/v1/"); });
        services.AddHostedService<BreweryInitializationService>();

        return services;
    }
}