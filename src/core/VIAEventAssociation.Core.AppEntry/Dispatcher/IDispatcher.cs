using VIAEventAssociation.Core.AppEntry.Commands;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.AppEntry.Dispatcher;

public interface IDispatcher {
    Task<Result> DispatchAsync(ICommand command);

}