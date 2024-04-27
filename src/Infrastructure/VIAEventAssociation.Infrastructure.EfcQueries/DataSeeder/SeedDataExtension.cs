namespace VIAEventAssociation.Infrastructure.EfcQueries.DataSeeder;

public static class SeedDataExtension {
    public static async Task<VeadatabaseProductionContext> SeedAllDataAsync(this VeadatabaseProductionContext context) {
        await context.Guests.AddRangeAsync(await GuestSeedFactory.GetGuestsFromJson());
        await context.VeaEvents.AddRangeAsync(await EventSeedFactory.GetEventsFromJson());

        await context.SaveChangesAsync();
        await ParticipationSeedFactory.SeedParticipationsAsync(context);
        await context.SaveChangesAsync();

        await InvitationSeedFactory.SeedInvitationsAsync(context);
        await context.SaveChangesAsync();


        return context;
    }
}