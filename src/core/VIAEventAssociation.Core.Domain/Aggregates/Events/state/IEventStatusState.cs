using VIAEventAssociation.Core.Domain.Aggregates.Events.Entities.Invitation;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using VIAEventAssociation.Core.Domain.Aggregates.Locations;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Events.state;

internal interface IEventStatusState {
    EventStatus CurrentStatus();
    Result UpdateTitle(VeaEvent veaEvent, EventTitle title);

    Result UpdateDescription(VeaEvent veaEvent,
        EventDescription eventDescription);

    Result MakePublic(VeaEvent veaEvent);
    Result MakePrivate(VeaEvent veaEvent);

    Result UpdateMaxNumberOfGuests(VeaEvent veaEvent,
        EventMaxGuests maxGuests);

    Result MakeReady(VeaEvent veaEvent, ISystemTime systemTime);
    Result MakeActive(VeaEvent veaEvent, ISystemTime systemTime);
    Result MakeCancelled(VeaEvent veaEvent);
    Result UpdateEventDuration(VeaEvent veaEvent, EventDuration eventDuration);
    Result InviteGuest(VeaEvent veaEvent, EventInvitation invitation);
    Result ParticipateGuest(VeaEvent veaEvent, GuestId guestId, ISystemTime systemTime);
    Result AcceptInvitation(VeaEvent veaEvent, EventInvitation invitation);
    Result DeclineInvitation(VeaEvent veaEvent, EventInvitation invitation);
    Result UpdateLocation(VeaEvent veaEvent, LocationId locationId);
}