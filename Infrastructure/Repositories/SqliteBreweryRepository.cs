using Application.Contracts;
using Application.Models;
using Application.Queries.SearchBrewery;
using Application.Utils;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

internal class SqliteBreweryRepository(BreweryContext context, ILogger<SqliteBreweryRepository> logger) : IBreweryRepository
{
    public async Task<Result<Unit, Error>> Set(IReadOnlyCollection<Brewery> breweries)
    {
        try
        {
            await context.Breweries.ExecuteDeleteAsync();

            //https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/bulk-insert
            context.AddRange(breweries);

            await context.SaveChangesAsync();

            return Unit.Value;
        }
        catch (Exception ex)
        {
            return new Error(ex);
        }
    }

    public async Task<Result<IReadOnlyCollection<Brewery>, Error>> Search(SearchQuery query, SortBreweryBy? sortBy, GeoLocation? geoLocation, CancellationToken cancellationToken)
    {
        if (sortBy == SortBreweryBy.Distance && geoLocation == null)
            return new ValidationError(Messages.SortMissingGeoLocation, nameof(SortBreweryBy));

        var queryable = GetSearchQuery(query);

        if (sortBy == SortBreweryBy.Distance)
        {
            logger.LogWarning("Sorting by distance in not available using EF. Sorting will be executed in memory");

            return (await queryable.ToListAsync(cancellationToken)).OrderBy(b => b.GeoLocation?.DistanceTo(geoLocation!) ?? double.MaxValue).ToList();
        }

        queryable = sortBy switch
        {
            SortBreweryBy.Name => queryable.OrderBy(b => b.Name),
            SortBreweryBy.City => queryable.OrderBy(b => b.City),
            _ => queryable
        };

        return await queryable.ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<Result<IReadOnlyCollection<Brewery>, Error>> Search(SearchQuery query, CancellationToken cancellationToken)
        => await GetSearchQuery(query).ToListAsync(cancellationToken);

    private IQueryable<Brewery> GetSearchQuery(SearchQuery query)
    {
        var lower = query.Value.ToLower();

        return context.Breweries
            .AsNoTracking()
            .Where(b => EF.Functions.Like(b.Name.ToLower(), $"%{lower}%") ||
                        EF.Functions.Like(b.City.ToLower(), $"%{lower}%"));
    }
}