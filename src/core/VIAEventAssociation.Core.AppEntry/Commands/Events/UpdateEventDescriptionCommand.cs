using VIAEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.AppEntry.Commands.Events;

public class UpdateEventDescriptionCommand  : ICommand{
    public EventId EventId { get; init; }
    public EventDescription EventDescription { get; init; }

    private UpdateEventDescriptionCommand(EventId eventId, EventDescription eventDescription) {
        EventId = eventId;
        EventDescription = eventDescription;
    }

    public static Result<UpdateEventDescriptionCommand> Create(string eventId, string eventDescription) {
        Result<EventId> idResult = EventId.Create(eventId);
        Result<EventDescription> descriptionResult = EventDescription.Create(eventDescription);

        return idResult.Combine(descriptionResult)
            .WithPayloadIfSuccess(() => new UpdateEventDescriptionCommand(idResult.Payload!, descriptionResult.Payload!));
    }
}