using VIAEventAssociation.Core.Domain.Aggregates.Events.Entities.Invitation;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using VIAEventAssociation.Core.Domain.Aggregates.Locations;
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
        if (veaEvent.Visibility.Equals(EventVisibility.Public)) {
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

    public Result MakeCancelled(VeaEvent veaEvent) {
        return Error.BadRequest(ErrorMessage.OnlyActiveEventsCanBeCancelled);
    }

    public Result UpdateEventDuration(VeaEvent veaEvent, EventDuration eventDuration) {
        veaEvent.SetEventDuration(eventDuration);
        veaEvent.SetStatusToDraft();
        return Result.Success();
    }

    public Result InviteGuest(VeaEvent veaEvent, EventInvitation invitation) {
        veaEvent.AddInvitation(invitation);
        return Result.Success();
    }

    public Result ParticipateGuest(VeaEvent veaEvent, GuestId guestId) {
        return Error.BadRequest(ErrorMessage.OnlyActiveEventsCanBeJoined);

    }

    public Result AcceptInvitation(VeaEvent veaEvent, EventInvitationId invitationId) {
        return Error.BadRequest(ErrorMessage.OnlyActiveEventsCanBeJoined);
    }

    public Result DeclineInvitation(VeaEvent veaEvent, EventInvitationId invitationId) {
        return Error.BadRequest(ErrorMessage.EventsCannotBeDeclinedYet);
    }

    public Result UpdateLocation(VeaEvent veaEvent, LocationId locationId) {
        veaEvent.SetLocation(locationId);
        veaEvent.SetStatusToDraft();
        return Result.Success();
    }
}