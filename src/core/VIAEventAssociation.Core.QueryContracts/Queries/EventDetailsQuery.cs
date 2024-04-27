using VIAEventAssociation.Core.QueryContracts.Contracts;

namespace VIAEventAssociation.Core.QueryContracts.Queries;

public static class EventDetailsQuery {

    public record Query(string EventId,
        int GuestPageNumber,
        int GuestsPerPage) : IQuery<Answer>;

    public record Answer(
        Event VeaEvent,
        Guests Guests);

    public record Event(
        string EventId,
        string Title,
        string Description,
        string? Date,
        string? StartTime,
        string Visibility,
        int NumberOfAttendees,
        int MaxGuests,
        string? Location);

    public record Guests(
        ICollection<Guest> Participants,
        int TotalGuestPages);
    public record Guest(
        string Id,
        string Name,
        string? ProfilePictureUrl);
}