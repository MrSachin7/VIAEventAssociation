using VIAEventAssociation.Core.AppEntry;
using VIAEventAssociation.Core.AppEntry.Commands.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Events.Entities.Invitation;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using VIAEventAssociation.Core.Domain.Common.UnitOfWork;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Application.CommandHandlers.Events;

public class InviteGuestCommandHandler : ICommandHandler<InviteGuestCommand> {

    private readonly IEventRepository _eventRepository;
    private readonly IGuestRepository _guestRepository;
    private readonly IUnitOfWork _unitOfWork;


    public InviteGuestCommandHandler(IEventRepository eventRepository, IGuestRepository guestRepository, IUnitOfWork unitOfWork) {
        _eventRepository = eventRepository;
        _guestRepository = guestRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(InviteGuestCommand command) {
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
        if (result.IsSuccess) {
            await _unitOfWork.SaveChangesAsync();
        }

        return result;
    }
}