using UnitTests.Common.Factories;
using VIAEventAssociation.Core.Domain.Aggregates.Locations;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Common.Values.Location;

public class LocationMaxGuestsTests {
    
    [Theory]
    [MemberData(nameof(LocationFactory.GetValidLocationMaxGuests), MemberType = typeof(LocationFactory))]
    public void GivenValidMaxGuests_LocationMaxGuests_CanBeCreated(int validMaxGuests) {
        Result<LocationMaxGuests> result = LocationMaxGuests.Create(validMaxGuests);

        Assert.True(result.IsSuccess);
        Assert.Equal(validMaxGuests, result.Payload!.Value);
    }

    [Theory]
    [MemberData(nameof(LocationFactory.GetInvalidLocationMaxGuests), MemberType = typeof(LocationFactory))]
    public void GivenInValidMaxGuests_LocationMaxGuests_CannotBeCreated(int validMaxGuests) {
        Result<LocationMaxGuests> result = LocationMaxGuests.Create(validMaxGuests);

        Assert.True(result.IsFailure);
    }
}