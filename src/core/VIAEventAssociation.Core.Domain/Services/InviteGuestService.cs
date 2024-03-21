using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Events.Entities.Invitation;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Services;

public class InviteGuestService {
    private readonly IEventRepository _eventRepository;
    private readonly IGuestRepository _guestRepository;


    public InviteGuestService(IGuestRepository guestRepository, IEventRepository eventRepository) {
        _guestRepository = guestRepository;
        _eventRepository = eventRepository;
    }

    public async Task<Result> Handle(EventId eventId, GuestId guestId) {
        VeaEvent? veaEvent = await _eventRepository.FindAsync(eventId);
        Guest? guest = await _guestRepository.FindAsync(guestId);

        if (veaEvent is null) {
            return Error.NotFound(ErrorMessage.EventNotFound(eventId.Value));
        }

        if (guest is null) {
            return Error.NotFound(ErrorMessage.GuestNotFound(eventId.Value));
        }

        EventInvitation invitation = EventInvitation.Create(guestId);
        return veaEvent.InviteGuest(invitation);
    }
}