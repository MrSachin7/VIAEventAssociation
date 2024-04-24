using VIAEventAssociation.Core.AppEntry;
using VIAEventAssociation.Core.AppEntry.Commands.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Application.CommandHandlers.Events;

public class MakeEventReadyCommandHandler : ICommandHandler<MakeEventReadyCommand> {

    private readonly IEventRepository _eventRepository;
    private readonly ISystemTime _systemTime;


    public MakeEventReadyCommandHandler(IEventRepository eventRepository, ISystemTime systemTime) {
        _eventRepository = eventRepository;
        _systemTime = systemTime;
    }

    public async Task<Result> HandleAsync(MakeEventReadyCommand command) {
        VeaEvent? veaEvent = await _eventRepository.FindAsync(command.EventId);
        if (veaEvent is null) {
            return Error.NotFound(ErrorMessage.EventNotFound(command.EventId.Value));
        }

        Result result = veaEvent.MakeReady(_systemTime);
        return result;
    }
}