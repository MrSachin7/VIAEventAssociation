using VIAEventAssociation.Core.AppEntry;
using VIAEventAssociation.Core.AppEntry.Commands.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Common.UnitOfWork;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Application.CommandHandlers.Events;

public class MakeEventReadyCommandHandler : ICommandHandler<MakeEventReadyCommand> {

    private readonly IEventRepository _eventRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISystemTime _systemTime;


    public MakeEventReadyCommandHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork, ISystemTime systemTime) {
        _eventRepository = eventRepository;
        _unitOfWork = unitOfWork;
        _systemTime = systemTime;
    }

    public async Task<Result> Handle(MakeEventReadyCommand command) {
        VeaEvent? veaEvent = await _eventRepository.FindAsync(command.EventId);
        if (veaEvent is null) {
            return Error.NotFound(ErrorMessage.EventNotFound(command.EventId.Value));
        }

        Result result = veaEvent.MakeReady(_systemTime);
        if (result.IsSuccess) {
            await _unitOfWork.SaveChangesAsync();
        }

        return result;
    }
}