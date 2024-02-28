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

    public Result UpdateTitle(VeaEventWithStatePattern veaEventWithStatePattern, EventTitle title) {
        return Error.BadRequest(ErrorMessage.CancelledEventIsUnmodifiable);
    }

    public Result UpdateDescription(VeaEventWithStatePattern veaEventWithStatePattern, EventDescription eventDescription) {
        return Error.BadRequest(ErrorMessage.CancelledEventIsUnmodifiable);
    }

    public Result MakePublic(VeaEventWithStatePattern veaEventWithStatePattern) {
        return Error.BadRequest(ErrorMessage.CancelledEventIsUnmodifiable);
    }

    public Result MakePrivate(VeaEventWithStatePattern veaEventWithStatePattern) {
        return Error.BadRequest(ErrorMessage.CancelledEventIsUnmodifiable);
    }

    public Result UpdateMaxNumberOfGuests(VeaEventWithStatePattern veaEventWithStatePattern, EventMaxGuests maxGuests) {
        return Error.BadRequest(ErrorMessage.CancelledEventIsUnmodifiable);
    }

    public Result MakeReady(VeaEventWithStatePattern veaEventWithStatePattern) {
        return Error.BadRequest(ErrorMessage.CancelledEventCannotBeMadeReady);
    }

    public Result MakeActive(VeaEventWithStatePattern veaEventWithStatePattern) {
        return Error.BadRequest(ErrorMessage.CancelledEventCannotBeActivated);
    }
}