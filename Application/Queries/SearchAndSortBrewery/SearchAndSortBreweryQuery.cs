using Application.Models;
using Application.Queries.SearchBrewery;
using Application.Utils;
using CSharpFunctionalExtensions;
using MediatR;

namespace Application.Queries.SearchAndSortBrewery
{
    public record SearchAndSortBreweryQuery(string Query, SortBreweryBy? SortBy, double? Latitude, double? Longitude) : IRequest<Result<IReadOnlyCollection<Brewery>, Error>>;
}
