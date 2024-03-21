using UnitTests.Common.Factories;
using UnitTests.Common.Stubs;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Locations;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Events.MakeActive;

public class MakeEventActiveTests {
    [Fact]
    public void GivenAnEventInStateReady_WhenMakingEventActive_ThenReturnsSuccessResult_AndTheEventIsInActiveStatus() {
        // Arrange
        VeaEvent veaEvent = EventFactory.GetReadyEvent();
        // Act
        Result result = veaEvent.MakeActive(new TestSystemTime());
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(EventStatus.Active, veaEvent.Status);
    }

    [Fact]
    public void GivenAnEventInDraftStatus_WhenMakingEventActive_AndAllFieldsAreSet_ThenReturnsSuccessResult() {
        // Arrange
        VeaEvent veaEvent = EventFactory.GetDraftEvent();
        veaEvent.UpdateDescription(EventFactory.GetValidEventDescription());
        veaEvent.UpdateTitle(EventFactory.GetValidEventTitle());
        veaEvent.UpdateEventDuration(EventFactory.GetValidEventDuration());
        veaEvent.UpdateLocation(Location.Create(LocationName.Create("C02.03").Payload!));

        // Act
        Result result = veaEvent.MakeActive(new TestSystemTime());

        // Assert
        Assert.True(result.IsSuccess);
        
        Assert.Equal(EventStatus.Active, veaEvent.Status);
    }

    [Fact]
    public void
        GivenAnEventInDraftStatus_WhenMakingEventActive_AndDescriptionIsDefault_ThenReturnsFailureResult_WithCorrectErrorMessage() {
        // Arrange with default description
        VeaEvent veaEvent = EventFactory.GetDraftEvent();
        veaEvent.UpdateTitle(EventFactory.GetValidEventTitle());
        veaEvent.UpdateEventDuration(EventFactory.GetValidEventDuration());

        // Act
        Result result = veaEvent.MakeActive(new TestSystemTime());

        // Assert
        Assert.True(result.IsFailure);

        Assert.Contains(ErrorMessage.DescriptionMustBeSetBeforeMakingAnEventReady, result.Error!.Messages);
        // State is NOT updated
        Assert.Equal(EventStatus.Draft, veaEvent.Status);
    }

    [Fact]
    public void
        GivenAnEventInDraftStatus_WhenMakingEventActive_AndTitleIsDefault_ThenReturnsFailureResult_WithCorrectErrorMessage() {
        // Arrange with default title
        VeaEvent veaEvent = EventFactory.GetDraftEvent();
        veaEvent.UpdateDescription(EventFactory.GetValidEventDescription());
        veaEvent.UpdateEventDuration(EventFactory.GetValidEventDuration());

        // Act
        Result result = veaEvent.MakeActive(new TestSystemTime());

        // Assert
        Assert.True(result.IsFailure);

        Assert.Contains(ErrorMessage.TitleMustBeSetBeforeMakingAnEventReady, result.Error!.Messages);
        // State is NOT updated
        Assert.Equal(EventStatus.Draft, veaEvent.Status);
    }

    [Fact]
    public void
        GivenAnEventInDraftStatus_WhenMakingEventActive_AndDurationIsDefault_ThenReturnsFailureResult_WithCorrectErrorMessage() {
        // Arrange with default title
        VeaEvent veaEvent = EventFactory.GetDraftEvent();
        veaEvent.UpdateDescription(EventFactory.GetValidEventDescription());
        veaEvent.UpdateTitle(EventFactory.GetValidEventTitle());

        // Act
        Result result = veaEvent.MakeActive(new TestSystemTime());

        // Assert
        Assert.True(result.IsFailure);

        Assert.Contains(ErrorMessage.EventDurationMustBeSetBeforeMakingAnEventReady, result.Error!.Messages);
        // State is NOT updated
        Assert.Equal(EventStatus.Draft, veaEvent.Status);
    }

    [Fact]
    public void GivenAnEventInStatusDraft_WhenMakingEventActive_AndDurationIsDefault_AndTitleIsDefault_AndDescriptionIsDefault_ThenReturnsFailureResult_WithMultipleErrorMessages() {
        // Arrange with default title
        VeaEvent veaEvent = EventFactory.GetDraftEvent();

        // Act
        Result result = veaEvent.MakeActive(new TestSystemTime());

        // Assert
        Assert.True(result.IsFailure);

        Assert.Contains(ErrorMessage.EventDurationMustBeSetBeforeMakingAnEventReady, result.Error!.Messages);
        Assert.Contains(ErrorMessage.TitleMustBeSetBeforeMakingAnEventReady, result.Error!.Messages);
        Assert.Contains(ErrorMessage.EventDurationMustBeSetBeforeMakingAnEventReady, result.Error!.Messages);
        // State is NOT updated
        Assert.Equal(EventStatus.Draft, veaEvent.Status);
    }

    [Fact]
    public void GivenAnEventInStatusActive_WhenMakingEventActive_ReturnsSuccessResult() {
        // Arrange
        VeaEvent veaEvent = EventFactory.GetActiveEvent();
        // Act
        Result result = veaEvent.MakeActive(new TestSystemTime());

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(EventStatus.Active, veaEvent.Status);
    }

    [Fact]
    public void GivenAnEventInStatusCancelled_WhenMakingEventReady_ReturnsFailureResult() {
        // Arrange
        VeaEvent veaEvent = EventFactory.GetCancelledEvent();
        // Act
        Result result = veaEvent.MakeActive(new TestSystemTime());

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessage.CancelledEventCannotBeActivated, result.Error!.Messages);
        Assert.Equal(EventStatus.Cancelled, veaEvent.Status);
    }
}