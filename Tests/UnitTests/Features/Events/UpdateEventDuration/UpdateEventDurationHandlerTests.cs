using UnitTests.Common.Factories;
using UnitTests.Common.Stubs;
using UnitTests.Fakes;
using VIAEventAssociation.Core.AppEntry.Commands.Events;
using VIAEventAssociation.Core.Application.CommandHandlers.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Common.UnitOfWork;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Events.UpdateEventDuration;

public class UpdateEventDurationHandlerTests {
    private readonly IUnitOfWork _unitOfWork = new TestUnitOfWork();


    [Fact]
    public async Task UpdateEventDurationHandler_UpdatesEventDuration() {
        // Arrange

        VeaEvent veaEvent = EventFactory.GetDraftEvent();

        // Make sure the repo contains the event 
        TestEventRepo eventRepo = TestEventRepo.With(veaEvent);

        EventDuration eventDuration = EventFactory.GetValidEventDuration();
        UpdateEventDurationCommand updateEventDurationCommand = 
            UpdateEventDurationCommand.Create(veaEvent.Id.Value.ToString(), eventDuration.StartDateTime, eventDuration.EndDateTime, new TestSystemTime()).Payload!;

        // Act
        UpdateEventDurationHandler handler = new(eventRepo, _unitOfWork);
        Result result = await handler.Handle(updateEventDurationCommand);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(eventDuration.StartDateTime, veaEvent.Duration!.StartDateTime);
        Assert.Equal(eventDuration.EndDateTime, veaEvent.Duration!.EndDateTime);

    }

    [Fact]
    public async Task UpdateEventDurationHandler_WhenEventDoesNotExist_Fails() {
        // Arrange

        VeaEvent veaEvent = EventFactory.GetDraftEvent();

        // Make sure the repo DOES NOT contains the event 
        TestEventRepo eventRepo = new TestEventRepo();

        EventDuration eventDuration = EventFactory.GetValidEventDuration();
        UpdateEventDurationCommand updateEventDurationCommand = 
            UpdateEventDurationCommand.Create(veaEvent.Id.Value.ToString(), eventDuration.StartDateTime, eventDuration.EndDateTime, new TestSystemTime()).Payload!;

        // Act
        UpdateEventDurationHandler handler = new(eventRepo, _unitOfWork);
        Result result = await handler.Handle(updateEventDurationCommand);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessage.EventNotFound(veaEvent.Id.Value), result.Error!.Messages);

    }
}