using VIAEventAssociation.Core.AppEntry;
using VIAEventAssociation.Core.AppEntry.Commands.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Common.UnitOfWork;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Application.CommandHandlers.Events;

public class CreateEventCommandHandler : ICommandHandler<CreateEventCommand> {
    private readonly IEventRepository _eventRepository;
    private readonly IUnitOfWork _unitOfWork;


    public CreateEventCommandHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork) {
        _eventRepository = eventRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(CreateEventCommand command) {
        VeaEvent veaEvent = VeaEvent.Empty();
        await _eventRepository.AddAsync(veaEvent);
        await _unitOfWork.SaveChangesAsync();
        return Result.Success();
    }
}