using Application.Models;
using Application.Queries.SearchBrewery;
using Application.Utils;
using CSharpFunctionalExtensions;
using MediatR;

namespace Application.Queries.SearchAndSortBrewery
{
    public record SearchAndSortBreweryQuery(string Query, SortBreweryBy? SortBy, GeoLocation? GeoLocation) : IRequest<Result<IReadOnlyCollection<Brewery>, Error>>;
}
