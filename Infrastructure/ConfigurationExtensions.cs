using Application.Contracts;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public enum PersistenceOptions
{
    InMemory,
    Sqlite
}

public static class ConfigurationExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, PersistenceOptions options) => options switch
    {
        PersistenceOptions.InMemory => AddInMemory(services),
        PersistenceOptions.Sqlite => AddSqlite(services),
        _ => throw new ArgumentOutOfRangeException(nameof(options), options, "Invalid persistence option specified.")
    };

    public static void MigrateDatabase(this IServiceScopeFactory scopeFactory)
    {
        using var scope = scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BreweryContext>();
        context.Database.Migrate();
    }

    private static IServiceCollection AddInMemory(this IServiceCollection services)
    {
        services.AddSingleton<IBreweryRepository, InMemoryBreweryRepository>();

        return services;
    }

    private static IServiceCollection AddSqlite(this IServiceCollection services)
    {
        services.AddDbContext<BreweryContext>(options => options.UseSqlite($"Data Source={Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}/brewery.db"));

        services.AddScoped<IBreweryRepository, SqliteBreweryRepository>();

        return services;
    }
}