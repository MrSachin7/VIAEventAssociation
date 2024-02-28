using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Events.state;

internal class ActiveStatusState : IEventStatusState {
    private static readonly EventStatus Status = EventStatus.Active;

    private ActiveStatusState() {
    }

    internal static ActiveStatusState GetInstance() {
        return new ActiveStatusState();
    }


    public EventStatus CurrentStatus() {
        return Status;
    }

    public Result UpdateTitle(VeaEventWithStatePattern veaEventWithStatePattern, EventTitle title) {
        return Error.BadRequest(ErrorMessage.ActiveEventIsUnmodifiable);
    }

    public Result UpdateDescription(VeaEventWithStatePattern viaEventWithStatePattern,
        EventDescription eventDescription) {
        return Error.BadRequest(ErrorMessage.ActiveEventIsUnmodifiable);
    }

    public Result MakePublic(VeaEventWithStatePattern veaEventWithStatePattern) {
        veaEventWithStatePattern.SetVisibility(EventVisibility.Public);
        return Result.Success();
    }

    public Result MakePrivate(VeaEventWithStatePattern veaEventWithStatePattern) {
        return Error.BadRequest(ErrorMessage.ActiveEventCannotBePrivate);
    }

    public Result UpdateMaxNumberOfGuests(VeaEventWithStatePattern veaEventWithStatePattern, EventMaxGuests maxGuests) {
        return veaEventWithStatePattern.SetMaximumNumberOfGuests(maxGuests);
    }

    // Todo: there is nothing in the use case description regarding the active state
    public Result MakeReady(VeaEventWithStatePattern veaEventWithStatePattern) {
        return Error.BadRequest(ErrorMessage.ActiveEventCannotBeMadeReady);
    }

    public Result MakeActive(VeaEventWithStatePattern veaEventWithStatePattern) {
        return Result.Success();
    }
}