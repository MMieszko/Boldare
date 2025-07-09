using Application.Models;
using Application.Queries.SearchBrewery;
using Application.Utils;
using CSharpFunctionalExtensions;

namespace Application.Contracts;

public interface IBreweryRepository
{
    Task<Result<Unit, Error>> Set(IReadOnlyCollection<Brewery> breweries);
    Task<Result<IReadOnlyCollection<Brewery>, Error>> Search(SearchQuery query, CancellationToken cancellationToken);
    Task<Result<IReadOnlyCollection<Brewery>, Error>> Search(SearchQuery query, SortBreweryBy? sortBy, GeoLocation? geoLocation, CancellationToken cancellationToken);
}