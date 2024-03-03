using UnitTests.Common.Factories;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Common.Values.Event;

public class EventMaxGuestsTests {
    [Theory]
    [MemberData(nameof(EventFactory.GetValidEventMaxGuests), MemberType = typeof(EventFactory))]
    public void GivenValidMaxGuests_EventMaxGuests_CanBeCreated(int validMaxGuests) {
        Result<EventMaxGuests> result = EventMaxGuests.From(validMaxGuests);

        Assert.True(result.IsSuccess);
        Assert.Equal(validMaxGuests, result.Payload!.Value);
    }

    [Theory]
    [MemberData(nameof(EventFactory.GetInValidEventMaxGuests), MemberType = typeof(EventFactory))]
    public void GivenInValidMaxGuests_EventMaxGuests_CannotBeCreated(int validMaxGuests) {
        Result<EventMaxGuests> result = EventMaxGuests.From(validMaxGuests);

        Assert.True(result.IsFailure);
    }
}