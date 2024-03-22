using UnitTests.Common.Factories;
using VIAEventAssociation.Core.AppEntry.Commands.Events;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Events.MakeReady;

public class MakeEventReadyCommandTests {
    [Fact]
    public void GivenValidEventId_MakeEventPrivateCommand_CanBeCreated() {
        // Arrange and act
        Result<MakeEventReadyCommand> result =
            MakeEventReadyCommand.Create(EventFactory.ValidEventId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Payload);
    }

    [Fact]
    public void GivenInvalidEventIdAndGuestId_MakeEventPrivateCommand_CannotBeCreated() {
        // Arrange and act
        Result<MakeEventReadyCommand> result = MakeEventReadyCommand.Create("");

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessage.UnParsableGuid, result.Error!.Messages);
    }
}