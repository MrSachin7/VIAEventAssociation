using UnitTests.Common.Factories;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Events.UpdateMaxGuests;

public class UpdateEventMaxGuestsServiceTests {

    [Theory]
    [MemberData(nameof(EventFactory.GetValidEventMaxGuests), MemberType = typeof(EventFactory))]
    public void GivenEventOnStateDraft_WhenUpdatingMaxGuests_ReturnsSuccessResult(int validMaxGuests) {
        // Arrange
        VeaEvent veaEvent = EventFactory.GetDraftEvent();
        veaEvent.SetLocation(LocationFactory.GetValidLocation());
        EventMaxGuests maxGuests = EventMaxGuests.Create(validMaxGuests).Payload!;

        // Act
        Result result = veaEvent.UpdateMaximumNumberOfGuests(maxGuests);

        // Assert    
        Assert.True(result.IsSuccess);
        Assert.Equal(validMaxGuests, veaEvent.MaxGuests.Value);
        Assert.Equal(EventStatus.Draft, veaEvent.Status);
    }

    [Theory]
    [MemberData(nameof(EventFactory.GetValidEventMaxGuests), MemberType = typeof(EventFactory))]
    public void GivenEventOnStateReady_WhenUpdatingMaxGuests_ReturnsSuccessResult(int validMaxGuests) {
        // Arrange
        VeaEvent veaEvent = EventFactory.GetReadyEvent();
        EventMaxGuests maxGuests = EventMaxGuests.Create(validMaxGuests).Payload!;

        // Act
        Result result = veaEvent.UpdateMaximumNumberOfGuests(maxGuests);

        // Assert    
        Assert.True(result.IsSuccess);
        Assert.Equal(validMaxGuests, veaEvent.MaxGuests.Value);
        Assert.Equal(EventStatus.Ready, veaEvent.Status);
    }

    [Fact]
    public void
        GivenEventOnStateActive_WhenUpdatingMaxGuests_AndNewMaxGuestsIsLargerThanPrevious_ReturnsSuccessResult() {
        // Arrange
        VeaEvent veaEvent = EventFactory.GetActiveEvent();

        // Act
        // Default is 5
        int validMaxGuestsMoreThanDefault5 = 10;
        EventMaxGuests maxGuests = EventMaxGuests.Create(validMaxGuestsMoreThanDefault5).Payload!;
        Result result = veaEvent.UpdateMaximumNumberOfGuests(maxGuests);

        // Assert    
        Assert.True(result.IsSuccess);
        Assert.Equal(validMaxGuestsMoreThanDefault5, veaEvent.MaxGuests.Value);
        Assert.Equal(EventStatus.Active, veaEvent.Status);
    }


    [Fact]
    public void
        GivenEventOnStateDraft_WhenUpdatingMaxGuests_AndNewMaxGuestsIsSmallerThanPrevious_ReturnsSuccessResult() {
        // Arrange with 10 max guests
        EventMaxGuests previousMaxGuests = EventMaxGuests.Create(10).Payload!;
        VeaEvent veaEvent = EventFactory.GetDraftEvent();
        veaEvent.SetLocation(LocationFactory.GetValidLocation());
        veaEvent.UpdateMaximumNumberOfGuests(previousMaxGuests);
        Assert.Equal(previousMaxGuests, veaEvent.MaxGuests);
        Assert.Equal(EventStatus.Draft, veaEvent.Status);

        // Act decreasing to 9
        EventMaxGuests newMaxGuests = EventMaxGuests.Create(9).Payload!;
        Result result = veaEvent.UpdateMaximumNumberOfGuests(newMaxGuests);

        // Assert    
        Assert.True(result.IsSuccess);
        Assert.Equal(newMaxGuests, veaEvent.MaxGuests);
    }

    [Fact]
    public void
        GivenEventOnStateReady_WhenUpdatingMaxGuests_AndNewMaxGuestsIsSmallerThanPrevious_ReturnsSuccessResult() {
        // Arrange with 10 max guests
        EventMaxGuests previousMaxGuests = EventMaxGuests.Create(10).Payload!;
        VeaEvent veaEvent = EventFactory.GetReadyEvent();
        veaEvent.UpdateMaximumNumberOfGuests(previousMaxGuests);
        Assert.Equal(previousMaxGuests, veaEvent.MaxGuests);
        Assert.Equal(EventStatus.Ready, veaEvent.Status);

        // Act decreasing to 9
        EventMaxGuests newMaxGuests = EventMaxGuests.Create(9).Payload!;
        Result result = veaEvent.UpdateMaximumNumberOfGuests(newMaxGuests);

        // Assert    
        Assert.True(result.IsSuccess);
        Assert.Equal(newMaxGuests, veaEvent.MaxGuests);
    }

    [Fact]
    public void
        GivenEventOnStateActive_WhenUpdatingMaxGuests_AndNewMaxGuestsIsSmallerThanPrevious_ReturnsFailureResult() {
        // Arrange with 10 max guests
        EventMaxGuests previousMaxGuests = EventMaxGuests.Create(10).Payload!;
        VeaEvent veaEvent = EventFactory.GetActiveEvent();
        veaEvent.UpdateMaximumNumberOfGuests(previousMaxGuests);
        Assert.Equal(previousMaxGuests, veaEvent.MaxGuests);
        Assert.Equal(EventStatus.Active, veaEvent.Status);

        // Act decreasing to 9
        EventMaxGuests newMaxGuests = EventMaxGuests.Create(9).Payload!;
        Result result = veaEvent.UpdateMaximumNumberOfGuests(newMaxGuests);

        // Assert    
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessage.ActiveEventCannotReduceMaxGuests, result.Error!.Messages);
    }

    [Fact]
    public void GivenEventOnStateCancelled_WhenUpdatingMaxGuests_ReturnsFailureResult_WithCorrectError() {
        // Arrange with 10 max guests
        VeaEvent veaEvent = EventFactory.GetCancelledEvent();
        EventMaxGuests initialMaxGuests = veaEvent.MaxGuests;


        // Act decreasing to 9
        EventMaxGuests newMaxGuests = EventMaxGuests.Create(9).Payload!;
        Result result = veaEvent.UpdateMaximumNumberOfGuests(newMaxGuests);

        // Assert    
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessage.CancelledEventIsUnmodifiable, result.Error!.Messages);
        Assert.Equal(EventStatus.Cancelled, veaEvent.Status);

        Assert.Equal(initialMaxGuests, veaEvent.MaxGuests);
    }

    


}