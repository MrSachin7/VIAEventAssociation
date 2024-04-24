using UnitTests.Common.Factories;
using UnitTests.Common.Stubs;
using UnitTests.Fakes;
using VIAEventAssociation.Core.AppEntry.Commands.Events;
using VIAEventAssociation.Core.Application.CommandHandlers.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Events.MakeActive;

public class MakeEventActiveHandlerTests {
        
    private readonly ISystemTime _systemTime = new TestSystemTime();
    
    [Fact]
    public async Task MakeEventActiveHandler_MakesEventActive() {
        // Arrange

        VeaEvent veaEvent = EventFactory.GetReadyEvent();

        // Make sure the repo contains the event 
        TestEventRepo eventRepo = TestEventRepo.With(veaEvent);

        MakeEventActiveCommand makeEventActiveCommand = 
            MakeEventActiveCommand.Create(veaEvent.Id.Value.ToString()).Payload!;

        // Act
        MakeEventActiveCommandHandler handler = new(eventRepo,_systemTime);
        Result result = await handler.HandleAsync(makeEventActiveCommand);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(EventStatus.Active, veaEvent.Status);

    }

    [Fact]
    public async Task MakeEventActiveHandler_WhenEventDoesNotExist_Fails() {
        // Arrange

        VeaEvent veaEvent = EventFactory.GetReadyEvent();

        // Make sure the repo DOES NOT contains the event 
        TestEventRepo eventRepo = new TestEventRepo();

        MakeEventActiveCommand makeEventActiveCommand = 
            MakeEventActiveCommand.Create(veaEvent.Id.Value.ToString()).Payload!;

        // Act
        MakeEventActiveCommandHandler handler = new(eventRepo,_systemTime);
        Result result = await handler.HandleAsync(makeEventActiveCommand);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessage.EventNotFound(veaEvent.Id.Value), result.Error!.Messages);

    }

}