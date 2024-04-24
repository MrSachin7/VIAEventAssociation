using VIAEventAssociation.Core.AppEntry;
using VIAEventAssociation.Core.AppEntry.Commands.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Application.CommandHandlers.Events;

public class UpdateEventMaxGuestsCommandHandler : ICommandHandler<UpdateEventMaxGuestsCommand> {

    private readonly IEventRepository _eventRepository;


    public UpdateEventMaxGuestsCommandHandler(IEventRepository eventRepository) {
        _eventRepository = eventRepository;
    }

    public async Task<Result> HandleAsync(UpdateEventMaxGuestsCommand command) {
        VeaEvent? veaEvent = await _eventRepository.FindAsync(command.EventId);

        if (veaEvent is null) {
            return Error.NotFound(ErrorMessage.EventNotFound(command.EventId.Value));
        }

        Result result = veaEvent.UpdateMaximumNumberOfGuests(command.EventMaxGuests);
        return result;
    }
}