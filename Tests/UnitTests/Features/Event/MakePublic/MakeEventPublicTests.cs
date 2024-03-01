using VIAEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Event.MakePublic;

public class MakeEventPublicTests {


    [Fact]
    public void GivenAnEventInStatusDraft_WhenMadePublic_ReturnsSuccessResult() {
        // Arrange
        VeaEvent veaEvent = EventFactory.GetDraftEvent();

        Result result = veaEvent.MakePublic();

        Assert.True(result.IsSuccess);
        Assert.Equal(EventVisibility.Public, veaEvent.Visibility);
        
        // Confirm that the state has not changed
        Assert.Equal(EventStatus.Draft, veaEvent.Status);
    }

    [Fact]
    public void GivenAnEventInStatusReady_WhenMadePublic_ReturnsSuccessResult() {
        // Arrange
        VeaEvent veaEvent = EventFactory.GetReadyEvent();

        Result result = veaEvent.MakePublic();

        Assert.True(result.IsSuccess);
        Assert.Equal(EventVisibility.Public, veaEvent.Visibility);

        // Confirm that the state has not changed
        Assert.Equal(EventStatus.Ready, veaEvent.Status);
    }

    [Fact]
    public void GivenAnEventInStatusActive_WhenMadePublic_ReturnsSuccessResult() {
        // Arrange
        VeaEvent veaEvent = EventFactory.GetActiveEvent();

        Result result = veaEvent.MakePublic();

        Assert.True(result.IsSuccess);
        Assert.Equal(EventVisibility.Public, veaEvent.Visibility);

        // Confirm that the state has not changed
        Assert.Equal(EventStatus.Active, veaEvent.Status);
    }


    [Fact]
    public void GivenAnEventInStatusCancelled_WhenMadePublic_ReturnsFailureResult_WithCorrectErrorMessage() {
        // Arrange
        VeaEvent veaEvent = EventFactory.GetCancelledEvent();

        Result result = veaEvent.MakePublic();

        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessage.CancelledEventIsUnmodifiable, result.Error!.Messages);

        // Confirm that the visibility has not changed
        Assert.Equal(EventVisibility.Private, veaEvent.Visibility);

        // Confirm that the state has not changed
        Assert.Equal(EventStatus.Cancelled, veaEvent.Status);
    }
}