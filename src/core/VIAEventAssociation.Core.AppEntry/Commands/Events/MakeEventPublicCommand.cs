using VIAEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.AppEntry.Commands.Events;

public class MakeEventPublicCommand  : ICommand{
    public EventId EventId { get; init; }


    private MakeEventPublicCommand(EventId eventId) {
        EventId = eventId;
    }


    public static Result<MakeEventPublicCommand> Create(string eventId) {
        Result<EventId> idResult = EventId.Create(eventId);
        return idResult.WithPayload(() => new MakeEventPublicCommand(idResult.Payload!));
    }
}