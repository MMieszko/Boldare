using System.Collections.Concurrent;
using Application.Contracts;
using Application.Utils;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Api.Caching;

internal class MemoryCache(IMemoryCache cache, IOptions<CacheOptions> options) : ICache
{
    private static readonly ConcurrentDictionary<string, SemaphoreSlim> Locks = new();

    public async Task<Result<T, Error>> GetOrSet<T>(string key, Func<Task<Result<T, Error>>> factory, TimeSpan? expiration = null)
    {
        if (cache.TryGetValue(key, out T cached))
            return cached!;

        var semaphore = Locks.GetOrAdd(key, _ => new SemaphoreSlim(1, 1));

        await semaphore.WaitAsync();

        try
        {
            if (cache.TryGetValue(key, out cached))
                return cached!;

            var result = await factory();

            if (result.IsFailure)
                return result;

            cached = result.Value;

            cache.Set(key, cached, expiration ?? TimeSpan.FromSeconds(options.Value.DefaultDurationInSeconds));
            return cached;
        }
        finally
        {
            semaphore.Release();
        }
    }
}