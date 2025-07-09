using Application.Contracts;
using Application.Utils;
using CSharpFunctionalExtensions;
using Application.Models;
using Application.Extensions;
using Application.Queries.SearchBrewery;

namespace Infrastructure.Repositories;

internal class InMemoryBreweryRepository : IBreweryRepository
{
    private Brewery[] _breweries = [];

    public Task<Result<Unit, Error>> Set(IReadOnlyCollection<Brewery> breweries)
    {
        _breweries = breweries.ToArray();

        return Unit.AsResult();
    }
    public Task<Result<IReadOnlyCollection<Brewery>, Error>> Search(SearchQuery query, CancellationToken cancellationToken)
        => _breweries.Where(b => b.Name.Contains(query, StringComparison.OrdinalIgnoreCase) || b.City.Contains(query, StringComparison.OrdinalIgnoreCase))
                     .ToList()
                     .AsResult();
    public Task<Result<IReadOnlyCollection<Brewery>, Error>> Search(SearchQuery query, SortBreweryBy? sortBy, GeoLocation? geoLocation, CancellationToken cancellationToken)
    {
        if (sortBy == SortBreweryBy.Distance && geoLocation == null)
            return Task.FromResult(Result.Failure<IReadOnlyCollection<Brewery>, Error>(new ValidationError(Messages.SortMissingGeoLocation, nameof(SortBreweryBy))));

        return Search(query, cancellationToken).Map(x => sortBy switch
        {
            SortBreweryBy.Name => x.OrderBy(b => b.Name).ToList(),
            SortBreweryBy.City => x.OrderBy(b => b.City).ToList(),
            SortBreweryBy.Distance => x.OrderBy(b => b.GeoLocation?.DistanceTo(geoLocation!)).ToList(),
            _ => x
        });
    }
}