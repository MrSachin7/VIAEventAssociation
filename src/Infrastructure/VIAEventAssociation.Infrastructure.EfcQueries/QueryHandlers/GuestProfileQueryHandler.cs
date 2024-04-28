using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using VIAEventAssociation.Core.QueryContracts.Contracts;
using VIAEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Core.Tools.OperationResult;
using VIAEventAssociation.Infrastructure.EfcQueries.DataSeeder;

namespace VIAEventAssociation.Infrastructure.EfcQueries.QueryHandlers;

public class GuestProfileQueryHandler : IQueryHandler<GuestProfileQuery.Query, GuestProfileQuery.Answer> {
    private readonly VeadatabaseProductionContext _context;
    private readonly ISystemTime _systemTime;

    public GuestProfileQueryHandler(VeadatabaseProductionContext context, ISystemTime systemTime) {
        _context = context;
        _systemTime = systemTime;
    }

    public async Task<Result<GuestProfileQuery.Answer>> HandleAsync(GuestProfileQuery.Query query) {
        // Include all required data on one database request
        Guest? guestFromDb = await _context.Guests
            .Include(guest => guest.Events)
            .ThenInclude(evt => evt.Guests)
            .Include(guest => guest.EventInvitations.Where(invitations => invitations.Status.Equals("Pending")))
            .FirstOrDefaultAsync(guest => guest.Id.Equals(query.GuestId));

        if (guestFromDb is null) {
            return Error.NotFound(ErrorMessage.GuestNotFound(query.GuestId));
        }

        return ToAnswer(guestFromDb);
    }

    private GuestProfileQuery.Answer ToAnswer(Guest guestFromDb) {
        // Since only pending invitations are included in the query, we can get the count directly
        int totalPendingInvitations = guestFromDb.EventInvitations.Count;

        GuestProfileQuery.GuestProfile guestProfile = new GuestProfileQuery.GuestProfile(guestFromDb.Id,
            $"{guestFromDb.FirstName} {guestFromDb.LastName}",
            guestFromDb.Email,
            guestFromDb.ProfilePictureUrl,
            totalPendingInvitations);

        // Upcoming
        ImmutableList<GuestProfileQuery.UpcomingEvent> upcomingEvents = guestFromDb.Events
            .Where(evt => evt.StartDateTime > _systemTime.CurrentTime())
            .OrderBy(evt => evt.StartDateTime)
            .Select(evt => new GuestProfileQuery.UpcomingEvent(evt.Id, evt.Title, evt.Guests.Count,
                evt.StartDateTime?.Date.ToString(DateTimeFormat.DateFormat),
                evt.StartDateTime?.TimeOfDay.ToString(DateTimeFormat.TimeFormat))
            ).ToImmutableList();

        // Past
        ImmutableList<GuestProfileQuery.PastEvent> pastEvents = guestFromDb.Events
            .Where(evt => evt.StartDateTime < _systemTime.CurrentTime())
            .OrderBy(evt => evt.StartDateTime)
            .Select(evt => new GuestProfileQuery.PastEvent(evt.Id, evt.Title)
            ).ToImmutableList();

        return new GuestProfileQuery.Answer(guestProfile, upcomingEvents, pastEvents);
    }
}