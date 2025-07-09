using Application.Contracts;
using Application.Models;
using Application.Utils;
using CSharpFunctionalExtensions;
using MediatR;

namespace Application.Queries.SearchBrewery;

internal record SearchBreweryQueryHandler(IBreweryRepository breweryRepository, ICache cache) : IRequestHandler<SearchBreweryQuery, Result<IReadOnlyCollection<Brewery>, Error>>
{
    public Task<Result<IReadOnlyCollection<Brewery>, Error>> Handle(SearchBreweryQuery request, CancellationToken cancellationToken)
        => from query in SearchQuery.Create(request.Query)
           from result in cache.GetOrSet(CacheKeys.BrewerySearch(query), () => breweryRepository.Search(query, cancellationToken))
           select result;
}