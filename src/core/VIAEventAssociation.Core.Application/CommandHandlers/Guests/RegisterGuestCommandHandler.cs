using VIAEventAssociation.Core.AppEntry;
using VIAEventAssociation.Core.AppEntry.Commands.Guests;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using VIAEventAssociation.Core.Domain.Common.UnitOfWork;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Application.CommandHandlers.Guests;

public class RegisterGuestCommandHandler : ICommandHandler<RegisterGuestCommand> {
    private readonly IGuestRepository _guestRepository;
    private readonly IUnitOfWork _unitOfWork;


    public RegisterGuestCommandHandler(IGuestRepository guestRepository, IUnitOfWork unitOfWork) {
        _guestRepository = guestRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(RegisterGuestCommand command) {
        await _guestRepository.AddAsync(command.Guest);
        await _unitOfWork.SaveChangesAsync();
        return Result.Success();
    }
}