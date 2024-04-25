using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using VIAEventAssociation.Core.Domain.Common.Bases;

namespace VIAEventAssociation.Core.Domain.Aggregates.Events;

public class EventToGuest : ValueObject{

    public EventId EventId { get; set; }
    public GuestId GuestId { get; set; }

    public static implicit operator EventToGuest(GuestId guestId) {
        return new EventToGuest (guestId);
    }

    public static implicit operator GuestId(EventToGuest eventToGuest) {
        return eventToGuest.GuestId;
    }

    private EventToGuest(GuestId guestId) {
        GuestId = guestId;
    }

    private EventToGuest() {
        // Efc needs this
    }


    protected override IEnumerable<object> GetEqualityComponents() {
        yield return GuestId;
        yield return EventId;
    }
}