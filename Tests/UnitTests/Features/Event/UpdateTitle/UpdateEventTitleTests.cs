using VIAEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Event.UpdateTitle;

public class UpdateEventTitleTests {

    [Theory]
    [MemberData(nameof(EventFactory.GetValidEventTitles), MemberType = typeof(EventFactory))]
    public void GivenValidTitle_WhenCreatingEventTitle_ThenReturnsSuccessResult(string title) {
        // Arrange
        // and
        // Act
        Result<EventTitle> result = EventTitle.From(title);
        Assert.True(result.IsSuccess);
        Assert.True(result.Payload!.Value.Equals(title));
    }

    [Theory]
    [MemberData(nameof(EventFactory.GetInValidEventTitles), MemberType = typeof(EventFactory))]
    public void GivenInValidTitle_WhenCreatingEventTitle_ThenReturnsFailureResult_WithCorrectError(string title) {
        // Arrange
        // and
        // Act
        Result<EventTitle> result = EventTitle.From(title);
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessage.TitleMustBeBetween3And75Chars, result.Error!.Messages);
    }

    [Theory]
    [MemberData(nameof(EventFactory.GetValidEventTitles), MemberType = typeof(EventFactory))]
    public void GivenEventInADraftStatus_WhenUpdatingTitle_ThenReturnsSuccessResult(string title) {
        // Arrange with a draft event
        VeaEvent veaEvent = EventFactory.GetDraftEvent();
        EventTitle eventTitle = EventTitle.From(title).Payload!;

        // Act
        Result result = veaEvent.UpdateTitle(eventTitle);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(eventTitle, veaEvent.Title);
    }

    [Theory]
    [MemberData(nameof(EventFactory.GetValidEventTitles), MemberType = typeof(EventFactory))]
    public void GivenEventInAReadyStatus_WhenUpdatingTitle_ThenReturnsSuccessResult_AndTheEventIsInDraftStatus(string title) {
        // Arrange with a ready event
        VeaEvent veaEvent = EventFactory.GetReadyEvent();
        EventTitle eventTitle = EventTitle.From(title).Payload!;

        // Act
       Result result = veaEvent.UpdateTitle(eventTitle);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal( eventTitle, veaEvent.Title);
        // Assert that the event is in draft status
        Assert.Equal(EventStatus.Draft, veaEvent.Status);
    }

    [Theory]
    [MemberData(nameof(EventFactory.GetValidEventTitles), MemberType = typeof(EventFactory))]
    public void GivenEventInAnActiveStatus_WhenUpdatingTitle_ThenReturnsFailureResult_WithCorrectErrorMessage(string title) {
        // Arrange with an active event
        VeaEvent veaEvent = EventFactory.GetActiveEvent();
        EventTitle initialTitle = veaEvent.Title;
        EventTitle eventTitle = EventTitle.From(title).Payload!;

        // Act
        Result result = veaEvent.UpdateTitle(eventTitle);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessage.ActiveEventIsUnmodifiable, result.Error!.Messages);

        // And the title is not updated
        Assert.Equal(initialTitle, veaEvent.Title);
    }

    [Theory]
    [MemberData(nameof(EventFactory.GetValidEventTitles), MemberType = typeof(EventFactory))]
    public void GivenEventInACancelledStatus_WhenUpdatingTitle_ThenReturnsFailureResult_WithCorrectErrorMessage(string title) {
        // Arrange with a cancelled event
        VeaEvent veaEvent = EventFactory.GetCancelledEvent();
        EventTitle initialTitle = veaEvent.Title;
        EventTitle eventTitle = EventTitle.From(title).Payload!;

        // Act
        Result result = veaEvent.UpdateTitle(eventTitle);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessage.CancelledEventIsUnmodifiable, result.Error!.Messages);

        // And the title is not updated
        Assert.Equal(initialTitle, veaEvent.Title);
    }


    


}