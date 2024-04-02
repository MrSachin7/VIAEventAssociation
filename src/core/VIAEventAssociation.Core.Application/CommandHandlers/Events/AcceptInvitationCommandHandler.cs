using VIAEventAssociation.Core.AppEntry;
using VIAEventAssociation.Core.AppEntry.Commands.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Events.Entities.Invitation;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using VIAEventAssociation.Core.Domain.Common.UnitOfWork;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Application.CommandHandlers.Events;

public class AcceptInvitationCommandHandler : ICommandHandler<AcceptInvitationCommand> {
    private readonly IEventRepository _eventRepository;
    private readonly IUnitOfWork _unitOfWork;


    public AcceptInvitationCommandHandler(IEventRepository eventRepository,
        IUnitOfWork unitOfWork) {
        _eventRepository = eventRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(AcceptInvitationCommand command) {
        VeaEvent? veaEvent = await _eventRepository.FindAsync(command.EventId);

        if (veaEvent is null) {
            return Error.NotFound(ErrorMessage.EventNotFound(command.EventId.Value));
        }

        Result result = veaEvent.AcceptInvitation(command.InvitationId);
        if (result.IsSuccess) {
            await _unitOfWork.SaveChangesAsync();
        }

        return result;
    }
}