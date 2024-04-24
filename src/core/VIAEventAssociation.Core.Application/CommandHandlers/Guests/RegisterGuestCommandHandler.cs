using VIAEventAssociation.Core.AppEntry;
using VIAEventAssociation.Core.AppEntry.Commands.Guests;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Application.CommandHandlers.Guests;

public class RegisterGuestCommandHandler : ICommandHandler<RegisterGuestCommand> {
    private readonly IGuestRepository _guestRepository;


    public RegisterGuestCommandHandler(IGuestRepository guestRepository) {
        _guestRepository = guestRepository;
    }

    public async Task<Result> HandleAsync(RegisterGuestCommand command) {
        await _guestRepository.AddAsync(command.Guest);
        return Result.Success();
    }
}