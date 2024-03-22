using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.AppEntry.Commands.Events;

public class DeclineInvitationCommand {
    public EventId Id { get; init; }
    public GuestId GuestId { get; init; }

    private DeclineInvitationCommand(EventId id, GuestId guestId) {
        Id = id;
        GuestId = guestId;
    }

    public static Result<DeclineInvitationCommand> Create(string eventId, string guestId) {
        Result<EventId> eventIdResult = EventId.Create(eventId);
        Result<GuestId> guestIdResult = GuestId.Create(guestId);

        return eventIdResult.Combine(guestIdResult).WithPayload(() =>
            new DeclineInvitationCommand(eventIdResult.Payload!, guestIdResult.Payload!));
    }
}