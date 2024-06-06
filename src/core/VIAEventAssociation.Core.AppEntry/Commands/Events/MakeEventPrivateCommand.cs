using VIAEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.AppEntry.Commands.Events;

public class MakeEventPrivateCommand  : ICommand{
    public EventId EventId { get; init; }

    private MakeEventPrivateCommand(EventId eventId) {
        EventId = eventId;
    }


    public static Result<MakeEventPrivateCommand> Create(string eventId) {
        Result<EventId> idResult = EventId.Create(eventId);
        return idResult.WithPayloadIfSuccess(() => new MakeEventPrivateCommand(idResult.Payload!));
    }
}