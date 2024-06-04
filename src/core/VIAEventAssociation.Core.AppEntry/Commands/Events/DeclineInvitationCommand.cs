using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Events.Entities.Invitation;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.AppEntry.Commands.Events;

public class DeclineInvitationCommand : ICommand {
    public EventId EventId { get; init; }
    public EventInvitationId EventInvitationId { get; init; }

    private DeclineInvitationCommand(EventId eventId, EventInvitationId eventInvitationId) {
        EventId = eventId;
        EventInvitationId = eventInvitationId;
    }

    // Todo: Ask, well generally i should be able to do this with just the EventInvitationId, is this the limitation of clean architecture?
    // Maybe, i should use the guestId here, but then why have invitationId, can just make the composite PK with two ids...
    // After some time. I realized maybe you want to invite the guest again when he rejects , so composite key doesnt make sense...
    // but still i am using more information than needed, maybe something i am missing ??.
    public static Result<DeclineInvitationCommand> Create(string eventId, string invitationId) {
        Result<EventId> eventIdResult = EventId.Create(eventId);
        Result<EventInvitationId> guestIdResult = EventInvitationId.Create(invitationId);

        return eventIdResult.Combine(guestIdResult).WithPayload(() =>
            new DeclineInvitationCommand(eventIdResult.Payload!, guestIdResult.Payload!));
    }
}