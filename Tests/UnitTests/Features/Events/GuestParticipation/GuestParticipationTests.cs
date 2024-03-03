using UnitTests.Common.Factories;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Events.GuestParticipation;

public class GuestParticipationTests {

    [Fact]
    public void GivenEventInStatusActive_AndPublic_AndNotFull_WhenGuestParticipates_ThenReturnsSuccessResult() {
        // Arrange with a public event and a valid guest
        VeaEvent veaEvent = EventFactory.GetActiveEvent();
        veaEvent.MakePublic();
        Assert.Equal(EventVisibility.Public, veaEvent.Visibility);
        Guest guest = GuestFactory.GetValidGuest();

        // Act
        Result result = veaEvent.ParticipateGuest(guest.Id);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Contains(guest.Id, veaEvent.IntendedParticipants);
    }

    [Fact]
    public void GivenEventInStatusActive_AndPrivate_AndNotFull_WhenGuestParticipates_ThenReturnsFailureResult_WithCorrectErrorMessage() {
        // Arrange with a private event and a valid guest
        VeaEvent veaEvent = EventFactory.GetActiveEvent();

        Assert.Equal(EventVisibility.Private, veaEvent.Visibility);
        Guest guest = GuestFactory.GetValidGuest();

        // Act
        Result result = veaEvent.ParticipateGuest(guest.Id);

        // Assert
        Assert.True(result.IsFailure);
        Assert.DoesNotContain(guest.Id, veaEvent.IntendedParticipants);
        Assert.Contains(ErrorMessage.PrivateEventCannotBeParticipatedUnlessInvited, result.Error!.Messages);
    }

    [Fact]
    public void GivenEventInStatusActive_AndPublic_AndFull_WhenGuestParticipates_ThenReturnsFailureResult_WithCorrectErrorMessage() {
        // Arrange with a private event and a valid guest
        VeaEvent veaEvent = EventFactory.GetActiveEvent();
        veaEvent.MakePublic();
        Assert.Equal(EventVisibility.Public, veaEvent.Visibility);
        Guest guest = GuestFactory.GetValidGuest();

        // Arrange a full event
        ArrangeFullEvent(veaEvent);
        bool isFull = veaEvent.IsFull();
        Assert.True(isFull);

        // Act
        Result result = veaEvent.ParticipateGuest(guest.Id);

        // Assert
        Assert.True(result.IsFailure);
        Assert.DoesNotContain(guest.Id, veaEvent.IntendedParticipants);
        Assert.Contains(ErrorMessage.MaximumNumberOfGuestsReached, result.Error!.Messages);

    }

    private void ArrangeFullEvent(VeaEvent veaEvent) {
        // Arrange a full event
        for (int i = 0; i < veaEvent.MaxGuests.Value; i++) {
            // Since we have a different id, it will persist
            veaEvent.ParticipateGuest(GuestFactory.GetValidGuest().Id);
        }
    }
}