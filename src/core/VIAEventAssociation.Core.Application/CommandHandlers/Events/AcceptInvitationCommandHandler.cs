using VIAEventAssociation.Core.AppEntry;
using VIAEventAssociation.Core.AppEntry.Commands.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Application.CommandHandlers.Events;

public class AcceptInvitationCommandHandler : ICommandHandler<AcceptInvitationCommand> {
    private readonly IEventRepository _eventRepository;


    public AcceptInvitationCommandHandler(IEventRepository eventRepository) {
        _eventRepository = eventRepository;
    }

    public async Task<Result> HandleAsync(AcceptInvitationCommand command) {
        VeaEvent? veaEvent = await _eventRepository.FindAsync(command.EventId);

        if (veaEvent is null) {
            return Error.NotFound(ErrorMessage.EventNotFound(command.EventId.Value));
        }

        return veaEvent.AcceptInvitation(command.InvitationId);
    }
}