using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.AppEntry.Commands.Events;

public class CancelEventParticipationCommand  : ICommand{
    public EventId EventId { get; init; }
    public GuestId GuestId { get; init; }

    private CancelEventParticipationCommand(EventId eventId, GuestId guestId) {
        EventId = eventId;
        GuestId = guestId;
    }

    public static Result<CancelEventParticipationCommand> Create(string eventId, string guestId) {
        Result<EventId> eventIdResult = EventId.Create(eventId);
        Result<GuestId> guestIdResult = GuestId.Create(guestId);

        return eventIdResult.Combine(guestIdResult).WithPayload(() =>
            new CancelEventParticipationCommand(eventIdResult.Payload!, guestIdResult.Payload!));
    }
}