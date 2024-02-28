using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Events.state;

internal interface IEventStatusState {
    internal EventStatus CurrentStatus();
    internal Result UpdateTitle(VeaEvent veaEvent, EventTitle title);

    internal Result UpdateDescription(VeaEvent veaEvent,
        EventDescription eventDescription);

    internal Result MakePublic(VeaEvent veaEvent);
    internal Result MakePrivate(VeaEvent veaEvent);

    internal Result UpdateMaxNumberOfGuests(VeaEvent veaEvent,
        EventMaxGuests maxGuests);

    internal Result MakeReady(VeaEvent veaEvent);
    internal Result MakeActive(VeaEvent veaEvent);
}