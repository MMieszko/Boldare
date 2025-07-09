using Application.Utils;
using CSharpFunctionalExtensions;

namespace Application.Models;

public readonly record struct SearchQuery
{
    public string Value { get; }

    private SearchQuery(string value) => Value = value;

    public static implicit operator string(SearchQuery query) => query.Value;

    public static Result<SearchQuery, Error> Create(string? query) => query switch
    {
        null or { Length: >= 50 } or { Length: < 3 } => new ValidationError(Messages.InvalidSearchQueryLength, nameof(SearchQuery)),
        _ => new SearchQuery(query)
    };

    public override string ToString() => Value;
}