using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Events.state;

internal class CancelledStatusState : IEventStatusState {
    private static readonly EventStatus Status = EventStatus.Cancelled;

    internal static CancelledStatusState GetInstance() {
        return new CancelledStatusState();
    }
    public EventStatus CurrentStatus() {
        return Status;
    }

    public Result UpdateTitle(VeaEvent veaEvent, EventTitle title) {
        return Error.BadRequest(ErrorMessage.CancelledEventIsUnmodifiable);
    }

    public Result UpdateDescription(VeaEvent veaEvent, EventDescription eventDescription) {
        return Error.BadRequest(ErrorMessage.CancelledEventIsUnmodifiable);
    }

    public Result MakePublic(VeaEvent veaEvent) {
        return Error.BadRequest(ErrorMessage.CancelledEventIsUnmodifiable);
    }

    public Result MakePrivate(VeaEvent veaEvent) {
        return Error.BadRequest(ErrorMessage.CancelledEventIsUnmodifiable);
    }

    public Result UpdateMaxNumberOfGuests(VeaEvent veaEvent, EventMaxGuests maxGuests) {
        return Error.BadRequest(ErrorMessage.CancelledEventIsUnmodifiable);
    }

    public Result MakeReady(VeaEvent veaEvent) {
        return Error.BadRequest(ErrorMessage.CancelledEventCannotBeMadeReady);
    }

    public Result MakeActive(VeaEvent veaEvent) {
        return Error.BadRequest(ErrorMessage.CancelledEventCannotBeActivated);
    }

    public Result MakeCancelled(VeaEvent veaEvent) {
        return Error.BadRequest(ErrorMessage.OnlyActiveEventsCanBeCancelled);
    }
}