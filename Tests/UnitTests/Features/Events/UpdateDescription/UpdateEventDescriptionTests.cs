using UnitTests.Common.Factories;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Events.UpdateDescription;

public class UpdateEventDescriptionTests {
   
    [Theory]
    [MemberData(nameof(EventFactory.GetValidEventDescriptions), MemberType = typeof(EventFactory))]
    public void GivenEventInADraftStatus_WhenUpdatingDescription_ThenReturnsSuccessResult(string description) {
        // Arrange with a draft event
        VeaEvent veaEvent = EventFactory.GetDraftEvent();
        EventDescription eventDescription = EventDescription.From(description).Payload!;

        // Act
        Result result = veaEvent.UpdateDescription(eventDescription);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(eventDescription, veaEvent.Description);
    }

    [Theory]
    [MemberData(nameof(EventFactory.GetValidEventDescriptions), MemberType = typeof(EventFactory))]
    public void GivenEventInAReadyStatus_WhenUpdatingDescription_ThenReturnsSuccessResult_AndTheEventIsInDraftStatus(string description) {
        // Arrange with a draft event
        VeaEvent veaEvent = EventFactory.GetReadyEvent();
        EventDescription eventDescription = EventDescription.From(description).Payload!;

        // Act
        Result result = veaEvent.UpdateDescription(eventDescription);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(eventDescription, veaEvent.Description);

        // Assert that the event is in draft status
        Assert.Equal(EventStatus.Draft, veaEvent.Status);
    }

    [Theory]
    [MemberData(nameof(EventFactory.GetValidEventDescriptions), MemberType = typeof(EventFactory))]
    public void GivenEventInAnActiveStatus_WhenUpdatingDescription_ThenReturnsFailureResult_WithCorrectErrorMessage(string description) {
        // Arrange with a draft event
        VeaEvent veaEvent = EventFactory.GetActiveEvent();
        EventDescription initialDescription = veaEvent.Description;
        EventDescription eventDescription = EventDescription.From(description).Payload!;

        // Act
        Result result = veaEvent.UpdateDescription(eventDescription);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessage.ActiveEventIsUnmodifiable, result.Error!.Messages);

        // And the title is not updated
        Assert.Equal(initialDescription, veaEvent.Description);

    }

    [Theory]
    [MemberData(nameof(EventFactory.GetValidEventDescriptions), MemberType = typeof(EventFactory))]
    public void GivenEventInACancelledStatus_WhenUpdatingDescription_ThenReturnsFailureResult_WithCorrectErrorMessage(string description) {
        // Arrange with a draft event
        VeaEvent veaEvent = EventFactory.GetCancelledEvent();
        EventDescription initialDescription = veaEvent.Description;
        EventDescription eventDescription = EventDescription.From(description).Payload!;

        // Act
        Result result = veaEvent.UpdateDescription(eventDescription);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessage.CancelledEventIsUnmodifiable, result.Error!.Messages);

        // And the title is not updated
        Assert.Equal(initialDescription, veaEvent.Description);

    }


}