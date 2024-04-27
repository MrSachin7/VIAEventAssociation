using System.Text.Json;

namespace VIAEventAssociation.Infrastructure.EfcQueries.DataSeeder;

public static class LocationSeedFactory {

    public static async Task<ICollection<Location>> GetLocationsFromJson() {

        const string filePath = @"./DataSeeder/Data/locations.json";
        string locationsAsJson = await File.ReadAllTextAsync(filePath);
        List<JsonLocation>? jsonLocations = JsonSerializer.Deserialize<List<JsonLocation>>(locationsAsJson);
        return jsonLocations is null ? new List<Location>() : jsonLocations.Select(ToLocation).ToList();
    }

    private static Location ToLocation(JsonLocation jsonLocation) {
        return new Location() {
            Id = jsonLocation.Id,
            LocationName = jsonLocation.Name,
            LocationMaxGuests = jsonLocation.MaxCapacity
        };
    }


    private record JsonLocation(
        string Id,
        string Name,
        int MaxCapacity);
}