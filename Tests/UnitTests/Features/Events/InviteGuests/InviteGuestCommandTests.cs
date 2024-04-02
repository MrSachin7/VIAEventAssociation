using UnitTests.Common.Factories;
using VIAEventAssociation.Core.AppEntry.Commands.Events;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Events.InviteGuests;

public class InviteGuestCommandTests {

    [Fact]
    public void GivenValidEventIdAndGuestId_InviteGuestCommand_CanBeCreated() {
        // Arrange and act
        Result<InviteGuestCommand> result =
            InviteGuestCommand.Create(EventFactory.ValidEventId, GuestFactory.ValidGuestId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Payload);
    }

    [Fact]
    public void GivenInvalidEventIdAndGuestId_InviteGuestCommand_CannotBeCreated() {
        // Arrange and act
        Result<InviteGuestCommand> result = InviteGuestCommand.Create("", "");

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessage.UnParsableGuid, result.Error!.Messages);
    }
}