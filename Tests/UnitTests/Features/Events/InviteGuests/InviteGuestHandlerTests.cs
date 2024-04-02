using Moq;
using UnitTests.Common.Factories;
using UnitTests.Fakes;
using VIAEventAssociation.Core.AppEntry.Commands.Events;
using VIAEventAssociation.Core.Application.CommandHandlers.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using VIAEventAssociation.Core.Domain.Common.UnitOfWork;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Events.InviteGuests;

public class InviteGuestHandlerTests {

    private readonly IUnitOfWork _unitOfWork = new TestUnitOfWork();
    
    [Fact]
    public async Task InviteGuestHandler_InvitesGuestsToEvent() {
        // Arrange

        // Ready event so that we can invite guests
        VeaEvent veaEvent = EventFactory.GetReadyEvent();
        Guest guest = await GuestFactory.GetValidGuest();

        // Make sure the repo contains the event and the guest.
        TestEventRepo eventRepo = TestEventRepo.With(veaEvent);
        TestGuestRepo guestRepo = TestGuestRepo.With(guest);

        InviteGuestCommand inviteGuestCommand = 
            InviteGuestCommand.Create(veaEvent.Id.Value.ToString(),
                guest.Id.Value.ToString()).Payload!;

        // Act
        InviteGuestCommandHandler handler = new(eventRepo, guestRepo, _unitOfWork);
        Result result = await handler.Handle(inviteGuestCommand);

        // Assert
        Assert.True(result.IsSuccess);
        // Assert that the guest is invited to the event
        Assert.Contains(guest.Id, veaEvent.EventInvitations.Select(x => x.GuestId));

    }

    [Fact]
    public async Task InviteGuestHandler_WhenGuestDoesNotExist_Fails() {
        // Arrange

        // Ready event so that we can invite guests
        VeaEvent veaEvent = EventFactory.GetReadyEvent();
        Guest guest = await GuestFactory.GetValidGuest();

        // Make sure the repo doesnot contain the guest 
        TestEventRepo eventRepo = TestEventRepo.With(veaEvent);
        TestGuestRepo guestRepo = new TestGuestRepo();

        InviteGuestCommand inviteGuestCommand = 
            InviteGuestCommand.Create(veaEvent.Id.Value.ToString(),
                guest.Id.Value.ToString()).Payload!;

        // Act
        InviteGuestCommandHandler handler = new(eventRepo, guestRepo, _unitOfWork);
        Result result = await handler.Handle(inviteGuestCommand);

        // Assert
        Assert.True(result.IsFailure);
        // Correct error message is returned
        Assert.Contains(ErrorMessage.GuestNotFound(guest.Id.Value), result.Error!.Messages);
        // Assert that the guest is not invited to the event
        Assert.DoesNotContain(guest.Id, veaEvent.EventInvitations.Select(x => x.GuestId));

    }

    [Fact]
    public async Task InviteGuestHandler_WhenEventDoesNotExist_Fails() {
        // Arrange

        // Ready event so that we can invite guests
        VeaEvent veaEvent = EventFactory.GetReadyEvent();
        Guest guest = await GuestFactory.GetValidGuest();

        // Make sure the repo doesnot contain the event 
        TestEventRepo eventRepo = new TestEventRepo();
        TestGuestRepo guestRepo = TestGuestRepo.With(guest);

        InviteGuestCommand inviteGuestCommand = 
            InviteGuestCommand.Create(veaEvent.Id.Value.ToString(),
                guest.Id.Value.ToString()).Payload!;

        // Act
        InviteGuestCommandHandler handler = new(eventRepo, guestRepo, _unitOfWork);
        Result result = await handler.Handle(inviteGuestCommand);

        // Assert
        Assert.True(result.IsFailure);
        // Correct error message is returned
        Assert.Contains(ErrorMessage.EventNotFound(veaEvent.Id.Value), result.Error!.Messages);
        // Assert that the guest is not invited to the event
        Assert.DoesNotContain(guest.Id, veaEvent.EventInvitations.Select(x => x.GuestId));

    }

}