using Api.Caching;
using Application.Contracts;

namespace Api.Extensions;

internal static class CacheExtensions
{
    public static IHostApplicationBuilder AddCache(this IHostApplicationBuilder builder)
    {
        builder.Services.Configure<CacheOptions>(builder.Configuration.GetSection(CacheOptions.Key));
        builder.Services.AddMemoryCache();
        builder.Services.AddSingleton<ICache, MemoryCache>();
        
        return builder;
    }
}