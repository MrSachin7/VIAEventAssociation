using VIAEventAssociation.Core.Domain.Aggregates.Events.Entities.Invitation;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using VIAEventAssociation.Core.Domain.Aggregates.Locations;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Events.state;

internal class CancelledStatusState : IEventStatusState {
    private static readonly EventStatus Status = EventStatus.Cancelled;

    private CancelledStatusState() {

    }

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

    public Result MakeReady(VeaEvent veaEvent, ISystemTime systemTime) {
        return Error.BadRequest(ErrorMessage.CancelledEventCannotBeMadeReady);
    }

    public Result MakeActive(VeaEvent veaEvent, ISystemTime systemTime) {
        return Error.BadRequest(ErrorMessage.CancelledEventCannotBeActivated);
    }

    public Result MakeCancelled(VeaEvent veaEvent) {
        return Error.BadRequest(ErrorMessage.OnlyActiveEventsCanBeCancelled);
    }

    public Result UpdateEventDuration(VeaEvent veaEvent, EventDuration eventDuration) {
        return Error.BadRequest(ErrorMessage.CancelledEventIsUnmodifiable);
    }

    public Result InviteGuest(VeaEvent veaEvent, EventInvitation invitation) {
        return Error.BadRequest(ErrorMessage.InvitationsCanOnlyBeMadeOnReadyOrActiveEvent);
    }

    public Result ParticipateGuest(VeaEvent veaEvent, GuestId guestId, ISystemTime systemTime) {
        return Error.BadRequest(ErrorMessage.OnlyActiveEventsCanBeJoined);
    }

    public Result AcceptInvitation(VeaEvent veaEvent, EventInvitation invitation) {
        return Error.BadRequest(ErrorMessage.OnlyActiveEventsCanBeJoined);

    }

    public Result DeclineInvitation(VeaEvent veaEvent, EventInvitation invitation) {
        return Error.BadRequest(ErrorMessage.EventsCannotBeDeclinedYet);

    }

    public Result UpdateLocation(VeaEvent veaEvent, Location locationId) {
        return Error.BadRequest(ErrorMessage.CancelledEventIsUnmodifiable);
    }
}