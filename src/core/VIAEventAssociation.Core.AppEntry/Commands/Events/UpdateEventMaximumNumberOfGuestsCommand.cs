using VIAEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.AppEntry.Commands.Events;

public class UpdateEventMaximumNumberOfGuestsCommand  : ICommand{
    public EventId EventId { get; init; }

    public EventMaxGuests
        EventMaxGuests { get; init; }

    private UpdateEventMaximumNumberOfGuestsCommand(EventId eventId, EventMaxGuests eventMaxGuests) {
        EventId = eventId;
        EventMaxGuests = eventMaxGuests;
    }


    public static Result<UpdateEventMaximumNumberOfGuestsCommand> Create(string eventId, int maxNumberOfGuests) {
        Result<EventId> idResult = EventId.Create(eventId);
        Result<EventMaxGuests> maxGuestsResult = EventMaxGuests.Create(maxNumberOfGuests);

        return idResult.Combine(maxGuestsResult)
            .WithPayload(() =>
                new UpdateEventMaximumNumberOfGuestsCommand(idResult.Payload!, maxGuestsResult.Payload!));
    }
}