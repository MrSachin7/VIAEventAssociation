using VIAEventAssociation.Core.AppEntry.Commands;
using VIAEventAssociation.Core.Domain.Common.UnitOfWork;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.AppEntry.Dispatcher.Decorators;

// Todo: Ask troels...
public class TransactionDecorator : IDispatcher {
    private readonly IDispatcher _commandDispatcher;
    private readonly IUnitOfWork _unitOfWork;

    public TransactionDecorator(IDispatcher commandDispatcher, IUnitOfWork unitOfWork ) {
        this._unitOfWork = unitOfWork;
        this._commandDispatcher = commandDispatcher;
    }

    public async Task<Result> DispatchAsync(ICommand command) {
        Result result = await _commandDispatcher.DispatchAsync(command);
        if (result.IsSuccess) {
            await _unitOfWork.SaveChangesAsync();
        }
        return result;

    }
}