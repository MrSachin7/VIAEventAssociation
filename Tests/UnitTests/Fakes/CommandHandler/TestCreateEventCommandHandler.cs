using VIAEventAssociation.Core.AppEntry;
using VIAEventAssociation.Core.AppEntry.Commands.Events;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Fakes.CommandHandler;

public class TestCreateEventCommandHandler : ICommandHandler<CreateEventCommand> {
    public int CallCount { get; private set; } = 0;

    public Task<Result> HandleAsync(CreateEventCommand command) {
        CallCount++;
        return Task.FromResult(Result.Success());
    }
}