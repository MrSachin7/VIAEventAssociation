using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Events.state;

internal class ReadyStatusState : IEventStatusState {
    private static readonly EventStatus Status = EventStatus.Ready;


    internal static ReadyStatusState GetInstance() {
        return new ReadyStatusState();
    }

    public EventStatus CurrentStatus() {
        return Status;
    }

    public Result UpdateTitle(VeaEventWithStatePattern veaEventWithStatePattern, EventTitle title) {
        veaEventWithStatePattern.SetTitle(title);
        veaEventWithStatePattern.SetStatusToDraft();
        return Result.Success();
    }

    public Result UpdateDescription(VeaEventWithStatePattern veaEventWithStatePattern,
        EventDescription eventDescription) {
        veaEventWithStatePattern.SetDescription(eventDescription);
        veaEventWithStatePattern.SetStatusToDraft();
        return Result.Success();
    }

    public Result MakePublic(VeaEventWithStatePattern veaEventWithStatePattern) {
        veaEventWithStatePattern.SetVisibility(EventVisibility.Public);
        return Result.Success();
    }

    public Result MakePrivate(VeaEventWithStatePattern veaEventWithStatePattern) {
        // If it is public before, change the status and visibility
        if (veaEventWithStatePattern.GetVisibility().Equals(EventVisibility.Public)) {
            veaEventWithStatePattern.SetVisibility(EventVisibility.Private);
            veaEventWithStatePattern.SetStatusToDraft();
        }
        // Otherwise do nothing.
        return Result.Success();
    }

    public Result UpdateMaxNumberOfGuests(VeaEventWithStatePattern veaEventWithStatePattern, EventMaxGuests maxGuests) {
        veaEventWithStatePattern.SetMaximumNumberOfGuests(maxGuests);
        return Result.Success();
    }

    // Todo: the use case document has no info. Should i return error or success ?
    public Result MakeReady(VeaEventWithStatePattern veaEventWithStatePattern) {
        return Result.Success();
    }

    public Result MakeActive(VeaEventWithStatePattern veaEventWithStatePattern) {
        veaEventWithStatePattern.SetStatusToActive();
        return Result.Success();
    }
}