using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using VIAEventAssociation.Core.QueryContracts.Contracts;
using VIAEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Core.Tools.OperationResult;
using VIAEventAssociation.Infrastructure.EfcQueries.DataSeeder;

namespace VIAEventAssociation.Infrastructure.EfcQueries.QueryHandlers;

public class UpcomingEventsQueryHandler : IQueryHandler<UpcomingEventsQuery.Query, UpcomingEventsQuery.Answer> {

    private readonly VeadatabaseProductionContext _context;
    private readonly ISystemTime _systemTime;

    public UpcomingEventsQueryHandler(VeadatabaseProductionContext context, ISystemTime systemTime) {
        _context = context;
        _systemTime = systemTime;
    }

    public async Task<Result<UpcomingEventsQuery.Answer>> HandleAsync(UpcomingEventsQuery.Query query) {
        IQueryable<VeaEvent> allUpcoming = _context.VeaEvents
            .Where(evt => evt.Title.ToLower().Contains(query.SearchTitle)
                          && evt.StartDateTime > _systemTime.CurrentTime());

        List<VeaEvent> veaEvents = await allUpcoming
            .Include(evt => evt.Guests)
            .Skip((query.CurrentPage - 1) * query.EventsPerPage)
            .Take(query.EventsPerPage).ToListAsync();

        int totalEvents = await allUpcoming.CountAsync();
        int totalPages = (int) Math.Ceiling((double) totalEvents / query.EventsPerPage);

        ImmutableList<UpcomingEventsQuery.Event> events = veaEvents.Select(evt => new UpcomingEventsQuery.Event(evt.Id,
            evt.Title,
            evt.StartDateTime?.Date.ToString(DateTimeFormat.DateFormat),
            evt.StartDateTime?.TimeOfDay.ToString(DateTimeFormat.TimeFormat),
            evt.Description,
            evt.Guests.Count,
            evt.MaxGuests,
            evt.Visibility)).ToImmutableList();

        return new UpcomingEventsQuery.Answer(events, totalPages);
    }
}