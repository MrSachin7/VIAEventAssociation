using VIAEventAssociation.Core.AppEntry.Commands;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.AppEntry.Dispatcher;

public class CommandDispatcher : IDispatcher {

    private readonly IServiceProvider _serviceProvider;

    public CommandDispatcher(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
    }

    // FromGuid Copilot
    public Task<Result> DispatchAsync(ICommand command) {
        Type commandHandlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
        dynamic commandHandler = _serviceProvider.GetService(commandHandlerType) ?? throw 
                new InvalidOperationException("Handler not found");
        return commandHandler.HandleAsync((dynamic)command);
    }
}