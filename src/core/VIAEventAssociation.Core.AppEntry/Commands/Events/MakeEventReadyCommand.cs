using VIAEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.AppEntry.Commands.Events;

public class MakeEventReadyCommand {
    public EventId Id { get; set; }

    private MakeEventReadyCommand(EventId id) {
        Id = id;
    }


    public static Result<MakeEventReadyCommand> Create(string eventId) {
        Result<EventId> idResult = EventId.From(eventId);
        return idResult.WithPayload(() => new MakeEventReadyCommand(idResult.Payload!));
    }
}