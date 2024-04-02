using VIAEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.AppEntry.Commands.Events;

public class UpdateEventMaxGuestsCommand  : ICommand{
    public EventId EventId { get; init; }

    public EventMaxGuests
        EventMaxGuests { get; init; }

    private UpdateEventMaxGuestsCommand(EventId eventId, EventMaxGuests eventMaxGuests) {
        EventId = eventId;
        EventMaxGuests = eventMaxGuests;
    }


    public static Result<UpdateEventMaxGuestsCommand> Create(string eventId, int maxNumberOfGuests) {
        Result<EventId> idResult = EventId.Create(eventId);
        Result<EventMaxGuests> maxGuestsResult = EventMaxGuests.Create(maxNumberOfGuests);

        return idResult.Combine(maxGuestsResult)
            .WithPayload(() =>
                new UpdateEventMaxGuestsCommand(idResult.Payload!, maxGuestsResult.Payload!));
    }
}