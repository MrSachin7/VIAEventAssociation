using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Events.state;

internal interface IEventStatusState {
    internal EventStatus CurrentStatus();
    internal Result UpdateTitle(VeaEventWithStatePattern veaEventWithStatePattern, EventTitle title);

    internal Result UpdateDescription(VeaEventWithStatePattern veaEventWithStatePattern,
        EventDescription eventDescription);

    internal Result MakePublic(VeaEventWithStatePattern veaEventWithStatePattern);
    internal Result MakePrivate(VeaEventWithStatePattern veaEventWithStatePattern);

    internal Result UpdateMaxNumberOfGuests(VeaEventWithStatePattern veaEventWithStatePattern,
        EventMaxGuests maxGuests);

    internal Result MakeReady(VeaEventWithStatePattern veaEventWithStatePattern);
    internal Result MakeActive(VeaEventWithStatePattern veaEventWithStatePattern);
}