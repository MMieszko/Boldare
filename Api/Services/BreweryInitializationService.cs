using System.Text.Json;
using System.Text.Json.Serialization;
using Application.Contracts;
using Application.Models;
using CSharpFunctionalExtensions;

namespace Api.Services;

public class BreweryInitializationService(IServiceScopeFactory scopeFactory, IHttpClientFactory httpClientFactory, ILogger<BreweryInitializationService> logger) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            var client = httpClientFactory.CreateClient(nameof(BreweryInitializationService));

            var response = await client.GetAsync("breweries", cancellationToken);
            var rawContent = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                logger.LogCritical("Failed to fetch breweries with status code - {statusCode}. Raw response: {rawResponse}", response.StatusCode, rawContent);

                return;
            }

            var jBreweries = JsonSerializer.Deserialize<BreweryJsonModel[]>(rawContent);

            if (jBreweries == null)
            {
                logger.LogCritical("Failed to deserialize breweries. Raw response: {rawResponse}", rawContent);

                return;
            }

            var breweries = jBreweries.Select(MapBrewery).ToList();

            await scopeFactory.CreateScope().ServiceProvider.GetService<IBreweryRepository>()!.Set(breweries)
                .TapError(error => logger.LogCritical("Failed to save breweries into repository with error: {error}", error));
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Unhandled exception while trying to initialize breweries");
        }
    }

    private static Brewery MapBrewery(BreweryJsonModel jBrewery)
    {
        var geoLocation = jBrewery.Longitude.HasValue && jBrewery.Latitude.HasValue
            ? new GeoLocation(jBrewery.Latitude.Value, jBrewery.Longitude.Value)
            : null;

        return new Brewery(jBrewery.Id, jBrewery.Name, geoLocation, jBrewery.City);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

internal record BreweryJsonModel
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("city")]
    public string City { get; set; }
    [JsonPropertyName("longitude")]
    public double? Longitude { get; set; }
    [JsonPropertyName("latitude")]
    public double? Latitude { get; set; }
}