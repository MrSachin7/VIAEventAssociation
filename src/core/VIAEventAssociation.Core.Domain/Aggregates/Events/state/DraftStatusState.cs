using VIAEventAssociation.Core.Domain.Aggregates.Events.Entities.Invitation;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using VIAEventAssociation.Core.Domain.Aggregates.Locations;
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

    public Result MakeCancelled(VeaEvent veaEvent) {
        return Error.BadRequest(ErrorMessage.OnlyActiveEventsCanBeCancelled);

    }

    public Result UpdateEventDuration(VeaEvent veaEvent, EventDuration eventDuration) {
        veaEvent.SetEventDuration(eventDuration);
        return Result.Success();
    }

    public Result InviteGuest(VeaEvent veaEvent, EventInvitation invitation) {
        return Error.BadRequest(ErrorMessage.InvitationsCanOnlyBeMadeOnReadyOrActiveEvent);

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
        return Result.Success();
    }
}