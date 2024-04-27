using VIAEventAssociation.Core.QueryContracts.Contracts;

namespace VIAEventAssociation.Core.QueryContracts.Queries;

public static class UnpublishedEventsQuery {

    public record Query() : IQuery<Answer>;
    public record Answer(
        ICollection<Event> DraftsEvents,
        ICollection<Event> ReadyEvents,
        ICollection<Event> CancelledEvents);

    public record Event(
        string EventId,
        string Title);
}