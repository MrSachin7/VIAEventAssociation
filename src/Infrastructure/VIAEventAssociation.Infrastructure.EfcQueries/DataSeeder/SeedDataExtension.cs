
namespace VIAEventAssociation.Infrastructure.EfcQueries.DataSeeder;

public static class SeedDataExtension {
    public static async Task<VeadatabaseProductionContext> SeedAllDataAsync(this VeadatabaseProductionContext context) {
        await context.Locations.AddRangeAsync(LocationSeedFactory.GetLocationsFromJson());
        await context.SaveChangesAsync();

        await context.Guests.AddRangeAsync( GuestSeedFactory.GetGuestsFromJson());
        await context.SaveChangesAsync();

        await context.VeaEvents.AddRangeAsync( EventSeedFactory.GetEventsFromJson());
        await context.SaveChangesAsync();

        await ParticipationSeedFactory.SeedParticipationsAsync(context);
        await context.SaveChangesAsync();

        await InvitationSeedFactory.SeedInvitationsAsync(context);
        await context.SaveChangesAsync();
        return context;
    }
}