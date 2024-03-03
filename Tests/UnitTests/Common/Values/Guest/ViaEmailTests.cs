using UnitTests.Common.Factories;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Common.Values.Guest;

public class ViaEmailTests {
    [Theory]
    [MemberData(nameof(GuestFactory.GetValidEmails), MemberType = typeof(GuestFactory))]
    public void GivenValidEmailAddress_CreateVIAEmail_ReturnsSuccessResult(string email) {
        // Arrange and act
        Result<ViaEmail> viaEmail = ViaEmail.From(email);

        // Assert
        Assert.True(viaEmail.IsSuccess);
        // Assert that the email is in lower case
        Assert.Equal(email.ToLower(), viaEmail.Payload!.Value);
    }

    [Theory]
    [MemberData(nameof(GuestFactory.GetInValidEmails), MemberType = typeof(GuestFactory))]
    public void GivenInValidEmailAddress_CreateVIAEmail_ReturnsFailureResult(string email) {
        // Arrange and act
        Result<ViaEmail> viaEmail = ViaEmail.From(email);
        // Assert
        Assert.True(viaEmail.IsFailure);
    }
}