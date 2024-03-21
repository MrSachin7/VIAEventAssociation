using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Services;

public class ParticipateGuestService {
    private readonly IGuestRepository _guestRepository;
    private readonly IEventRepository _eventRepository;

    public ParticipateGuestService(IGuestRepository guestRepository, IEventRepository eventRepository) {
        _guestRepository = guestRepository;
        _eventRepository = eventRepository;
    }

    public async Task<Result> Handle(EventId eventId, GuestId guestId, ISystemTime systemTime) {
        VeaEvent? veaEvent = await _eventRepository.FindAsync(eventId);
        Guest? guest = await _guestRepository.FindAsync(guestId);

        if (veaEvent is null) {
            return Error.NotFound(ErrorMessage.EventNotFound(eventId.Value));
        }

        if (guest is null) {
            return Error.NotFound(ErrorMessage.GuestNotFound(eventId.Value));
        }

        return veaEvent.ParticipateGuest(guest, systemTime);
    }
}