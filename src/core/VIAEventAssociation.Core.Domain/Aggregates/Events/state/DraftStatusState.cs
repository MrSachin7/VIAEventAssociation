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

    public Result UpdateTitle(VeaEvent veaEvent, EventTitle title) {
        veaEvent.SetTitle(title);
        return Result.Success();
    }

    public Result UpdateDescription(VeaEvent veaEvent,
        EventDescription eventDescription) {
        veaEvent.SetDescription(eventDescription);
        return Result.Success();
    }

    public Result MakePublic(VeaEvent veaEvent) {
        veaEvent.SetVisibility(EventVisibility.Public);
        return Result.Success();
    }

    public Result MakePrivate(VeaEvent veaEvent) {
        veaEvent.SetVisibility(EventVisibility.Private);
        return Result.Success();
    }

    public Result UpdateMaxNumberOfGuests(VeaEvent veaEvent, EventMaxGuests maxGuests) {
        veaEvent.SetMaximumNumberOfGuests(maxGuests);
        return Result.Success();
    }

    public Result MakeReady(VeaEvent veaEvent) {
        return veaEvent.SetStatusToReady();
    }

    public Result MakeActive(VeaEvent veaEvent) {
        Result result = veaEvent.MakeReady();
        return result.IsFailure ? result : veaEvent.MakeActive();
    }
}