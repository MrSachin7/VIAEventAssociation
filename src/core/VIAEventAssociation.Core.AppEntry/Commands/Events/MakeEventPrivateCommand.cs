using VIAEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.AppEntry.Commands.Events;

public class MakeEventPrivateCommand {
    public EventId Id { get; init; }

    private MakeEventPrivateCommand(EventId id) {
        Id = id;
    }


    public static Result<MakeEventPrivateCommand> Create(string eventId) {
        Result<EventId> idResult = EventId.From(eventId);
        return idResult.WithPayload(() => new MakeEventPrivateCommand(idResult.Payload!));
    }
}