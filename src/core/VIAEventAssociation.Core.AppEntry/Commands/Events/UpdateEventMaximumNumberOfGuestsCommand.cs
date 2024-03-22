using VIAEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.AppEntry.Commands.Events;

public class UpdateEventMaximumNumberOfGuestsCommand {
    public EventId Id { get; init; }

    public EventMaxGuests
        MaxGuests { get; init; }

    private UpdateEventMaximumNumberOfGuestsCommand(EventId id, EventMaxGuests maxGuests) {
        Id = id;
        MaxGuests = maxGuests;
    }


    public static Result<UpdateEventMaximumNumberOfGuestsCommand> Create(string eventId, int maxNumberOfGuests) {
        Result<EventId> idResult = EventId.Create(eventId);
        Result<EventMaxGuests> maxGuestsResult = EventMaxGuests.Create(maxNumberOfGuests);

        return idResult.Combine(maxGuestsResult)
            .WithPayload(() =>
                new UpdateEventMaximumNumberOfGuestsCommand(idResult.Payload!, maxGuestsResult.Payload!));
    }
}