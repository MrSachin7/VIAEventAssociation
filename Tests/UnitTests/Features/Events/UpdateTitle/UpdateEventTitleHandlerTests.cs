using UnitTests.Common.Factories;
using UnitTests.Fakes;
using VIAEventAssociation.Core.AppEntry.Commands.Events;
using VIAEventAssociation.Core.Application.CommandHandlers.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Common.UnitOfWork;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Events.UpdateTitle;

public class UpdateEventTitleHandlerTests {
    private readonly IUnitOfWork _unitOfWork = new TestUnitOfWork();

    [Fact]
    public async Task UpdateEventTitleHandler_UpdatesEventTitle() {
        // Arrange

        VeaEvent veaEvent = EventFactory.GetDraftEvent();

        // Make sure the repo contains the event 
        TestEventRepo eventRepo = TestEventRepo.With(veaEvent);

        const string title = "New Title";
        UpdateEventTitleCommand updateEventTitleCommand =
            UpdateEventTitleCommand.Create(veaEvent.Id.Value.ToString(), title).Payload!;

        // Act
        UpdateEventTitleCommandHandler handler = new(eventRepo, _unitOfWork);
        Result result = await handler.Handle(updateEventTitleCommand);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(title, veaEvent.Title.Value);
    }

    [Fact]
    public async Task UpdateEventTitleHandler_WhenEventDoesNotExist_Fails() {
        // Arrange

        VeaEvent veaEvent = EventFactory.GetDraftEvent();

        // Make sure the repo DOES NOT contains the event 
        TestEventRepo eventRepo = new TestEventRepo();

        const string title = "New Title";
        UpdateEventTitleCommand updateEventTitleCommand =
            UpdateEventTitleCommand.Create(veaEvent.Id.Value.ToString(), title).Payload!;

        // Act
        UpdateEventTitleCommandHandler handler = new(eventRepo, _unitOfWork);
        Result result = await handler.Handle(updateEventTitleCommand);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessage.EventNotFound(veaEvent.Id.Value), result.Error!.Messages);
    }
}