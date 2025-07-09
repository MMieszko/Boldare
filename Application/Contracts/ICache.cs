using Application.Utils;
using CSharpFunctionalExtensions;

namespace Application.Contracts;

public interface ICache
{
    Task<Result<T, Error>> GetOrSet<T>(string key, Func<Task<Result<T, Error>>> factory, TimeSpan? expiration = null);
}