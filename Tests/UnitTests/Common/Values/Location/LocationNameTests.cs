using UnitTests.Common.Factories;
using VIAEventAssociation.Core.Domain.Aggregates.Locations;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Common.Values.Location;

public class LocationNameTests {

    [Theory]
    [MemberData(nameof(LocationFactory.GetValidLocationNames), MemberType = typeof(LocationFactory))]

    public void GivenValidLocationName_CreateLocationName_ReturnsSuccessResult(string locationName) {
        // Arrange and act
        Result<LocationName> result = LocationName.Create(locationName);

        // Assert
        Assert.True(result.IsSuccess);
        // Assert that the email is in lower case
        Assert.NotNull(result.Payload!.Value);

    }


    [Theory]
    [MemberData(nameof(LocationFactory.GetInValidLocationNames), MemberType = typeof(LocationFactory))]

    public void GivenInValidLocationName_CreateLocationName_ReturnsFailureResult(string locationName) {
        // Arrange and act
        Result<LocationName> result = LocationName.Create(locationName);

        // Assert
        Assert.True(result.IsFailure);
        // Assert that the email is in lower case

    }
    
}