using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.AppEntry.Commands.Events;

public class AcceptInvitationCommand {
    public EventId Id { get; init; }
    public GuestId GuestId { get; init; }

    private AcceptInvitationCommand(EventId id, GuestId guestId) {
        Id = id;
        GuestId = guestId;
    }

    public static Result<AcceptInvitationCommand> Create(string eventId, string guestId) {
        Result<EventId> eventIdResult = EventId.Create(eventId);
        Result<GuestId> guestIdResult = GuestId.Create(guestId);

        return eventIdResult.Combine(guestIdResult).WithPayload(() =>
            new AcceptInvitationCommand(eventIdResult.Payload!, guestIdResult.Payload!));
    }
}