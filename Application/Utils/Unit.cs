using CSharpFunctionalExtensions;

namespace Application.Utils;

public readonly struct Unit
{
    public static readonly Unit Value = new();

    public static Task<Result<Unit, Error>> AsResult()
    {
        return Task.FromResult(Result.Success<Unit, Error>(Value));
    }
}