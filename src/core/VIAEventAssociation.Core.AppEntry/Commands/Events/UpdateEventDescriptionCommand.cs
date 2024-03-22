using VIAEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.AppEntry.Commands.Events;

public class UpdateEventDescriptionCommand {
    public EventId Id { get; init; }
    public EventDescription Description { get; init; }

    private UpdateEventDescriptionCommand(EventId id, EventDescription description) {
        Id = id;
        Description = description;
    }

    public static Result<UpdateEventDescriptionCommand> Create(string eventId, string eventDescription) {
        Result<EventId> idResult = EventId.Create(eventId);
        Result<EventDescription> descriptionResult = EventDescription.Create(eventDescription);

        return idResult.Combine(descriptionResult)
            .WithPayload(() => new UpdateEventDescriptionCommand(idResult.Payload!, descriptionResult.Payload!));
    }
}