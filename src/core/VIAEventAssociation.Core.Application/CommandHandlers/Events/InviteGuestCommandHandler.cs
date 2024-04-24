using VIAEventAssociation.Core.AppEntry;
using VIAEventAssociation.Core.AppEntry.Commands.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Events.Entities.Invitation;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Application.CommandHandlers.Events;

public class InviteGuestCommandHandler : ICommandHandler<InviteGuestCommand> {

    private readonly IEventRepository _eventRepository;
    private readonly IGuestRepository _guestRepository;


    public InviteGuestCommandHandler(IEventRepository eventRepository, IGuestRepository guestRepository) {
        _eventRepository = eventRepository;
        _guestRepository = guestRepository;
    }

    public async Task<Result> HandleAsync(InviteGuestCommand command) {
        VeaEvent? veaEvent = await _eventRepository.FindAsync(command.EventId);
        if (veaEvent is null) {
            return Error.NotFound(ErrorMessage.EventNotFound(command.EventId.Value));
        }

        Guest? guest = await _guestRepository.FindAsync(command.GuestId);

        if (guest is null) {
            return Error.NotFound(ErrorMessage.GuestNotFound(command.GuestId.Value));
        }

        EventInvitation eventInvitation = EventInvitation.Create(guest.Id);

        Result result = veaEvent.InviteGuest(eventInvitation);

        return result;
    }
}