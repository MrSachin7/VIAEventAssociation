using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Events.Entities.Invitation;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.AppEntry.Commands.Events;

public class AcceptInvitationCommand  : ICommand{
    public EventId EventId { get; init; }
    public EventInvitationId InvitationId { get; init; }

    private AcceptInvitationCommand(EventId eventId,EventInvitationId invitationId) {
        EventId = eventId;
        InvitationId = invitationId;
    }

    public static Result<AcceptInvitationCommand> Create(string eventId, string invitationId) {
        Result<EventId> eventIdResult = EventId.Create(eventId);
        Result<EventInvitationId> invitationIdResult = EventInvitationId.Create(invitationId);

        return eventIdResult.Combine(invitationIdResult).WithPayloadIfSuccess(() =>
            new AcceptInvitationCommand(eventIdResult.Payload!, invitationIdResult.Payload!));
    }
}