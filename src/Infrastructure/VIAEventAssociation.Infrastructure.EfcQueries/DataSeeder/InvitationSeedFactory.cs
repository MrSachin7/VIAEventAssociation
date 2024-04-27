using System.Text.Json;
using VIAEventAssociation.Infrastructure.EfcQueries.DataSeeder.Data;

namespace VIAEventAssociation.Infrastructure.EfcQueries.DataSeeder;

public static class InvitationSeedFactory {

    public static async Task SeedInvitationsAsync(VeadatabaseProductionContext context) {
        ICollection<JsonInvitation> invitationsFromFile =  GetInvitationsFromJson();

        IEnumerable<EventInvitation> eventInvitations = invitationsFromFile.Select(inv => new EventInvitation() {
            Id = Guid.NewGuid().ToString(),
            GuestId = inv.GuestId,
            VeaEventId = inv.EventId,
            Status = inv.Status
        });

        foreach (EventInvitation invitation in eventInvitations) {
            string eventId = invitation.VeaEventId;
            VeaEvent? veaEvent = await context.VeaEvents.FindAsync(eventId);
            Guest? guest = await context.Guests.FindAsync(invitation.GuestId);

            if (guest is null) {
                continue;
            }
            veaEvent?.EventInvitations.Add(invitation);
            await context.SaveChangesAsync();
        }

    }


    private static  ICollection<JsonInvitation> GetInvitationsFromJson() {
        string invitationsAsJson = InvitationsData.Json;
        return
            JsonSerializer.Deserialize<List<JsonInvitation>>(invitationsAsJson)
            ?? [];
    }


    public record JsonInvitation(
        string EventId,
        string GuestId,
        string Status);
}