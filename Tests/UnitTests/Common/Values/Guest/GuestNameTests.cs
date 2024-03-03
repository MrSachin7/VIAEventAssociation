using UnitTests.Common.Factories;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Common.Values.Guest;

public class GuestNameTests {
    [Theory]
    [MemberData(nameof(GuestFactory.GetValidFirstName), MemberType = typeof(GuestFactory))]
    public void GivenValidFirstName_CreateGuestFirstName_ReturnsSuccessResult(string firstName) {
        // Arrange and act
        Result<GuestFirstName> result = GuestFirstName.From(firstName);

        // Assert
        Assert.True(result.IsSuccess);
        // Assert that the email is in lower case
        firstName = firstName.Trim();
        string expected = char.ToUpper(firstName[0]) + firstName[1..].ToLower();
        Assert.Equal(expected, result.Payload!.Value);

    }

    [Theory]
    [MemberData(nameof(GuestFactory.GetInValidFirstName), MemberType = typeof(GuestFactory))]
    public void GivenInValidFirstName_CreateGuestFirstName_ReturnsFailureResult(string lastName) {
        // Arrange and act
        Result<GuestFirstName> result = GuestFirstName.From(lastName);

        // Assert
        Assert.True(result.IsFailure);
    }


    [Theory]
    [MemberData(nameof(GuestFactory.GetValidLastName), MemberType = typeof(GuestFactory))]
    public void GivenValidLastName_CreateGuestLastName_ReturnsSuccessResult(string lastName) {
        // Arrange and act
        Result<GuestFirstName> result = GuestFirstName.From(lastName);

        // Assert
        Assert.True(result.IsSuccess);
        // Assert that the email is in lower case
        lastName = lastName.Trim();
        string expected = char.ToUpper(lastName[0]) + lastName[1..].ToLower();
        Assert.Equal(expected, result.Payload!.Value);

    }

    [Theory]
    [MemberData(nameof(GuestFactory.GetInValidLastName), MemberType = typeof(GuestFactory))]
    public void GivenInValidLastName_CreateGuestLastName_ReturnsFailureResult(string lastName) {
        // Arrange and act
        Result<GuestFirstName> result = GuestFirstName.From(lastName);

        // Assert
        Assert.True(result.IsFailure);
    }
}