using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Events.Entities.Invitation;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.AppEntry.Commands.Events;

public class DeclineInvitationCommand  : ICommand{
    public EventId EventId { get; init; }
    public EventInvitationId EventInvitationId { get; init; }

    private DeclineInvitationCommand(EventId eventId, EventInvitationId eventInvitationId) {
        EventId = eventId;
        EventInvitationId = eventInvitationId;
    }

    public static Result<DeclineInvitationCommand> Create(string eventId, string invitationId) {
        Result<EventId> eventIdResult = EventId.Create(eventId);
        Result<EventInvitationId> guestIdResult = EventInvitationId.Create(invitationId);

        return eventIdResult.Combine(guestIdResult).WithPayload(() =>
            new DeclineInvitationCommand(eventIdResult.Payload!, guestIdResult.Payload!));
    }
}