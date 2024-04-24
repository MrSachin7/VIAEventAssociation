using VIAEventAssociation.Core.AppEntry;
using VIAEventAssociation.Core.AppEntry.Commands.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Application.CommandHandlers.Events;

public class UpdateEventTitleCommandHandler : ICommandHandler<UpdateEventTitleCommand> {
    
    private readonly IEventRepository _eventRepository;


    public UpdateEventTitleCommandHandler(IEventRepository eventRepository) {
        _eventRepository = eventRepository;
    }

    public async Task<Result> HandleAsync(UpdateEventTitleCommand command) {
        VeaEvent? veaEvent = await _eventRepository.FindAsync(command.EventId);

        if (veaEvent is null) {
            return Error.NotFound(ErrorMessage.EventNotFound(command.EventId.Value));
        }

        Result result = veaEvent.UpdateEventTitle(command.EventTitle);
        return result;

    }
}