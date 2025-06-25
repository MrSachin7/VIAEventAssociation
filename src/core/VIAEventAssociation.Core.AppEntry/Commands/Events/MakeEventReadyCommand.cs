using VIAEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.AppEntry.Commands.Events;

public class MakeEventReadyCommand  : ICommand{
    public EventId EventId { get; set; }

    private MakeEventReadyCommand(EventId eventId) {
        EventId = eventId;
    }


    public static Result<MakeEventReadyCommand> Create(string eventId) {
        Result<EventId> idResult = EventId.Create(eventId);
        return idResult.WithPayloadIfSuccess(() => new MakeEventReadyCommand(idResult.Payload!));
    }
}