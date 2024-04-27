using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
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
        VeaEvent? eventFromDb = await _context.VeaEvents
            .Include(evt => evt.Guests.Skip((query.GuestPageNumber-1) * query.GuestsPerPage).Take(query.GuestsPerPage))
            .Include(evt => evt.Location)
            .FirstOrDefaultAsync(evt => evt.Id.Equals(query.EventId));

        // Todo: Ask troels..
        /*
        Here, I decided to make a separate query to get the total number of guests on the event, 
           I could instead have used the eventFromDb.Guests.Count, and instead not made the pagination query above 
           but I am genuinely not sure which is more efficient / better approach in this case.
        **/
        int totalGuestsOnEvent = await _context.Guests.CountAsync(guest => guest.Events.Any(evt => evt.Id.Equals(query.EventId)));

        if (eventFromDb is null) {
            return Error.NotFound(ErrorMessage.EventNotFound(query.EventId));
        }

        return ToAnswer(eventFromDb,query.GuestPageNumber, query.GuestsPerPage, totalGuestsOnEvent);
    }

    private EventDetailsQuery.Answer ToAnswer(VeaEvent evt, int pageNumber, int guestsPerPage, int totalGuestsOnEvent) {
        EventDetailsQuery.Event answerEvent = new EventDetailsQuery.Event(evt.Id,
            evt.Title,
            evt.Description,
            evt.StartDateTime?.Date.ToString(DateTimeFormat.DateFormat),
            evt.StartDateTime?.TimeOfDay.ToString(DateTimeFormat.TimeFormat),
            evt.Visibility,
            evt.Guests.Count,
            evt.MaxGuests,
            evt.Location?.LocationName);

        ImmutableList<EventDetailsQuery.Guest> guests = evt.Guests.Select(guest => new EventDetailsQuery.Guest(
                guest.Id,
                $"{guest.FirstName} {guest.LastName}",
                guest.ProfilePictureUrl))
            .ToImmutableList();

        int totalGuestPages = (int) Math.Ceiling((double) totalGuestsOnEvent / guestsPerPage);

        EventDetailsQuery.Guests answerGuests = new EventDetailsQuery.Guests(guests, totalGuestPages);

        return new EventDetailsQuery.Answer(answerEvent, answerGuests);
    }
}