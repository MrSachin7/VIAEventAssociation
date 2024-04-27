using System.Text.Json;
using VIAEventAssociation.Infrastructure.EfcQueries.DataSeeder.Data;

namespace VIAEventAssociation.Infrastructure.EfcQueries.DataSeeder;

public static class EventSeedFactory {
    public static ICollection<VeaEvent> GetEventsFromJson() {
        string eventsAsJson = EventsData.Json;
        List<JsonEvent>? jsonEvents = JsonSerializer.Deserialize<List<JsonEvent>>(eventsAsJson);
        return jsonEvents is null ? [] : jsonEvents.Select(ToVeaEvent).ToList();
    }

    private static VeaEvent ToVeaEvent(JsonEvent jsonEvent) {
        return new VeaEvent() {
            Id = jsonEvent.Id,
            Title = jsonEvent.Title,
            Description = jsonEvent.Description,
            StartDateTime = DateTimeFormat.ParseFromJsonString(jsonEvent.Start),
            EndDateTime = DateTimeFormat.ParseFromJsonString(jsonEvent.End),
            Visibility = jsonEvent.Visibility,
            Status = jsonEvent.Status,
            MaxGuests = jsonEvent.MaxGuests,
            LocationId = jsonEvent.LocationId
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
        int MaxGuests,
        string LocationId);
}