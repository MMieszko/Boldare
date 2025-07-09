using Swashbuckle.AspNetCore.Annotations;

namespace Api.Models.Requests;

public class BrewerySearchRequestModel
{
    [SwaggerSchema("Search phrase (e.g., brewery name, city)")]
    public string Query { get; set; }
}