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
           from _ in ValidateSortParameters(request)
           from result in cache.GetOrSet(CacheKeys.BrewerySearchAndSort(query, request.SortBy, request.GeoLocation), () => breweryRepository.Search(query, request.SortBy, request.GeoLocation, cancellationToken))
           select result;

    private static Result<Unit, Error> ValidateSortParameters(SearchAndSortBreweryQuery request) => request switch
    {
        { SortBy: SortBreweryBy.Distance, GeoLocation: null } => new ValidationError(Messages.SortMissingGeoLocation, nameof(SortBreweryBy.Distance)),
        _ => Unit.Value
    };
}