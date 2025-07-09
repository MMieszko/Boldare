namespace Application.Models;

public record GeoLocation(double Latitude, double Longitude)
{
    private const double EarthRadiusMeters = 6371e3;

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