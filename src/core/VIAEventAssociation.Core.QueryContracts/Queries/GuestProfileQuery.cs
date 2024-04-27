using VIAEventAssociation.Core.QueryContracts.Contracts;

namespace VIAEventAssociation.Core.QueryContracts.Queries;

public static class GuestProfileQuery {
    public record Query(
        string GuestId
    ) : IQuery<Answer>;

    public record Answer(
        GuestProfile GuestProfile,
        ICollection<UpcomingEvent> UpcomingEvents,
        ICollection<PastEvent> PastEvents);

    public record GuestProfile(
        string GuestId,
        string Name,
        string Email,
        string? ProfilePictureUrl,
        int TotalPendingInvitations);

    public record UpcomingEvent(
        string EventId,
        string Title,
        int NumberOfAttendees,
        string? Date,
        string? StartTime);

    public record PastEvent(string EventId,string Title);
}