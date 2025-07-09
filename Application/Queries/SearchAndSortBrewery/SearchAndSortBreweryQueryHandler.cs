using Application.Contracts;
using Application.Models;
using Application.Queries.SearchBrewery;
using Application.Utils;
using CSharpFunctionalExtensions;
using MediatR;
using Unit = Application.Utils.Unit;

namespace Application.Queries.SearchAndSortBrewery;

internal class SearchAndSortBreweryQueryHandler(IBreweryRepository breweryRepository, ICache cache) : IRequestHandler<SearchAndSortBreweryQuery, Result<IReadOnlyCollection<Brewery>, Error>>
{
    public Task<Result<IReadOnlyCollection<Brewery>, Error>> Handle(SearchAndSortBreweryQuery request, CancellationToken cancellationToken)
        => from query in SearchQuery.Create(request.Query)
           from geoLocation in GetGeoLocation(request)
           from _ in ValidateSortParameters(request.SortBy, geoLocation.GetValueOrDefault())
           from result in cache.GetOrSet(CacheKeys.BrewerySearchAndSort(query, request.SortBy, geoLocation.GetValueOrDefault()), () => breweryRepository.Search(query, request.SortBy, geoLocation.GetValueOrDefault(), cancellationToken))
           select result;

    private static Result<Maybe<GeoLocation>, Error> GetGeoLocation(SearchAndSortBreweryQuery request) => (request.Latitude, request.Longitude) switch
    {
        (null, _) or (_, null) => Maybe<GeoLocation>.None,
        var (latitude, longitude) => GeoLocation.Create(latitude.Value, longitude.Value).Map(r => r.AsMaybe())
    };

    private static Result<Unit, Error> ValidateSortParameters(SortBreweryBy? sortBy, GeoLocation? geoLocation) => (sortBy, geoLocation) switch
    {
        (SortBreweryBy.Distance, null) => new ValidationError(Messages.SortMissingGeoLocation, nameof(SortBreweryBy.Distance)),
        _ => Unit.Value
    };
}