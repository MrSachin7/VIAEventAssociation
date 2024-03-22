using VIAEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.AppEntry.Commands.Events;

public class UpdateEventTitleCommand {
    public EventId Id { get; init; }
    public EventTitle Title { get; init; }

    private UpdateEventTitleCommand(EventId id, EventTitle title) {
        Id = id;
        Title = title;
    }

    public static Result<UpdateEventTitleCommand> Create(string eventId, string eventTitle) {
        Result<EventId> idResult = EventId.Create(eventId);
        Result<EventTitle> titleResult = EventTitle.Create(eventTitle);

        return idResult.Combine(titleResult)
            .WithPayload(() => new UpdateEventTitleCommand(idResult.Payload!, titleResult.Payload!));

    }
}