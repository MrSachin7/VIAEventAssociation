using VIAEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.AppEntry.Commands.Events;

public class UpdateEventDurationCommand {
    public EventId Id { get; init; }
    public EventDuration Duration { get; init; }

    private UpdateEventDurationCommand(EventId id, EventDuration duration) {
        Id = id;
        Duration = duration;
    }

    public static Result<UpdateEventDurationCommand> Create(string eventId, DateTime startDateTime, DateTime endDateTime, ISystemTime systemTime) {
        Result<EventId> idResult = EventId.Create(eventId);
        Result<EventDuration> durationResult = EventDuration.Create(startDateTime, endDateTime, systemTime);

        return idResult.Combine(durationResult)
            .WithPayload(() => new UpdateEventDurationCommand(idResult.Payload!, durationResult.Payload!));
    }
}