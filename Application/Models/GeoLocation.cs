using Application.Utils;
using CSharpFunctionalExtensions;

namespace Application.Models;

public record GeoLocation
{
    private const double EarthRadiusMeters = 6371e3;

    public double Latitude { get; }
    public double Longitude { get; }

    private GeoLocation(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }

    public static Result<GeoLocation, Error> Create(double latitude, double longitude) => (latitude, longitude) switch
    {
        ( < -90 or > 90, _) => new ValidationError(Messages.InvalidLatitude, nameof(latitude)),
        (_, < -180 or > 180) => new ValidationError(Messages.InvalidLongitude, nameof(longitude)),
        _ => new GeoLocation(latitude, longitude)
    };

    public double DistanceTo(GeoLocation other)
    {
        var phi1 = ToRadians(Latitude);
        var phi2 = ToRadians(other.Latitude);
        var deltaPhi = ToRadians(other.Latitude - Latitude);
        var deltaLambda = ToRadians(other.Longitude - Longitude);

        var a = Math.Sin(deltaPhi / 2) * Math.Sin(deltaPhi / 2) +
                Math.Cos(phi1) * Math.Cos(phi2) *
                Math.Sin(deltaLambda / 2) * Math.Sin(deltaLambda / 2);

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        return EarthRadiusMeters * c;

        double ToRadians(double deg) => deg * Math.PI / 180;
    }
}