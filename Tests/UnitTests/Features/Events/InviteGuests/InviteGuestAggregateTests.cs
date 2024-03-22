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
    public async Task GivenEventInStatusActive_AndNotFull_AndNotStartedYet_WhenGuestIsInvited_ThenReturnsSuccessResult() {
        // Arrange with a public event and a valid guest
        VeaEvent veaEvent = EventFactory.GetActiveEvent();
        Guest guest = await GuestFactory.GetValidGuest();
        EventInvitation invitation = EventInvitation.Create(guest.Id);

        // Act
        Result result = veaEvent.InviteGuest(invitation);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Contains(invitation, veaEvent.EventInvitations);
    }

    [Fact]
    public  async Task GivenEventInStatusReady_AndNotFull_AndNotStartedYet_WhenGuestIsInvited_ThenReturnsSuccessResult() {
        // Arrange with a public event and a valid guest
        VeaEvent veaEvent = EventFactory.GetReadyEvent();
        Guest guest =await GuestFactory.GetValidGuest();
        EventInvitation invitation = EventInvitation.Create(guest.Id);

        // Act
        Result result = veaEvent.InviteGuest(invitation);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Contains(invitation, veaEvent.EventInvitations);
    }

    [Fact]
    public  async Task GivenEventInStatusDraft_AndNotFull_AndNotStartedYet_WhenGuestIsInvited_ThenReturnsFailureResult_WithCorrectErrorMessage() {
        // Arrange with a public event and a valid guest
        VeaEvent veaEvent = EventFactory.GetDraftEvent();
        Guest guest =await GuestFactory.GetValidGuest();
        EventInvitation invitation = EventInvitation.Create(guest.Id);

        // Act
        Result result = veaEvent.InviteGuest(invitation);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessage.InvitationsCanOnlyBeMadeOnReadyOrActiveEvent, result.Error!.Messages);
    }

    [Fact]
    public  async Task GivenEventInStatusCancelled_AndNotFull_AndNotStartedYet_WhenGuestIsInvited_ThenReturnsFailureResult_WithCorrectErrorMessage() {
        // Arrange with a public event and a valid guest
        VeaEvent veaEvent = EventFactory.GetCancelledEvent();
        Guest guest = await GuestFactory.GetValidGuest();
        EventInvitation invitation = EventInvitation.Create(guest.Id);

        // Act
        Result result = veaEvent.InviteGuest(invitation);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessage.InvitationsCanOnlyBeMadeOnReadyOrActiveEvent, result.Error!.Messages);
    }

    [Fact]
    public  async Task GivenEventInStatusActive_AndFull_AndNotStartedYet_WhenGuestIsInvited_ThenReturnsFailureResult_WithCorrectErrorMessage() {
        // Arrange with a public event and a valid guest
        VeaEvent veaEvent = EventFactory.GetActiveEvent();
        EventFactory.ArrangeFullEvent(veaEvent);

        Guest guest = await GuestFactory.GetValidGuest();
        EventInvitation invitation = EventInvitation.Create(guest.Id);

        // Act
        Result result = veaEvent.InviteGuest(invitation);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessage.MaximumNumberOfGuestsReached, result.Error!.Messages);
    }
}