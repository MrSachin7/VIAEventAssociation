using UnitTests.Common.Factories;
using VIAEventAssociation.Core.AppEntry.Commands.Events;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Events.ParticipateGuests;

public class ParticipateGuestCommandTests {
    [Fact]
    public void GivenValidEventIdAndGuestId_ParticipateGuestCommand_CanBeCreated() {
        // Arrange and act
        Result<ParticipateGuestCommand> result =
            ParticipateGuestCommand.Create(EventFactory.ValidEventId, GuestFactory.ValidGuestId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Payload);
    }

    [Fact]
    public void GivenInvalidEventIdAndGuestId_ParticipateGuestCommand_CannotBeCreated() {
        // Arrange and act
        Result<ParticipateGuestCommand> result = ParticipateGuestCommand.Create("", "");

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessage.UnParsableGuid, result.Error!.Messages);
    }

    
}