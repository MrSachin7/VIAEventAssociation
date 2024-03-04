using UnitTests.Common.Factories;
using UnitTests.Common.Stubs;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
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


    private VeaEvent GetAlreadyStartedActiveEvent() {
        VeaEvent veaEvent = EventFactory.GetReadyEvent();
        ISystemTime systemTime = new TestSystemTime();
        // Arrange a valid start date but in past

        // Todo: For gods sake , I cant make an active event with a past start date. I hope Troels helps me heree..
        DateTime startDateTime = systemTime.CurrentTime().AddDays(-1).AddHours(8);
        DateTime endDateTime = systemTime.CurrentTime().AddDays(-1).AddHours(11);


        // Todo: I have tried my best to hack a solution to have a valid active event with a past start date.
        // Todo: I have given different systemtimes for EventDuration and the veaEvent itself.
        // Todo: Extremely hacky, but i could not come up with better solution.
        EventDuration eventDuration = EventDuration.From(startDateTime, endDateTime, new FakeSystemTime()).Payload!;

        // Todo: Also here i am invoking the internal setter for the state to force my way in.
        veaEvent.SetEventDuration(eventDuration);
        veaEvent.MakeActive();

        // Make sure that the event is actually active
        Assert.Equal(
            EventStatus.Active, veaEvent.Status);
        // Make sure that the event has already started
        Assert.True(new DefaultSystemTime().CurrentTime() >= veaEvent.Duration!.StartDateTime);

        return veaEvent;
    }


    private void ArrangeFullEvent(VeaEvent veaEvent) {
        // Arrange a full event
        for (int i = 0; i < veaEvent.MaxGuests.Value; i++) {
            // Since we have a different id, it will persist
            veaEvent.ParticipateGuest(GuestFactory.GetValidGuest().Id);
        }
    }

    private class FakeSystemTime : ISystemTime {
        public DateTime CurrentTime() {
            return new TestSystemTime().CurrentTime().AddDays(-2);
        }
    }
}