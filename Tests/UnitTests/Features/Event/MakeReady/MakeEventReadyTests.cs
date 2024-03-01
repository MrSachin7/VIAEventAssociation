using VIAEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Event.MakeReady;

public class MakeEventReadyTests {

    [Fact]
    public void GivenAnEventInDraftStatus_WhenMakingEventReady_AndAllFieldsAreSet_ThenReturnsSuccessResult() {
        // Arrange
        VeaEvent veaEvent = EventFactory.GetDraftEvent();
        veaEvent.UpdateDescription(EventFactory.GetValidEventDescription());
        veaEvent.UpdateTitle(EventFactory.GetValidEventTitle());
        veaEvent.UpdateEventDuration(EventFactory.GetValidEventDuration());

        // Act
        Result result = veaEvent.MakeReady();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(EventStatus.Ready, veaEvent.Status);
    }
    
}