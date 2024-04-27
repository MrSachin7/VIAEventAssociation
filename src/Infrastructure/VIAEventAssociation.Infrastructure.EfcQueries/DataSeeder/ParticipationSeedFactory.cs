using System.Collections;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace VIAEventAssociation.Infrastructure.EfcQueries.DataSeeder;

public static class ParticipationSeedFactory {


    public static async Task SeedParticipationsAsync(VeadatabaseProductionContext context) {
        ICollection<JsonParticipation> participations = await GetParticipationFromJson();
        ICollection<VeaEvent> events =await EventSeedFactory.GetEventsFromJson();
        foreach (VeaEvent eVeaEvent in events) {
            await AddParticipantsToEventAsync(context, eVeaEvent.Id, participations);
        }
    }

    private static async Task<ICollection<JsonParticipation>> GetParticipationFromJson() {

        const string filePath = @"./DataSeeder/Data/participations.json";
        string participationAsJson =await File.ReadAllTextAsync(filePath);
        return 
            JsonSerializer.Deserialize<List<JsonParticipation>>(participationAsJson)
            ?? new List<JsonParticipation>();
    }

    private static async Task AddParticipantsToEventAsync(VeadatabaseProductionContext context,
        string eventId,
        ICollection<JsonParticipation> participations) {

        VeaEvent? veaEvent = await context.VeaEvents.SingleAsync(ev => ev.Id.Equals(eventId));

        IEnumerable<JsonParticipation> paricipantsForCurrentEvent = participations.Where(p => p.EventId.Equals(eventId));

        foreach (JsonParticipation jsonParticipation in paricipantsForCurrentEvent) {
            Guest guest = await context.Guests.Include(guest => guest.Events).SingleAsync(g => g.Id.Equals(jsonParticipation.GuestId));
            if (guest.Events.Contains(veaEvent)) {
                continue;
            }
            guest.Events.Add(veaEvent);
        }
    }
    

    public record JsonParticipation(
        string GuestId,
        string EventId
   );
}