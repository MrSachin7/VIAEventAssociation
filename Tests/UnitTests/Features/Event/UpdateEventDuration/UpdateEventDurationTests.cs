using UnitTests.Common.Stubs;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Tools.OperationResult;
using Xunit.Abstractions;
using ErrorMessage = ViaEventAssociation.Core.Tools.OperationResult.ErrorMessage;

namespace UnitTests.Features.Event.UpdateEventDuration;

public class UpdateEventDurationTests(ITestOutputHelper testOutputHelper) {

    [Theory]
    [MemberData(nameof(EventFactory.GetValidEventDurations), MemberType = typeof(EventFactory))]
    public void GivenValidStartAndEndTime_EventDuration_CanBeCreated(DateTime validStartTime, DateTime validEndTime) {
        ISystemTime systemTime = new TestSystemTime();
        Result<EventDuration> result = EventDuration.From(validStartTime, validEndTime, systemTime);

        Assert.True(result.IsSuccess);
        Assert.Equal(validStartTime, result.Payload!.StartDateTime);
        Assert.Equal(validEndTime, result.Payload!.EndDateTime);
    }

    [Theory]
    [MemberData(nameof(EventFactory.GetInValidEventDurations), MemberType = typeof(EventFactory))]
    public void GivenInValidStartAndEndTime_EventDuration_CannotBeCreated(DateTime validStartTime, DateTime validEndTime) {
        ISystemTime systemTime = new TestSystemTime();
        Result<EventDuration> result = EventDuration.From(validStartTime, validEndTime, systemTime);


        Assert.True(result.IsFailure);
    }

    [Theory]
    [MemberData(nameof(EventFactory.GetValidEventDurations), MemberType = typeof(EventFactory))]
    public void GivenAnEventOnStateReady_WhenUpdatingTheEventDurationWithValidDuration_ThenReturnsSuccessResult_AndTheEventIsInDraftStatus(DateTime validStartTime, DateTime validEndTime) {
        // Arrange with a ready event
        VeaEvent veaEvent = EventFactory.GetReadyEvent();
        ISystemTime systemTime = new TestSystemTime();
        EventDuration eventDuration = EventDuration.From(validStartTime, validEndTime, systemTime).Payload!;

        // Act
        Result result = veaEvent.UpdateEventDuration(eventDuration);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(eventDuration, veaEvent.Duration);
        // Assert that the event is in draft status
        Assert.Equal(EventStatus.Draft, veaEvent.Status);
    }

    [Fact]
    public void GivenPastStartTime_EventDuration_CannotBeCreated_ReturnsCorrectErrorMessage() {
        // Arrange with a past start time
        ISystemTime systemTime = new TestSystemTime();
        DateTime testCurrentTime = systemTime.CurrentTime();

        DateTime pastStartTime = testCurrentTime.AddMinutes(-1);
        DateTime validEndTime = testCurrentTime.AddHours(5);

        // Act
        Result<EventDuration> result = EventDuration.From(pastStartTime, validEndTime, systemTime);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessage.EventStartTimeCannotBeInPast, result.Error!.Messages);
    }

    [Fact]
    public void GivenStartDateBeforeEndDate_EventDuration_CannotBeCreated_ReturnsCorrectErrorMessage() {
        // Arrange with a start time before end time
        ISystemTime systemTime = new TestSystemTime();
        DateTime testCurrentTime = systemTime.CurrentTime();

        DateTime startTime = testCurrentTime.AddDays(2);
        DateTime endTime = testCurrentTime.AddDays(2).AddHours(-1);

        // Act
        Result<EventDuration> result = EventDuration.From(startTime, endTime, systemTime);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessage.StartTimeMustBeBeforeEndTime, result.Error!.Messages);
    }

    [Fact]
    public void GivenEventDurationLessThanOneHour_EventDuration_CannotBeCreated_ReturnsCorrectErrorMessage() {
        // Arrange with a duration less than 1 hour
        ISystemTime systemTime = new TestSystemTime();
        DateTime testCurrentTime = systemTime.CurrentTime();

        DateTime startTime = testCurrentTime.AddDays(2);
        DateTime endTime = testCurrentTime.AddDays(2).AddMinutes(59);

        // Act
        Result<EventDuration> result = EventDuration.From(startTime, endTime, systemTime);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessage.EventDurationMustBeMoreThan1Hour, result.Error!.Messages);
    }

    [Fact]
    public void GivenEventDurationMoreThanTenHours_EventDuration_CannotBeCreated_ReturnsCorrectErrorMessage() {
        // Arrange with a duration more than 10 hours
        ISystemTime systemTime = new TestSystemTime();
        DateTime testCurrentTime = systemTime.CurrentTime();

        DateTime startTime = testCurrentTime.AddDays(2);
        DateTime endTime = testCurrentTime.AddDays(2).AddHours(10).AddMinutes(1);

        // Act
        Result<EventDuration> result = EventDuration.From(startTime, endTime, systemTime);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessage.EventDurationMustBeLessThan10Hour, result.Error!.Messages);
    }

    [Fact]
    public void GivenEventSpanningBetween1AmAnd8Am_EventDuration_CannotBeCreated_ReturnsCorrectErrorMessage() {
        // Arrange with an event spanning between 1 AM and 8 AM
        ISystemTime systemTime = new TestSystemTime();
        DateTime testCurrentTime = systemTime.CurrentTime();

        DateTime startTime = testCurrentTime.AddDays(2).AddHours(1);
        DateTime endTime = testCurrentTime.AddDays(2).AddHours(7);

        // Act
        Result<EventDuration> result = EventDuration.From(startTime, endTime, systemTime);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessage.EventCannotSpanBetween1AmAnd8Am, result.Error!.Messages);
    }

    [Fact]
    public void GivenEventStartingBefore8Am_EventDuration_CannotBeCreated_ReturnsCorrectErrorMessage() {
        // Arrange with an event starting before 8 AM
        ISystemTime systemTime = new TestSystemTime();
        DateTime testCurrentTime = systemTime.CurrentTime();

        DateTime startTime = testCurrentTime.AddDays(2).AddHours(7);
        DateTime endTime = testCurrentTime.AddDays(2).AddHours(9);

        // Act
        Result<EventDuration> result = EventDuration.From(startTime, endTime, systemTime);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessage.EventCannotStartBefore8Am, result.Error!.Messages);
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