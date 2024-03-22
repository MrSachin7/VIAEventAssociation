using UnitTests.Common.Factories;
using UnitTests.Fakes;
using VIAEventAssociation.Core.AppEntry.Commands.Events;
using VIAEventAssociation.Core.Application.CommandHandlers.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Common.UnitOfWork;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Events.UpdateDescription;

public class UpdateEventDescriptionHandlerTests {
    private readonly IUnitOfWork _unitOfWork = new TestUnitOfWork();

    [Fact]
    public async Task UpdateEventDescriptionHandler_UpdatesEventDescription() {
        // Arrange

        VeaEvent veaEvent = EventFactory.GetDraftEvent();

        // Make sure the repo contains the event 
        TestEventRepo eventRepo = TestEventRepo.With(veaEvent);

        const string newDescription = "New Description";
        UpdateEventDescriptionCommand updateEventDescriptionCommand = 
            UpdateEventDescriptionCommand.Create(veaEvent.Id.Value.ToString(), newDescription).Payload!;

        // Act
        UpdateEventDescriptionCommandHandler handler = new(eventRepo, _unitOfWork);
        Result result = await handler.Handle(updateEventDescriptionCommand);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(newDescription, veaEvent.Description.Value);

    } 

    [Fact]
    public async Task UpdateEventDescriptionHandler_WhenEventDoesNotExist_Fails() {
        // Arrange

        VeaEvent veaEvent = EventFactory.GetDraftEvent();

        // Make sure the repo DOES NOT contains the event 
        TestEventRepo eventRepo = new TestEventRepo();

        const string newDescription = "New Description";
        UpdateEventDescriptionCommand updateEventDescriptionCommand = 
            UpdateEventDescriptionCommand.Create(veaEvent.Id.Value.ToString(), newDescription).Payload!;

        // Act
        UpdateEventDescriptionCommandHandler handler = new(eventRepo, _unitOfWork);
        Result result = await handler.Handle(updateEventDescriptionCommand);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessage.EventNotFound(veaEvent.Id.Value), result.Error!.Messages);

    }
  
}