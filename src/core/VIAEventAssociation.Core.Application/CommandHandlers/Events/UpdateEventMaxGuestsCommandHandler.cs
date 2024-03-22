using VIAEventAssociation.Core.AppEntry;
using VIAEventAssociation.Core.AppEntry.Commands.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Common.UnitOfWork;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Application.CommandHandlers.Events;

public class UpdateEventMaxGuestsCommandHandler : ICommandHandler<UpdateEventMaxGuestsCommand> {

    private readonly IEventRepository _eventRepository;
    private readonly IUnitOfWork _unitOfWork;


    public UpdateEventMaxGuestsCommandHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork) {
        _eventRepository = eventRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateEventMaxGuestsCommand command) {
        VeaEvent? veaEvent = await _eventRepository.FindAsync(command.EventId);

        if (veaEvent is null) {
            return Error.NotFound(ErrorMessage.EventNotFound(command.EventId.Value));
        }

        Result result = veaEvent.UpdateMaximumNumberOfGuests(command.EventMaxGuests);
        if (result.IsSuccess) {
            await _unitOfWork.SaveChangesAsync();
        }

        return result;
    }
}