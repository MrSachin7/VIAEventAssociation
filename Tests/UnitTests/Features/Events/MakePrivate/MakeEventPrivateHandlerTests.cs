using UnitTests.Common.Factories;
using UnitTests.Common.Stubs;
using UnitTests.Fakes;
using VIAEventAssociation.Core.AppEntry.Commands.Events;
using VIAEventAssociation.Core.Application.CommandHandlers.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Common.UnitOfWork;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Events.MakePrivate;

public class MakeEventPrivateHandlerTests {
    private readonly IUnitOfWork _unitOfWork = new TestUnitOfWork();
    
    [Fact]
    public async Task MakeEventPrivateHandler_MakesEventPrivate() {
        // Arrange

        VeaEvent veaEvent = EventFactory.GetReadyEvent();

        // Make sure the repo contains the event 
        TestEventRepo eventRepo = TestEventRepo.With(veaEvent);

        MakeEventPrivateCommand makeEventActiveCommand = 
            MakeEventPrivateCommand.Create(veaEvent.Id.Value.ToString()).Payload!;

        // Act
        MakeEventPrivateCommandHandler handler = new(eventRepo, _unitOfWork);
        Result result = await handler.Handle(makeEventActiveCommand);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(EventVisibility.Private, veaEvent.Visibility);

    }

    [Fact]
    public async Task MakeEventPrivateHandler_WhenEventDoesNotExist_Fails() {
        // Arrange

        VeaEvent veaEvent = EventFactory.GetReadyEvent();

        // Make sure the repo DOES NOT contains the event 
        TestEventRepo eventRepo = new TestEventRepo();

        MakeEventPrivateCommand makeEventPrivateCommand = 
            MakeEventPrivateCommand.Create(veaEvent.Id.Value.ToString()).Payload!;

        // Act
        MakeEventPrivateCommandHandler handler = new(eventRepo, _unitOfWork);
        Result result = await handler.Handle(makeEventPrivateCommand);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessage.EventNotFound(veaEvent.Id.Value), result.Error!.Messages);

    }
}