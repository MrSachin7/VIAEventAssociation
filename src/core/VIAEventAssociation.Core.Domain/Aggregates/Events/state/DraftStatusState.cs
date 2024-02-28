using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Events.state;

internal class DraftStatusState : IEventStatusState {
    private static readonly EventStatus Status = EventStatus.Draft;

    internal static DraftStatusState GetInstance() {
        return new DraftStatusState();
    }

    public EventStatus CurrentStatus() {
        return Status;
    }

    public Result UpdateTitle(VeaEventWithStatePattern veaEventWithStatePattern, EventTitle title) {
        veaEventWithStatePattern.SetTitle(title);
        return Result.Success();
    }

    public Result UpdateDescription(VeaEventWithStatePattern veaEventWithStatePattern,
        EventDescription eventDescription) {
        veaEventWithStatePattern.SetDescription(eventDescription);
        return Result.Success();
    }

    public Result MakePublic(VeaEventWithStatePattern veaEventWithStatePattern) {
        veaEventWithStatePattern.SetVisibility(EventVisibility.Public);
        return Result.Success();
    }

    public Result MakePrivate(VeaEventWithStatePattern veaEventWithStatePattern) {
        veaEventWithStatePattern.SetVisibility(EventVisibility.Private);
        return Result.Success();
    }

    public Result UpdateMaxNumberOfGuests(VeaEventWithStatePattern veaEventWithStatePattern, EventMaxGuests maxGuests) {
        veaEventWithStatePattern.SetMaximumNumberOfGuests(maxGuests);
        return Result.Success();
    }

    public Result MakeReady(VeaEventWithStatePattern veaEventWithStatePattern) {
        return veaEventWithStatePattern.SetStatusToReady();
    }

    public Result MakeActive(VeaEventWithStatePattern veaEventWithStatePattern) {
        Result result = veaEventWithStatePattern.MakeReady();
        return result.IsFailure ? result : veaEventWithStatePattern.MakeActive();
    }
}