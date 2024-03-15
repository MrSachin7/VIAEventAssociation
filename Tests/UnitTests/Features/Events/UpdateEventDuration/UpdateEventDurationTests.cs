using UnitTests.Common.Factories;
using UnitTests.Common.Stubs;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Tools.OperationResult;
using Xunit.Abstractions;
using ErrorMessage = ViaEventAssociation.Core.Tools.OperationResult.ErrorMessage;

namespace UnitTests.Features.Events.UpdateEventDuration;

public class UpdateEventDurationTests(ITestOutputHelper testOutputHelper) {

   

    [Theory]
    [MemberData(nameof(EventFactory.GetValidEventDurations), MemberType = typeof(EventFactory))]
    public void GivenAnEventOnStateReady_WhenUpdatingTheEventDurationWithValidDuration_ThenReturnsSuccessResult_AndTheEventIsInDraftStatus(DateTime validStartTime, DateTime validEndTime) {
        // Arrange with a ready event
        VeaEvent veaEvent = EventFactory.GetReadyEvent();
        ISystemTime systemTime = new TestSystemTime();
        EventDuration eventDuration = EventDuration.Create(validStartTime, validEndTime, systemTime).Payload!;

        // Act
        Result result = veaEvent.UpdateEventDuration(eventDuration);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(eventDuration, veaEvent.Duration);
        // Assert that the event is in draft status
        Assert.Equal(EventStatus.Draft, veaEvent.Status);
    }

   

    [Fact]
    public void GivenAnEventOnStateActive_WhenUpdatingTheEventDurationWithValidDuration_ThenReturnsFailureResult_WithCorrectErrorMessage() {
        // Arrange with a ready event
        VeaEvent veaEvent = EventFactory.GetActiveEvent();

        EventDuration eventDuration = EventFactory.GetValidEventDuration();

        // Act
        Result result = veaEvent.UpdateEventDuration(eventDuration);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessage.ActiveEventIsUnmodifiable, result.Error!.Messages);
    }

    [Fact]
    public void GivenAnEventOnStateCancelled_WhenUpdatingTheEventDurationWithValidDuration_ThenReturnsFailureResult_WithCorrectErrorMessage() {
        // Arrange with a ready event
        VeaEvent veaEvent = EventFactory.GetCancelledEvent();

        EventDuration eventDuration = EventFactory.GetValidEventDuration();

        // Act
        Result result = veaEvent.UpdateEventDuration(eventDuration);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessage.CancelledEventIsUnmodifiable, result.Error!.Messages);
    }
    
}