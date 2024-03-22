using VIAEventAssociation.Core.AppEntry.Commands.Events;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Events.CreateEvent;

public class CreateEventCommandTests {
    [Fact]
    public void CreateEmptyEvent_WithDefaultValues_ReturnsSuccessResult() {
        Result<CreateEventCommand> command = CreateEventCommand.Create();
        Assert.True(command.IsSuccess);
    }
}