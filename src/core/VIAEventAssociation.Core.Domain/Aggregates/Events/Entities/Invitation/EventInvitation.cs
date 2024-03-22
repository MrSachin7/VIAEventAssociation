using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using VIAEventAssociation.Core.Domain.Common.Bases;

namespace VIAEventAssociation.Core.Domain.Aggregates.Events.Entities.Invitation;

public class EventInvitation : Entity<EventInvitationId> {

    internal JoinStatus Status { get; private set; }

    internal Guests.GuestId GuestId { get; private set; }

    private EventInvitation(EventInvitationId id, Guests.GuestId guestId) {
        Id = id;
        GuestId = guestId;
        Status = JoinStatus.Pending;
    }

    public static EventInvitation Create(Guests.GuestId guestId) {
        return new EventInvitation(Invitation.EventInvitationId.New(), guestId);
    }

    internal void Accept() {
        Status = JoinStatus.Accepted;
    }

    internal void Decline() {
        Status = JoinStatus.Declined;
    }
    
}