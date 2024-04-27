using VIAEventAssociation.Core.QueryContracts.Contracts;

namespace VIAEventAssociation.Core.QueryContracts.Queries;

public static class UpcomingEventsQuery {

    public record Query(string SearchTitle,
        int CurrentPage,
        int EventsPerPage) : IQuery<Answer>;

    public record Answer(
        ICollection<Event> UpcomingEvents,
        int TotalPages);

    public record Event(
        string EventId,
        string Title,
        string? Date,
        string? StartTime,
        string EventDescription,
        int NumberOfAttendees,
        int MaxGuests,
        string Visibility);

}