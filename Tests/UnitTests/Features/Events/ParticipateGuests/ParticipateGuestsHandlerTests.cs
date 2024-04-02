using UnitTests.Common.Factories;
using UnitTests.Common.Stubs;
using UnitTests.Fakes;
using VIAEventAssociation.Core.AppEntry.Commands.Events;
using VIAEventAssociation.Core.Application.CommandHandlers.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using VIAEventAssociation.Core.Domain.Common.UnitOfWork;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Events.ParticipateGuests;

public class ParticipateGuestsHandlerTests {
    
    private readonly IUnitOfWork _unitOfWork = new TestUnitOfWork();
    private readonly ISystemTime _systemTime = new TestSystemTime();
    
    [Fact]
    public async Task ParticipateGuestHandler_ParticipatedGuestsToEvent() {
        // Arrange

        // Ready event so that we can participate guests
        VeaEvent veaEvent = EventFactory.GetActiveEvent();
        veaEvent.MakePublic();
        Guest guest = await GuestFactory.GetValidGuest();

        // Make sure the repo contains the event and the guest.
        TestEventRepo eventRepo = TestEventRepo.With(veaEvent);
        TestGuestRepo guestRepo = TestGuestRepo.With(guest);

        ParticipateGuestCommand inviteGuestCommand = 
            ParticipateGuestCommand.Create(veaEvent.Id.Value.ToString(),
                guest.Id.Value.ToString()).Payload!;

        // Act
        ParticipateGuestCommandHandler handler = new(eventRepo, guestRepo, _unitOfWork,_systemTime);
        Result result = await handler.Handle(inviteGuestCommand);

        // Assert
        Assert.True(result.IsSuccess);
        // Assert that the guest is participated to the event
        Assert.Contains(guest.Id, veaEvent.IntendedParticipants);

    }

    [Fact]
    public async Task ParticipateGuestHandler_WhenGuestDoesNotExist_Fails() {
        // Arrange

        // Ready event so that we can participate guests
        VeaEvent veaEvent = EventFactory.GetReadyEvent();
        Guest guest = await GuestFactory.GetValidGuest();

        // Make sure the repo contains the event and **NOT** the guest.
        TestEventRepo eventRepo = TestEventRepo.With(veaEvent);
        TestGuestRepo guestRepo = new TestGuestRepo();

        ParticipateGuestCommand participateGuestCommand = 
            ParticipateGuestCommand.Create(veaEvent.Id.Value.ToString(),
                guest.Id.Value.ToString()).Payload!;

        // Act
        ParticipateGuestCommandHandler handler = new(eventRepo, guestRepo, _unitOfWork,_systemTime);
        Result result = await handler.Handle(participateGuestCommand);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessage.GuestNotFound(guest.Id.Value), result.Error!.Messages);
        // Assert that the guest is not participated to the event
        Assert.DoesNotContain(guest.Id, veaEvent.IntendedParticipants);

    }

    [Fact]
    public async Task ParticipateGuestHandler_WhenEventDoesNotExist_Fails() {
        // Arrange

        // Ready event so that we can participate guests
        VeaEvent veaEvent = EventFactory.GetReadyEvent();
        Guest guest = await GuestFactory.GetValidGuest();

        // Make sure the repo contains the gusts and **NOT** the event.
        TestEventRepo eventRepo = new TestEventRepo();
        TestGuestRepo guestRepo = TestGuestRepo.With(guest);

        ParticipateGuestCommand participateGuestCommand = 
            ParticipateGuestCommand.Create(veaEvent.Id.Value.ToString(),
                guest.Id.Value.ToString()).Payload!;

        // Act
        ParticipateGuestCommandHandler handler = new(eventRepo, guestRepo, _unitOfWork,_systemTime);
        Result result = await handler.Handle(participateGuestCommand);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessage.EventNotFound(veaEvent.Id.Value), result.Error!.Messages);
        // Assert that the guest is not participated to the event
        Assert.DoesNotContain(guest.Id, veaEvent.IntendedParticipants);

    }
}