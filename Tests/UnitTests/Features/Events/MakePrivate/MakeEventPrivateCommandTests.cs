using UnitTests.Common.Factories;
using VIAEventAssociation.Core.AppEntry.Commands.Events;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Events.MakePrivate;

public class MakeEventPrivateCommandTests {
    [Fact]
    public void GivenValidEventId_MakeEventPrivateCommand_CanBeCreated() {
        // Arrange and act
        Result<MakeEventPrivateCommand> result =
            MakeEventPrivateCommand.Create(EventFactory.ValidEventId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Payload);
    }

    [Fact]
    public void GivenInvalidEventIdAndGuestId_MakeEventPrivateCommand_CannotBeCreated() {
        // Arrange and act
        Result<MakeEventPrivateCommand> result = MakeEventPrivateCommand.Create("");

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessage.UnParsableGuid, result.Error!.Messages);
    }
}