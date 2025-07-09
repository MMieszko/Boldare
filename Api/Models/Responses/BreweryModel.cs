using Application.Models;

namespace Api.Models.Responses;

public class BreweryModel(string id, string name, string city, double? latitude, double? longitude)
{
    public string Id { get; } = id;
    public string Name { get; } = name;
    public string City { get; } = city;
    public double? Latitude { get; } = latitude;
    public double? Longitude { get; } = longitude;

    public static BreweryModel FromApplication(Brewery brewery) => new(brewery.Id, brewery.Name, brewery.City, brewery.GeoLocation?.Latitude, brewery.GeoLocation?.Longitude);
}