using VIAEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.AppEntry.Commands.Events;

public class UpdateEventDurationCommand  : ICommand{
    public EventId EventId { get; init; }
    public EventDuration EventDuration { get; init; }

    private UpdateEventDurationCommand(EventId eventId, EventDuration eventDuration) {
        EventId = eventId;
        EventDuration = eventDuration;
    }

    public static Result<UpdateEventDurationCommand> Create(string eventId, DateTime startDateTime, DateTime endDateTime, ISystemTime systemTime) {
        Result<EventId> idResult = EventId.Create(eventId);
        Result<EventDuration> durationResult = EventDuration.Create(startDateTime, endDateTime, systemTime);

        return idResult.Combine(durationResult)
            .WithPayloadIfSuccess(() => new UpdateEventDurationCommand(idResult.Payload!, durationResult.Payload!));
    }
}