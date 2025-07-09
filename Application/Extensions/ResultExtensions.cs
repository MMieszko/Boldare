using Application.Utils;
using CSharpFunctionalExtensions;

namespace Application.Extensions;

public static class ResultExtensions
{
    public static Task<Result<IReadOnlyCollection<T>, Error>> AsResult<T>(this IReadOnlyCollection<T> result)
        => Task.FromResult(Result.Success<IReadOnlyCollection<T>, Error>(result));
}