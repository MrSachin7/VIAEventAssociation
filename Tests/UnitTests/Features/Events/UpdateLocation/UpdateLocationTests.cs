using UnitTests.Common.Factories;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Locations;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Events.UpdateLocation;

public class UpdateLocationTests {
    [Fact]
    public void GivenAnEventInDraftStatus_WhenUpdatingLocation_ReturnsSuccessResult() {
        // Arrange
        VeaEvent veaEvent = EventFactory.GetDraftEvent();
        Location location = LocationFactory.GetValidLocation();

        // Act
        Result result = veaEvent.UpdateLocation(location);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(location, veaEvent.Location);
    }

    [Fact]
    public void GivenAnEventInReadyStatus_WhenUpdatingLocation_ReturnsSuccessResult_AndTheEventIsInDDraftStatus() {
        // Arrange
        VeaEvent veaEvent = EventFactory.GetReadyEvent();
        Location location = LocationFactory.GetValidLocation();

        // Act
        Result result = veaEvent.UpdateLocation(location);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(location, veaEvent.Location);

        Assert.Equal(EventStatus.Draft, veaEvent.Status);
    }

    [Fact]
    public void GivenAnEventInActiveStatus_WhenUpdatingLocation_ReturnsFailureResult_WithCorrectErrorMessage() {
        // Arrange
        VeaEvent veaEvent = EventFactory.GetActiveEvent();
        Location location = LocationFactory.GetValidLocation();

        // Act
        Result result = veaEvent.UpdateLocation(location);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessage.ActiveEventIsUnmodifiable, result.Error!.Messages);
    }

    [Fact]
    public void GivenAnEventInCancelledStatus_WhenUpdatingLocation_ReturnsFailureResult_WithCorrectErrorMessage() {
        // Arrange
        VeaEvent veaEvent = EventFactory.GetCancelledEvent();
        Location location = LocationFactory.GetValidLocation();

        // Act
        Result result = veaEvent.UpdateLocation(location);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessage.CancelledEventIsUnmodifiable, result.Error!.Messages);
    }
}