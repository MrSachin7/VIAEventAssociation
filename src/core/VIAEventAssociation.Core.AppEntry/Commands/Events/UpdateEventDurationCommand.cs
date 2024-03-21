// using VIAEventAssociation.Core.Domain.Aggregates.Events;
// using ViaEventAssociation.Core.Tools.OperationResult;
//
// namespace VIAEventAssociation.Core.AppEntry.Commands.Events;
//
// public class UpdateEventDurationCommand {
//     public EventId Id { get; init; }
//     public EventDuration Duration { get; init; }
//
//     private UpdateEventDurationCommand(EventId id, EventDuration duration) {
//         Id = id;
//         Duration = duration;
//     }
//
//     public static Result<UpdateEventDurationCommand> Create(string eventId, string startDateTime, string endDateTime) {
//         Result<EventId> idResult = EventId.From(eventId);
//         Result<EventDuration> durationResult = EventDuration.Create(eventDuration);
//
//         return idResult.Combine(durationResult)
//             .WithPayload(() => new UpdateEventDurationCommand(idResult.Payload!, durationResult.Payload!));
//     }
//
//     public static Result<UpdateEventDurationCommand> Create(string eventId, DateTime startDateTime, DateTime endDateTime) {
//         Result<EventId> idResult = EventId.From(eventId);
//         Result<EventDuration> durationResult = EventDuration.Create(startDateTime, endDateTime);
//
//         return idResult.Combine(durationResult)
//             .WithPayload(() => new UpdateEventDurationCommand(idResult.Payload!, durationResult.Payload!));
//     }
// }