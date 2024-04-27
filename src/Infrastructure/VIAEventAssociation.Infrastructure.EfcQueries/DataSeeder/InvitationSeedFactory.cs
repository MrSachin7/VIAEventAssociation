using System.Text.Json;

namespace VIAEventAssociation.Infrastructure.EfcQueries.DataSeeder;

public static class InvitationSeedFactory {

    public static async Task SeedInvitationsAsync(VeadatabaseProductionContext context) {
        ICollection<JsonInvitation> invitationsFromFile = await GetInvitationsFromJson();
        ICollection<VeaEvent> eventsFromJson = await EventSeedFactory.GetEventsFromJson();

        foreach (var veaEvent in eventsFromJson) {
            await AddInvitationsToEvent(context, veaEvent.Id, invitationsFromFile);
        }
    }

    private static Task AddInvitationsToEvent(VeadatabaseProductionContext context,
        string eventId,
        ICollection<JsonInvitation> invitations) {
        IEnumerable<EventInvitation> eventInvitations = invitations.Where(inv => inv.EventId.Equals(eventId))
            .Select(inv => new EventInvitation() {
                GuestId = inv.GuestId,
                VeaEventId = inv.EventId,
                Status = inv.Status
            });

        return context.EventInvitations.AddRangeAsync(eventInvitations);
    }


    private static async Task<ICollection<JsonInvitation>> GetInvitationsFromJson() {
        const string filePath = @"./DataSeeder/Data/invitations.json";
        string invitationsAsJson = await File.ReadAllTextAsync(filePath);
        return
            JsonSerializer.Deserialize<List<JsonInvitation>>(invitationsAsJson)
            ?? [];
    }


    public record JsonInvitation(
        string EventId,
        string GuestId,
        string Status);
}