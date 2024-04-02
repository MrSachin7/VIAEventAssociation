using UnitTests.Common.Factories;
using UnitTests.Fakes;
using VIAEventAssociation.Core.AppEntry.Commands.Events;
using VIAEventAssociation.Core.Application.CommandHandlers.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Common.UnitOfWork;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Events.UpdateMaxGuests;

public class UpdateEventMaxGuestsHandlerTests {
    private readonly IUnitOfWork _unitOfWork = new TestUnitOfWork();

    [Fact]
    public async Task UpdateEventMaxGuestsHandler_UpdatesEventMaxGuests() {
        // Arrange

        VeaEvent veaEvent = EventFactory.GetReadyEvent();

        // Make sure the repo contains the event 
        TestEventRepo eventRepo = TestEventRepo.With(veaEvent);

        int maxGuests = 10;
        UpdateEventMaxGuestsCommand updateEventMaxGuestsCommand = 
            UpdateEventMaxGuestsCommand.Create(veaEvent.Id.Value.ToString(), maxGuests).Payload!;

        // Act
        UpdateEventMaxGuestsCommandHandler handler = new(eventRepo, _unitOfWork);
        Result result = await handler.Handle(updateEventMaxGuestsCommand);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(maxGuests, veaEvent.MaxGuests.Value);

    }

    [Fact]
    public async Task UpdateEventMaxGuestsHandler_WhenEventDoesNotExist_Fails() {
        // Arrange

        VeaEvent veaEvent = EventFactory.GetDraftEvent();

        // Make sure the repo DOES NOT contains the event 
        TestEventRepo eventRepo = new TestEventRepo();

        int maxGuests = 10;
        UpdateEventMaxGuestsCommand updateEventMaxGuestsCommand = 
            UpdateEventMaxGuestsCommand.Create(veaEvent.Id.Value.ToString(), maxGuests).Payload!;

        // Act
        UpdateEventMaxGuestsCommandHandler handler = new(eventRepo, _unitOfWork);
        Result result = await handler.Handle(updateEventMaxGuestsCommand);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessage.EventNotFound(veaEvent.Id.Value), result.Error!.Messages);
    }
}