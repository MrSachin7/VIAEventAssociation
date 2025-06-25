using VIAEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.AppEntry.Commands.Events;

public class UpdateEventTitleCommand  : ICommand{
    public EventId EventId { get; init; }
    public EventTitle EventTitle { get; init; }

    private UpdateEventTitleCommand(EventId eventId, EventTitle eventTitle) {
        EventId = eventId;
        EventTitle = eventTitle;
    }

    public static Result<UpdateEventTitleCommand> Create(string eventId, string eventTitle) {
        Result<EventId> idResult = EventId.Create(eventId);
        Result<EventTitle> titleResult = EventTitle.Create(eventTitle);

        return idResult.Combine(titleResult)
            .WithPayloadIfSuccess(() => new UpdateEventTitleCommand(idResult.Payload!, titleResult.Payload!));

    }
}