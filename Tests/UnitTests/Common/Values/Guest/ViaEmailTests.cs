using UnitTests.Common.Factories;
using UnitTests.Fakes;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using VIAEventAssociation.Core.Domain.Contracts;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Common.Values.Guest;

public class ViaEmailTests {

    private readonly TestUniqueEmailChecker _uniqueEmailChecker = new TestUniqueEmailChecker();

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

        Result<ViaEmail> viaEmail = await ViaEmail.Create(email, _uniqueEmailChecker);
        // Assert
        Assert.True(viaEmail.IsFailure);
    }

    [Theory]
    [MemberData(nameof(GuestFactory.GetValidEmails), MemberType = typeof(GuestFactory))]
    public async Task GivenValidEmailAddress_AndItAlreadyExists_CreateVIAEmail_ReturnsFailureResult(string email) {
        // Arrange and act
        _uniqueEmailChecker.Value = false;
        Result<ViaEmail> result =await ViaEmail.Create(email, _uniqueEmailChecker);

        // Assert
        Assert.True(result.IsFailure);
        // Assert that the email is in lower case
        Assert.Contains(ErrorMessage.EmailAlreadyAssociatedWithAnotherGuest, result.Error!.Messages);
    }

}