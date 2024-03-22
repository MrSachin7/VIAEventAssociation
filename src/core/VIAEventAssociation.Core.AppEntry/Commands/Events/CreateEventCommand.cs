using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.AppEntry.Commands.Events;

public class CreateEventCommand  : ICommand{
    private CreateEventCommand() {
    }

    public static Result<CreateEventCommand> Create() {
        return new CreateEventCommand();
    }
}