using VIAEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Event.MakePrivate;

public class MakeEventPrivateTests {
    [Fact]
    public void
        GivenAnEventInStateDraft_AndTheEventIsAlreadyPrivate_WhenMadePrivate_ReturnsSuccessResult_AndTheEventIsUnmodified() {
        // Arrange
        VeaEvent veaEvent = EventFactory.GetDraftEvent();
        // Act
        Result result = veaEvent.MakePrivate();

        // Assert
        Assert.True(result.IsSuccess);
        // Confirm that the visibility has not changed
        Assert.Equal(EventVisibility.Private, veaEvent.Visibility);
    }

    [Fact]
    public void
        GivenAnEventInStateReady_AndTheEventIsAlreadyPrivate_WhenMadePrivate_ReturnsSuccessResult_AndTheEventIsUnmodified() {
        // Arrange
        VeaEvent veaEvent = EventFactory.GetReadyEvent();
        // Act
        Result result = veaEvent.MakePrivate();

        // Assert
        Assert.True(result.IsSuccess);
        // Confirm that the visibility has not changed
        Assert.Equal(EventVisibility.Private, veaEvent.Visibility);
    }

    [Fact]
    public void
        GivenAnEventInStateDraft_AndTheEventIsPublic_WhenMadePrivate_ReturnsSuccessResult_AndTheEventIsInDraftStatus() {
        // Arrange a ready event that is public
        VeaEvent veaEvent = EventFactory.GetDraftEvent();
        veaEvent.MakePublic();
        Assert.Equal(EventVisibility.Public, veaEvent.Visibility);


        // Act
        Result result = veaEvent.MakePrivate();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(EventVisibility.Private, veaEvent.Visibility);

        Assert.Equal(EventStatus.Draft, veaEvent.Status);
    }

    [Fact]
    public void
        GivenAnEventInStateReady_AndTheEventIsPublic_WhenMadePrivate_ReturnsSuccessResult_AndTheEventIsInDraftStatus() {
        // Arrange a ready event that is public
        VeaEvent veaEvent = EventFactory.GetReadyEvent();
        veaEvent.MakePublic();
        Assert.Equal(EventVisibility.Public, veaEvent.Visibility);
        // Act
        Result result = veaEvent.MakePrivate();

        // Assert
        Assert.True(result.IsSuccess);
        // Confirm that the visibility has not changed
        Assert.Equal(EventVisibility.Private, veaEvent.Visibility);

        Assert.Equal(EventStatus.Draft, veaEvent.Status);

    }




}