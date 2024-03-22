using UnitTests.Common.Factories;
using VIAEventAssociation.Core.AppEntry.Commands.Events;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Events.MakeActive;

public class MakeEventActiveCommandTests {
    [Fact]
    public void GivenValidEventId_MakeEventActiveCommand_CanBeCreated() {
        // Arrange and act
        Result<MakeEventActiveCommand> result =
            MakeEventActiveCommand.Create(EventFactory.ValidEventId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Payload);
    }

    [Fact]
    public void GivenInvalidEventIdAndGuestId_MakeEventActiveCommand_CannotBeCreated() {
        // Arrange and act
        Result<MakeEventActiveCommand> result = MakeEventActiveCommand.Create("");

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessage.UnParsableGuid, result.Error!.Messages);
    }
}