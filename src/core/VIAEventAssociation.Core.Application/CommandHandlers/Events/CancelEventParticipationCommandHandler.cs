using VIAEventAssociation.Core.AppEntry;
using VIAEventAssociation.Core.AppEntry.Commands.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Application.CommandHandlers.Events;

public class CancelEventParticipationCommandHandler : ICommandHandler<CancelEventParticipationCommand> {
    private readonly IEventRepository _eventRepository;
    private readonly IGuestRepository _guestRepository;
    private readonly ISystemTime _systemTime;


    public CancelEventParticipationCommandHandler(IEventRepository eventRepository, IGuestRepository guestRepository, ISystemTime systemTime) {
        _eventRepository = eventRepository;
        _guestRepository = guestRepository;
        _systemTime = systemTime;
    }

    public async Task<Result> HandleAsync(CancelEventParticipationCommand command) {
        VeaEvent? veaEvent = await _eventRepository.FindAsync(command.EventId);
        if (veaEvent is null) {
            return Error.NotFound(ErrorMessage.EventNotFound(command.EventId.Value));
        }

        Guest? guest = await _guestRepository.FindAsync(command.GuestId);

        if (guest is null) {
            return Error.NotFound(ErrorMessage.GuestNotFound(command.GuestId.Value));
            
        }

        Result result = veaEvent.CancelGuestParticipation(guest, _systemTime);
        return result;
    }
}