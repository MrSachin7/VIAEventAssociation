using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using VIAEventAssociation.Core.Domain.Aggregates.Events.Entities;
using VIAEventAssociation.Core.QueryContracts.Contracts;
using VIAEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Infrastructure.EfcQueries.QueryHandlers;

public class EventDetailsQueryHandler : IQueryHandler<EventDetailsQuery.Query, EventDetailsQuery.Answer> {
    private readonly VeadatabaseProductionContext _context;

    public EventDetailsQueryHandler(VeadatabaseProductionContext context) {
        _context = context;
    }

    public async Task<Result<EventDetailsQuery.Answer>> HandleAsync(EventDetailsQuery.Query query) {
        int skipCount = (query.GuestPageNumber - 1) * query.GuestsPerPage;
        int takeCount = query.GuestsPerPage;


        // This is not the best way to get the event with guests
        VeaEvent? eventFromDb = await _context.VeaEvents
            .Include(evt => evt.EventInvitations
                .Where(inv => inv.Status.Equals(JoinStatus.Accepted.DisplayName)))
            .ThenInclude(inv => inv.Guest)
            .Include(evt => evt.Guests)
            .Include(evt => evt.Location)
            .FirstOrDefaultAsync(evt => evt.Id.Equals(query.EventId));

        if (eventFromDb is null) {
            return Error.NotFound(ErrorMessage.EventNotFound(query.EventId));
        }

        IEnumerable<EventDetailsQuery.Guest> acceptedInvitations = eventFromDb.EventInvitations.Select(
            inv => new EventDetailsQuery.Guest(inv.GuestId,
                $"{inv.Guest.FirstName} {inv.Guest.LastName}",
                inv.Guest.ProfilePictureUrl));

        IEnumerable<EventDetailsQuery.Guest> intendedParticipants = eventFromDb.Guests.Select(
            guest => new EventDetailsQuery.Guest(guest.Id,
                $"{guest.FirstName} {guest.LastName}",
                guest.ProfilePictureUrl));

        ICollection<EventDetailsQuery.Guest> guests = acceptedInvitations
            .Concat(intendedParticipants)
            .Skip(skipCount)
            .Take(takeCount)
            .ToImmutableList();

        int totalGuestsOnEvent = eventFromDb.EventInvitations.Count + eventFromDb.Guests.Count;
        int totalGuestPages = (int) Math.Ceiling((double) totalGuestsOnEvent / query.GuestsPerPage);
        EventDetailsQuery.Guests answerGuests = new EventDetailsQuery.Guests(guests, totalGuestPages);

        EventDetailsQuery.Event answerEvent = new EventDetailsQuery.Event(eventFromDb.Id,
            eventFromDb.Title,
            eventFromDb.Description,
            eventFromDb.StartDateTime?.Date.ToString(DateTimeFormat.DateFormat),
            eventFromDb.StartDateTime?.TimeOfDay.ToString(DateTimeFormat.TimeFormat),
            eventFromDb.Visibility,
            totalGuestsOnEvent,
            eventFromDb.MaxGuests,
            eventFromDb.Location?.LocationName);

        return new EventDetailsQuery.Answer(answerEvent, answerGuests);

    }


}
