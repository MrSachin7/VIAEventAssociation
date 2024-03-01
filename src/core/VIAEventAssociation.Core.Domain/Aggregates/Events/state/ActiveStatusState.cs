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

    public Result UpdateTitle(VeaEvent veaEvent, EventTitle title) {
        return Error.BadRequest(ErrorMessage.ActiveEventIsUnmodifiable);
    }

    public Result UpdateDescription(VeaEvent viaEvent,
        EventDescription eventDescription) {
        return Error.BadRequest(ErrorMessage.ActiveEventIsUnmodifiable);
    }

    public Result MakePublic(VeaEvent veaEvent) {
        veaEvent.SetVisibility(EventVisibility.Public);
        return Result.Success();
    }

    public Result MakePrivate(VeaEvent veaEvent) {
        return Error.BadRequest(ErrorMessage.ActiveEventCannotBePrivate);
    }

    public Result UpdateMaxNumberOfGuests(VeaEvent veaEvent, EventMaxGuests maxGuests) {
        if ( maxGuests.Value < veaEvent.MaxGuests.Value) {
            return Error.BadRequest(ErrorMessage.ActiveEventCannotReduceMaxGuests);
        }
        return veaEvent.SetMaximumNumberOfGuests(maxGuests);
    }

    // Todo: there is nothing in the use case description regarding the active state
    public Result MakeReady(VeaEvent veaEvent) {
        return Error.BadRequest(ErrorMessage.ActiveEventCannotBeMadeReady);
    }

    public Result MakeActive(VeaEvent veaEvent) {
        return Result.Success();
    }

    public Result MakeCancelled(VeaEvent veaEvent) {
        veaEvent.SetStatusToCancelled();
        return Result.Success();
    }

    public Result UpdateEventDuration(VeaEvent veaEvent, EventDuration eventDuration) {
        return Error.BadRequest(ErrorMessage.ActiveEventIsUnmodifiable);
    }
}