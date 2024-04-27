using System.Text.Json;
using VIAEventAssociation.Infrastructure.EfcQueries.DataSeeder.Data;

namespace VIAEventAssociation.Infrastructure.EfcQueries.DataSeeder;

public static class GuestSeedFactory {

    public static  ICollection<Guest> GetGuestsFromJson() {

        string guestsAsJson = GuestsData.Json;
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