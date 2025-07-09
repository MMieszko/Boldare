namespace Application.Models;

public class Brewery(string id, string name, GeoLocation? geoLocation, string city)
{
    public string Id { get; } = id;
    public string Name { get; } = name;
    public GeoLocation? GeoLocation { get; } = geoLocation;
    public string City { get; } = city;

    private Brewery() : this(string.Empty, string.Empty, null, string.Empty) { }
}
