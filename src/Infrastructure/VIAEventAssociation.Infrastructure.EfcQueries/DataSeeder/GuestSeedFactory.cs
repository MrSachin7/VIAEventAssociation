using System.Collections;
using System.Text.Json;

namespace VIAEventAssociation.Infrastructure.EfcQueries.DataSeeder;

public static class GuestSeedFactory {

    public static async Task<ICollection<Guest>> GetGuestsFromJson() {

        const string filePath = @"./DataSeeder/Data/guests.json";
        string guestsAsJson = await File.ReadAllTextAsync(filePath);
        List<JsonGuest>? jsonGuests = JsonSerializer.Deserialize<List<JsonGuest>>(guestsAsJson);
        return jsonGuests is null ? new List<Guest>() : jsonGuests.Select(ToGuest).ToList();
    }

    private static Guest ToGuest(JsonGuest jsonGuest) {
        return new Guest() {
            Id = jsonGuest.Id,
            FirstName = jsonGuest.FirstName,
            LastName = jsonGuest.LastName,
            Email = jsonGuest.Email,
            ProfilePictureUrl = jsonGuest.Url
        };
    }


    private record JsonGuest(
        string Id,
        string FirstName,
        string LastName,
        string Email,
        string Url);
}