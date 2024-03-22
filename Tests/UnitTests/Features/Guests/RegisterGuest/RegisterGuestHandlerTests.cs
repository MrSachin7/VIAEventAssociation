using UnitTests.Common.Factories;
using UnitTests.Fakes;
using VIAEventAssociation.Core.AppEntry.Commands.Guests;
using VIAEventAssociation.Core.Application.CommandHandlers.Guests;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using VIAEventAssociation.Core.Domain.Common.UnitOfWork;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Guests.RegisterGuest;

public class RegisterGuestHandlerTests {
    private readonly IUnitOfWork _unitOfWork = new TestUnitOfWork();


    [Fact]
    public async Task RegisterGuestHandler_RegistersGuest() {
        // Arrange

        Guest guest = await GuestFactory.GetValidGuest();

        // Make sure the repo contains the guest 
        TestGuestRepo guestRepo = new TestGuestRepo();

        RegisterGuestCommand registerGuestCommand =
           (await RegisterGuestCommand.Create(
                guest.FirstName.Value,
                guest.LastName.Value,
                guest.Email.Value,
                guest.ProfilePictureUrl!.Url.ToString(),
                new TestUniqueEmailChecker())).Payload!;

        // Act
        RegisterGuestCommandHandler handler = new(guestRepo, _unitOfWork);
        Result result = await handler.Handle(registerGuestCommand);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(guestRepo.Values);
        Assert.Equal(guest.FirstName, guestRepo.Values[0].FirstName);
        Assert.Equal(guest.LastName, guestRepo.Values[0].LastName);
        Assert.Equal(guest.Email, guestRepo.Values[0].Email);
        Assert.Equal(guest.ProfilePictureUrl, guestRepo.Values[0].ProfilePictureUrl);
    }
}