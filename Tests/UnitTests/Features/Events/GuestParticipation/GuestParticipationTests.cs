using UnitTests.Common.Factories;
using UnitTests.Common.Stubs;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Events.Entities.Invitation;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Events.GuestParticipation;

public class GuestParticipationTests {
    [Fact]
    public void
        GivenEventInStatusActive_AndPublic_AndNotFull_AndNotStartedYet_WhenGuestParticipates_ThenReturnsSuccessResult() {
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
    public void
        GivenEventInStatusActive_AndPrivate_AndNotFull_AndNotStartedYet_WhenGuestParticipates_ThenReturnsFailureResult_WithCorrectErrorMessage() {
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
    public void
        GivenEventInStatusActive_AndPublic_AndFull_AndNotStartedYet_WhenGuestParticipates_ThenReturnsFailureResult_WithCorrectErrorMessage() {
        // Arrange with a private event and a valid guest
        VeaEvent veaEvent = EventFactory.GetActiveEvent();
        veaEvent.MakePublic();
        Assert.Equal(EventVisibility.Public, veaEvent.Visibility);
        Guest guest = GuestFactory.GetValidGuest();

        // Arrange a full event
        EventFactory.ArrangeFullEvent(veaEvent);
     ;

        // Act
        Result result = veaEvent.ParticipateGuest(guest.Id);

        // Assert
        Assert.True(result.IsFailure);
        Assert.DoesNotContain(guest.Id, veaEvent.IntendedParticipants);
        Assert.Contains(ErrorMessage.MaximumNumberOfGuestsReached, result.Error!.Messages);
    }

    [Fact]
    public void
        GivenEventInStatusActive_AndPublic_AndNotFull_AndAlreadyStarted_WhenGuestParticipates_ThenReturnsFailureResult_WithCorrectErrorMessage() {
        // Arrange with a public event and a valid guest
        VeaEvent veaEvent = GetAlreadyStartedActiveEvent();
        veaEvent.MakePublic();
        Assert.Equal(EventVisibility.Public, veaEvent.Visibility);
        Guest guest = GuestFactory.GetValidGuest();

        // Act
        Result result = veaEvent.ParticipateGuest(guest.Id);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessage.EventHasAlreadyStarted, result.Error!.Messages);
    }

    [Fact]
    public void
        GivenEventInInStatusActive_AndTheGuestParticipationExists_WhenGuestCancelsParticipation_ReturnsSuccessResult_AndTheParticipationIsDeleted() {
        // Arrange with an event that has a guest participation
        VeaEvent veaEvent = EventFactory.GetActiveEvent();
        veaEvent.MakePublic();
        Guest guest = GuestFactory.GetValidGuest();
        veaEvent.ParticipateGuest(guest.Id);
        // Make sure that the guest is participating before continuing
        Assert.Contains(guest.Id, veaEvent.IntendedParticipants);

        // Act
        Result result = veaEvent.CancelGuestParticipation(guest.Id);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.DoesNotContain(guest.Id, veaEvent.IntendedParticipants);
    }


    private VeaEvent GetAlreadyStartedActiveEvent() {
        VeaEvent veaEvent = EventFactory.GetReadyEvent();
        ISystemTime systemTime = new TestSystemTime();
        // Arrange a valid start date but in past

        // Todo: For gods sake , I cant make an active event with a past start date. I hope Troels helps me heree..
        DateTime startDateTime = systemTime.CurrentTime().AddDays(-1).AddHours(8);
        DateTime endDateTime = systemTime.CurrentTime().AddDays(-1).AddHours(11);


 
        EventDuration eventDuration = EventDuration.Create(startDateTime, endDateTime, new FakeSystemTime()).Payload!;

        // Also here i am invoking the internal setter for the state to force my way in.
        veaEvent.SetEventDuration(eventDuration);
        veaEvent.MakeActive();

        // Make sure that the event is actually active
        Assert.Equal(
            EventStatus.Active, veaEvent.Status);
        // Make sure that the event has already started
        Assert.True(new DefaultSystemTime().CurrentTime() >= veaEvent.Duration!.StartDateTime);

        return veaEvent;
    }


   

    private class FakeSystemTime : ISystemTime {
        public DateTime CurrentTime() {
            return new TestSystemTime().CurrentTime().AddDays(-2);
        }
    }
}