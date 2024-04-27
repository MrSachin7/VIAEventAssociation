using System.Text.Json;

namespace VIAEventAssociation.Infrastructure.EfcQueries.DataSeeder;

public static class EventSeedFactory {
    public static async Task<ICollection<VeaEvent>> GetEventsFromJson() {
        const string eventsFilePath = @"./DataSeeder/Data/events.json";
        string eventsAsJson = await File.ReadAllTextAsync(eventsFilePath);
        List<JsonEvent>? jsonEvents = JsonSerializer.Deserialize<List<JsonEvent>>(eventsAsJson);
        return jsonEvents is null ? [] : jsonEvents.Select(ToVeaEvent).ToList();

    }

    private static VeaEvent ToVeaEvent(JsonEvent jsonEvent) {
        return new VeaEvent() {
            Id = jsonEvent.Id,
            Title = jsonEvent.Title,
            Description = jsonEvent.Description,
            StartDateTime = DateTime.Parse(jsonEvent.Start),
            EndDateTime = DateTime.Parse(jsonEvent.End),
            Visibility = jsonEvent.Visibility,
            Status = jsonEvent.Status,
            MaxGuests = jsonEvent.MaxGuests
        };
    }

    private record JsonParticipation(string EventId, string GuestId);

    private record JsonEvent(
        string Id,
        string Title,
        string Description,
        string Status,
        string Visibility,
        string Start,
        string End,
        int MaxGuests);
}