using VIAEventAssociation.Core.Domain.Aggregates.Events.Entities.Invitation;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Events.state;

internal interface IEventStatusState {
    internal EventStatus CurrentStatus();
    internal Result UpdateTitle(VeaEvent veaEvent, EventTitle title);

    internal Result UpdateDescription(VeaEvent veaEvent,
        EventDescription eventDescription);

    internal Result MakePublic(VeaEvent veaEvent);
    internal Result MakePrivate(VeaEvent veaEvent);

    internal Result UpdateMaxNumberOfGuests(VeaEvent veaEvent,
        EventMaxGuests maxGuests);

    internal Result MakeReady(VeaEvent veaEvent);
    internal Result MakeActive(VeaEvent veaEvent);
    internal Result MakeCancelled(VeaEvent veaEvent);
    internal Result UpdateEventDuration(VeaEvent veaEvent, EventDuration eventDuration);
    Result InviteGuest(VeaEvent veaEvent, EventInvitation invitation);
    Result ParticipateGuest(VeaEvent veaEvent, GuestId guestId);
    Result AcceptInvitation(VeaEvent veaEvent, EventInvitationId invitationId);
    Result DeclineInvitation(VeaEvent veaEvent, EventInvitationId invitationId);
}