using VIAEventAssociation.Core.AppEntry;
using VIAEventAssociation.Core.AppEntry.Commands.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Application.CommandHandlers.Events;

public class CreateEventCommandHandler : ICommandHandler<CreateEventCommand> {
    private readonly IEventRepository _eventRepository;


    public CreateEventCommandHandler(IEventRepository eventRepository) {
        _eventRepository = eventRepository;
    }

    public async Task<Result> HandleAsync(CreateEventCommand command) {
        VeaEvent veaEvent = VeaEvent.Empty();
        await _eventRepository.AddAsync(veaEvent);
        return Result.Success();
    }
}