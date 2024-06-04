using VIAEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.AppEntry.Commands.Events;

public class CreateEventCommand  : ICommand{
    public EventId EventId { get; set; }
    private CreateEventCommand() {
        EventId = EventId.New();
    }

    public static CreateEventCommand Create() {
        return new CreateEventCommand();
    }
}