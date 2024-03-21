using VIAEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.AppEntry.Commands.Events;

public class MakeEventPublicCommand {
    public EventId Id { get; init; }


    private MakeEventPublicCommand(EventId id) {
        Id = id;
    }


    // Todo: Where should i check if the event exists in the database ??
    public static Result<MakeEventPublicCommand> Create(string eventId) {
        Result<EventId> idResult = EventId.From(eventId);
        return idResult.WithPayload(() => new MakeEventPublicCommand(idResult.Payload!));
    }
}