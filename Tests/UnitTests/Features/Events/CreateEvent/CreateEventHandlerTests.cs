using UnitTests.Fakes;
using VIAEventAssociation.Core.AppEntry.Commands.Events;
using VIAEventAssociation.Core.Application.CommandHandlers.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Common.UnitOfWork;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Events.CreateEvent;

public class CreateEventHandlerTests {

    private readonly TestEventRepo _eventRepository = new TestEventRepo();
    private readonly IUnitOfWork _unitOfWork = new TestUnitOfWork();

    [Fact]
    public async Task CreateEventHandler_CreatesAnEmptyEvent() {
        // Arrange
        CreateEventCommand createEventCommand = CreateEventCommand.Create().Payload!;
        CreateEventCommandHandler handler = new CreateEventCommandHandler(_eventRepository, _unitOfWork);

        // Act
        Result result =await handler.Handle(createEventCommand);

        // Assert
        Assert.True(result.IsSuccess);
        // Make sure that the event is added to the repository
        Assert.Single( _eventRepository.Values);

    }
}