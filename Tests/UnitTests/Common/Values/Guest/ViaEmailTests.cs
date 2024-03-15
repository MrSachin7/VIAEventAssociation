using UnitTests.Common.Factories;
using UnitTests.Fakes;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using VIAEventAssociation.Core.Domain.temp;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Common.Values.Guest;

public class ViaEmailTests {
    private readonly TestUniqueEmailChecker _uniqueEmailChecker = new ();

    [Theory]
    [MemberData(nameof(GuestFactory.GetValidEmails), MemberType = typeof(GuestFactory))]
    public async Task GivenValidEmailAddress_CreateVIAEmail_ReturnsSuccessResult(string email) {
        // Arrange and act
        Result<ViaEmail> viaEmail =await ViaEmail.Create(email, _uniqueEmailChecker);

        // Assert
        Assert.True(viaEmail.IsSuccess);
        // Assert that the email is in lower case
        Assert.Equal(email.ToLower(), viaEmail.Payload!.Value);
    }

    [Theory]
    [MemberData(nameof(GuestFactory.GetInValidEmails), MemberType = typeof(GuestFactory))]
    public async Task GivenInValidEmailAddress_CreateVIAEmail_ReturnsFailureResult(string email) {
        // Arrange and act

        Result<ViaEmail> viaEmail =await ViaEmail.Create(email, _uniqueEmailChecker);
        // Assert
        Assert.True(viaEmail.IsFailure);
    }

    [Theory]
    [MemberData(nameof(GuestFactory.GetValidEmails), MemberType = typeof(GuestFactory))]

    public async Task GivenValidEmailAddress_AndEmailAlreadyExists_CreateVIAEmail_ReturnsFailureResult(string email) {
        // Arrange
        _uniqueEmailChecker.InitialValue = false;

        // Act
        Result<ViaEmail> viaEmail =await ViaEmail.Create(email, _uniqueEmailChecker);

        // Assert
        Assert.True(viaEmail.IsFailure);
        Assert.Contains(ErrorMessage.EmailAlreadyAssociatedWithAnotherGuest, viaEmail.Error!.Messages);
    }


}