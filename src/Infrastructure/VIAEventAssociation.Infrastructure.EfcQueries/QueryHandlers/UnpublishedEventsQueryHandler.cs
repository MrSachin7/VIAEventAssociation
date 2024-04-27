using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.QueryContracts.Contracts;
using VIAEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Infrastructure.EfcQueries.QueryHandlers;

public class UnpublishedEventsQueryHandler : IQueryHandler<UnpublishedEventsQuery.Query, UnpublishedEventsQuery.Answer> {

    private readonly VeadatabaseProductionContext _context;

    public UnpublishedEventsQueryHandler(VeadatabaseProductionContext context) {
        _context = context;
    }


    public async Task<Result<UnpublishedEventsQuery.Answer>> HandleAsync(UnpublishedEventsQuery.Query query) {
        var veaEvents = await _context.VeaEvents
            .Where(evt => !evt.Status.Equals(EventStatus.Active.DisplayName))

            // Only select the properties we need, this is much more efficient than selecting all properties
            .Select(evt => new {evt.Id, evt.Title, evt.Status, evt.StartDateTime})
            .ToListAsync();

        ImmutableList<UnpublishedEventsQuery.Event> draftEvents = veaEvents.Where(evt => evt.Status.Equals(EventStatus.Draft.DisplayName))
            .OrderBy(evt => evt.StartDateTime)
            .Select(evt => new UnpublishedEventsQuery.Event(evt.Id, evt.Title))
            .ToImmutableList();

        ImmutableList<UnpublishedEventsQuery.Event> readyEvents = veaEvents.Where(evt => evt.Status.Equals(EventStatus.Ready.DisplayName))
            .OrderBy(evt => evt.StartDateTime)
            .Select(evt => new UnpublishedEventsQuery.Event(evt.Id, evt.Title))
            .ToImmutableList();

        ImmutableList<UnpublishedEventsQuery.Event> cancelledEvents = veaEvents.Where(evt => evt.Status.Equals(EventStatus.Cancelled.DisplayName))
            .OrderBy(evt => evt.StartDateTime)
            .Select(evt => new UnpublishedEventsQuery.Event(evt.Id, evt.Title))
            .ToImmutableList();

        return new UnpublishedEventsQuery.Answer(draftEvents, readyEvents, cancelledEvents);


    }
}