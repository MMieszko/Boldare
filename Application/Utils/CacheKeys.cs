using Application.Models;
using Application.Queries.SearchBrewery;

namespace Application.Utils;

public static class CacheKeys
{
    public static string BrewerySearch(SearchQuery query) => $"brewery-search&query={query}";
    public static string BrewerySearchAndSort(SearchQuery query, SortBreweryBy? sortBy, GeoLocation? geoLocation) => $"brewery-search&query={query}?=sortBy={sortBy}?geoLocation={geoLocation}";
}