using UnitTests.Common.Factories;
using UnitTests.Fakes;
using VIAEventAssociation.Core.AppEntry.Commands.Guests;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Guests.RegisterGuest;

public class RegisterGuestCommandTests {
    [Fact]
    public async Task GivenValidGuest_RegisterGuestCommand_CanBeCreated() {
        // Arrange
        Guest guest = await GuestFactory.GetValidGuest();

        // Act
        Result<RegisterGuestCommand> result =await RegisterGuestCommand.Create(
            guest.FirstName.Value,
            guest.LastName.Value,
            guest.Email.Value,
            guest.ProfilePictureUrl!.Url.ToString(),
            new TestUniqueEmailChecker());

        // Assert
        Assert.True(result.IsSuccess);
    }


    [Fact]
    public async Task GivenInvalidGuest_RegisterGuestCommand_CannotBeCreated() {

        Result<RegisterGuestCommand> result =await RegisterGuestCommand.Create(
            "",
            "",
            "",
            "",
            new TestUniqueEmailChecker());

        // Assert
        Assert.True(result.IsFailure);
    }
}