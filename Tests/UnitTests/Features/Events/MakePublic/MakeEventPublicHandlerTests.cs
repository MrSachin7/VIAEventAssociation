using UnitTests.Common.Factories;
using UnitTests.Fakes;
using VIAEventAssociation.Core.AppEntry.Commands.Events;
using VIAEventAssociation.Core.Application.CommandHandlers.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Common.UnitOfWork;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Events.MakePublic;

public class MakeEventPublicHandlerTests {
    private readonly IUnitOfWork _unitOfWork = new TestUnitOfWork();
    
    [Fact]
    public async Task MakeEventPublicHandler_MakesEventPublic() {
        // Arrange

        VeaEvent veaEvent = EventFactory.GetReadyEvent();

        // Make sure the repo contains the event 
        TestEventRepo eventRepo = TestEventRepo.With(veaEvent);

        MakeEventPublicCommand makeEventPublicCommand = 
            MakeEventPublicCommand.Create(veaEvent.Id.Value.ToString()).Payload!;

        // Act
        MakeEventPublicCommandHandler handler = new(eventRepo, _unitOfWork);
        Result result = await handler.Handle(makeEventPublicCommand);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(EventVisibility.Public, veaEvent.Visibility);

    }

    [Fact]
    public async Task MakeEventPublicHandler_WhenEventDoesNotExist_Fails() {
        // Arrange

        VeaEvent veaEvent = EventFactory.GetReadyEvent();

        // Make sure the repo DOES NOT contains the event 
        TestEventRepo eventRepo = new TestEventRepo();

        MakeEventPublicCommand makeEventPublicCommand = 
            MakeEventPublicCommand.Create(veaEvent.Id.Value.ToString()).Payload!;

        // Act
        MakeEventPublicCommandHandler handler = new(eventRepo, _unitOfWork);
        Result result = await handler.Handle(makeEventPublicCommand);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessage.EventNotFound(veaEvent.Id.Value), result.Error!.Messages);

    }
}