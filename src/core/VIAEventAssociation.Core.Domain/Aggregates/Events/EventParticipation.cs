using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using VIAEventAssociation.Core.Domain.Common.Bases;

namespace VIAEventAssociation.Core.Domain.Aggregates.Events;

public class EventParticipation : ValueObject{

    public EventId EventId { get; set; }
    public GuestId GuestId { get; set; }

    public static implicit operator EventParticipation(GuestId guestId) {
        return new EventParticipation (guestId);
    }

    public static implicit operator GuestId(EventParticipation eventParticipation) {
        return eventParticipation.GuestId;
    }

    private EventParticipation(GuestId guestId) {
        GuestId = guestId;
    }

    private EventParticipation() {
        // Efc needs this
    }


    protected override IEnumerable<object> GetEqualityComponents() {
        yield return GuestId;
        yield return EventId;
    }
}