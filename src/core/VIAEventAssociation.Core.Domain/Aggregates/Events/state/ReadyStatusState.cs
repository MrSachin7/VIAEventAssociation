﻿using VIAEventAssociation.Core.Domain.Aggregates.Events.Entities.Invitation;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using VIAEventAssociation.Core.Domain.Aggregates.Locations;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Events.state;

internal class ReadyStatusState : IEventStatusState {
    private static readonly EventStatus Status = EventStatus.Ready;

    private ReadyStatusState() {

    }

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

    public Result MakeReady(VeaEvent veaEvent, ISystemTime systemTime) {
        return Result.Success();
    }

    public Result MakeActive(VeaEvent veaEvent, ISystemTime systemTime) {
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

    public Result ParticipateGuest(VeaEvent veaEvent, GuestId guestId, ISystemTime systemTime) {
        return Error.BadRequest(ErrorMessage.OnlyActiveEventsCanBeJoined);

    }

    public Result AcceptInvitation(VeaEvent veaEvent, EventInvitation invitation) {
        return Error.BadRequest(ErrorMessage.OnlyActiveEventsCanBeJoined);
    }

    public Result DeclineInvitation(VeaEvent veaEvent, EventInvitation invitationId) {
        return Error.BadRequest(ErrorMessage.EventsCannotBeDeclinedYet);
    }

    public Result UpdateLocation(VeaEvent veaEvent, Location location) {
        veaEvent.SetLocation(location);
        veaEvent.SetStatusToDraft();
        return Result.Success();
    }
}