using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using VIAEventAssociation.Infrastructure.EfcQueries.DataSeeder.Data;

namespace VIAEventAssociation.Infrastructure.EfcQueries.DataSeeder;

public static class ParticipationSeedFactory {


    public static async Task SeedParticipationsAsync(VeadatabaseProductionContext context) {
        ICollection<JsonParticipation> participations =  GetParticipationFromJson();
        foreach (JsonParticipation participation in participations) {
            VeaEvent? veaEvent = await context.VeaEvents.FindAsync(participation.EventId);
            Guest? guest = await context.Guests.FindAsync(participation.GuestId);

            if (guest is null) {
                continue;
            }
            veaEvent?.Guests.Add(guest!);
            await context.SaveChangesAsync();
        }
       
    }

    private static  ICollection<JsonParticipation> GetParticipationFromJson() {

        string participationAsJson =ParticipationsData.Json;
        return 
            JsonSerializer.Deserialize<List<JsonParticipation>>(participationAsJson)
            ?? new List<JsonParticipation>();
    }

    private static async Task AddParticipantsToEventAsync(VeadatabaseProductionContext context,
        string eventId,
        ICollection<JsonParticipation> participations) {

        VeaEvent? veaEvent = await context.VeaEvents.SingleAsync(ev => ev.Id.Equals(eventId));

        IList<JsonParticipation> paricipantsForCurrentEvent = participations.Where(p => p.EventId.Equals(eventId)).ToList();

        foreach (JsonParticipation jsonParticipation in paricipantsForCurrentEvent) {
            Guest? guest = await context.Guests.Include(guest => guest.Events).SingleOrDefaultAsync(g => g.Id.Equals(jsonParticipation.GuestId));
            if (guest is null) {
                continue;
            }

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