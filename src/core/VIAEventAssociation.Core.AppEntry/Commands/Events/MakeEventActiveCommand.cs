using VIAEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.AppEntry.Commands.Events;

public class MakeEventActiveCommand  : ICommand{
    public EventId EventId { get; init; }

    private MakeEventActiveCommand(EventId eventId) {
        EventId = eventId;
    }

    public static Result<MakeEventActiveCommand> Create(string eventId) {
        Result<EventId> idResult = EventId.Create(eventId);
        return idResult.WithPayloadIfSuccess(() => new MakeEventActiveCommand(idResult.Payload!));
    }
}