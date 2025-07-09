using Application.Models;
using Application.Queries.SearchBrewery;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Models.Requests;

public class BrewerySearchRequest
{
    [SwaggerSchema("Search phrase (e.g., brewery name, city)")]
    public string Query { get; set; }
    [SwaggerSchema("Sort order of results: Name, Distance, City")]
    public SortBreweryBy? SortBy { get; set; }
    [SwaggerSchema("Latitude (required for sorting by distance)")]
    public double? Latitude { get; set; }

    [SwaggerSchema("Longitude (required for sorting by distance)")]
    public double? Longitude { get; set; }
}