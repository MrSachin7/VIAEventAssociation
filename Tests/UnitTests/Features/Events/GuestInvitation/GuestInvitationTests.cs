using UnitTests.Common.Factories;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Events.Entities.Invitation;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Events.GuestInvitation;

/*
 * 
 */
public class GuestInvitationTests {
    
    [Fact]
    public void GivenEventInStatusActive_AndNotFull_AndNotStartedYet_WhenGuestIsInvited_ThenReturnsSuccessResult() {
        // Arrange with a public event and a valid guest
        VeaEvent veaEvent = EventFactory.GetActiveEvent();
        Guest guest = GuestFactory.GetValidGuest();
        EventInvitation invitation = EventInvitation.From(guest.Id);

        // Act
        Result result = veaEvent.InviteGuest(invitation);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Contains(invitation, veaEvent.EventInvitations);
    }

    [Fact]
    public void GivenEventInStatusReady_AndNotFull_AndNotStartedYet_WhenGuestIsInvited_ThenReturnsSuccessResult() {
        // Arrange with a public event and a valid guest
        VeaEvent veaEvent = EventFactory.GetReadyEvent();
        Guest guest = GuestFactory.GetValidGuest();
        EventInvitation invitation = EventInvitation.From(guest.Id);

        // Act
        Result result = veaEvent.InviteGuest(invitation);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Contains(invitation, veaEvent.EventInvitations);
    }

    [Fact]
    public void GivenEventInStatusDraft_AndNotFull_AndNotStartedYet_WhenGuestIsInvited_ThenReturnsFailureResult_WithCorrectErrorMessage() {
        // Arrange with a public event and a valid guest
        VeaEvent veaEvent = EventFactory.GetDraftEvent();
        Guest guest = GuestFactory.GetValidGuest();
        EventInvitation invitation = EventInvitation.From(guest.Id);

        // Act
        Result result = veaEvent.InviteGuest(invitation);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessage.InvitationsCanOnlyBeMadeOnReadyOrActiveEvent, result.Error!.Messages);
    }

    [Fact]
    public void GivenEventInStatusCancelled_AndNotFull_AndNotStartedYet_WhenGuestIsInvited_ThenReturnsFailureResult_WithCorrectErrorMessage() {
        // Arrange with a public event and a valid guest
        VeaEvent veaEvent = EventFactory.GetCancelledEvent();
        Guest guest = GuestFactory.GetValidGuest();
        EventInvitation invitation = EventInvitation.From(guest.Id);

        // Act
        Result result = veaEvent.InviteGuest(invitation);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessage.InvitationsCanOnlyBeMadeOnReadyOrActiveEvent, result.Error!.Messages);
    }

    [Fact]
    public void GivenEventInStatusActive_AndFull_AndNotStartedYet_WhenGuestIsInvited_ThenReturnsFailureResult_WithCorrectErrorMessage() {
        // Arrange with a public event and a valid guest
        VeaEvent veaEvent = EventFactory.GetActiveEvent();
        EventFactory.ArrangeFullEvent(veaEvent);

        Guest guest = GuestFactory.GetValidGuest();
        EventInvitation invitation = EventInvitation.From(guest.Id);

        // Act
        Result result = veaEvent.InviteGuest(invitation);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessage.MaximumNumberOfGuestsReached, result.Error!.Messages);
    }
}