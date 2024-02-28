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

    public Result UpdateTitle(VeaEvent veaEvent, EventTitle title) {
        veaEvent.SetTitle(title);
        veaEvent.SetStatusToDraft();
        return Result.Success();
    }

    public Result UpdateDescription(VeaEvent veaEvent,
        EventDescription eventDescription) {
        veaEvent.SetDescription(eventDescription);
        veaEvent.SetStatusToDraft();
        return Result.Success();
    }

    public Result MakePublic(VeaEvent veaEvent) {
        veaEvent.SetVisibility(EventVisibility.Public);
        return Result.Success();
    }

    public Result MakePrivate(VeaEvent veaEvent) {
        // If it is public before, change the status and visibility
        if (veaEvent.GetVisibility().Equals(EventVisibility.Public)) {
            veaEvent.SetVisibility(EventVisibility.Private);
            veaEvent.SetStatusToDraft();
        }
        // Otherwise do nothing.
        return Result.Success();
    }

    public Result UpdateMaxNumberOfGuests(VeaEvent veaEvent, EventMaxGuests maxGuests) {
        veaEvent.SetMaximumNumberOfGuests(maxGuests);
        return Result.Success();
    }

    // Todo: the use case document has no info. Should i return error or success ?
    public Result MakeReady(VeaEvent veaEvent) {
        return Result.Success();
    }

    public Result MakeActive(VeaEvent veaEvent) {
        veaEvent.SetStatusToActive();
        return Result.Success();
    }
}