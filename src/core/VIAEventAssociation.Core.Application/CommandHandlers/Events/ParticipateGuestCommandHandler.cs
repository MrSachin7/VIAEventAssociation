using VIAEventAssociation.Core.AppEntry;
using VIAEventAssociation.Core.AppEntry.Commands.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using VIAEventAssociation.Core.Domain.Common.UnitOfWork;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Application.CommandHandlers.Events;

public class ParticipateGuestCommandHandler : ICommandHandler<ParticipateGuestCommand> {
    private readonly IEventRepository _eventRepository;
    private readonly IGuestRepository _guestRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISystemTime _systemTime;


    public ParticipateGuestCommandHandler(IEventRepository eventRepository, IGuestRepository guestRepository,
        IUnitOfWork unitOfWork, ISystemTime systemTime) {
        _eventRepository = eventRepository;
        _guestRepository = guestRepository;
        _unitOfWork = unitOfWork;
        _systemTime = systemTime;
    }

    public async Task<Result> Handle(ParticipateGuestCommand command) {
        VeaEvent? veaEvent = await _eventRepository.FindAsync(command.EventId);
        if (veaEvent is null) {
            return Error.NotFound(ErrorMessage.EventNotFound(command.EventId.Value));
        }

        Guest? guest = await _guestRepository.FindAsync(command.GuestId);
        if (guest is null) {
            return Error.NotFound(ErrorMessage.GuestNotFound(command.GuestId.Value));
        }

        Result result = veaEvent.ParticipateGuest(guest, _systemTime);
        if (result.IsSuccess) {
            await _unitOfWork.SaveChangesAsync();
        }

        return result;
    }
}