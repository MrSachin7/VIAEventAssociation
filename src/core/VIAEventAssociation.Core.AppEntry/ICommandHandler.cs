using ViaEventAssociation.Core.Tools.OperationResult;
using ICommand = VIAEventAssociation.Core.AppEntry.Commands.ICommand;

namespace VIAEventAssociation.Core.AppEntry;

public interface ICommandHandler<in TCommand> where TCommand : ICommand {
    Task<Result> HandleAsync(TCommand command);
    
}