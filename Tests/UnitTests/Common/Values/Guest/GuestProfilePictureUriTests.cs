using UnitTests.Common.Factories;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Common.Values.Guest;

public class GuestProfilePictureUriTests {
    [Theory]
    [MemberData(nameof(GuestFactory.GetValidProfileUri), MemberType = typeof(GuestFactory))]
    public void GivenValidProfileUri_CreateGuestProfileUri_ReturnsSuccessResult(string profileUri) {
        // Arrange and act
        Result<ProfilePictureUrl> result = ProfilePictureUrl.Create(profileUri);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(profileUri, result.Payload!.Url.OriginalString);
    }

    [Theory]
    [MemberData(nameof(GuestFactory.GetInvalidProfileUri), MemberType = typeof(GuestFactory))]
    public void GivenInvalidProfileUri_CreateGuestProfileUri_ReturnsFailureResult(string profileUri) {
        // Arrange and act
        Result<ProfilePictureUrl> result = ProfilePictureUrl.Create(profileUri);

        // Assert
        Assert.True(result.IsFailure);
    }
    
}