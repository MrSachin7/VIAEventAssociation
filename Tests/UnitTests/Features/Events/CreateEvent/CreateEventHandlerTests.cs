using UnitTests.Fakes;
using VIAEventAssociation.Core.AppEntry.Commands.Events;
using VIAEventAssociation.Core.Application.CommandHandlers.Events;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Events.CreateEvent;

public class CreateEventHandlerTests {

    private readonly TestEventRepo _eventRepository = new TestEventRepo();

    [Fact]
    public async Task CreateEventHandler_CreatesAnEmptyEvent() {
        // Arrange
        CreateEventCommand createEventCommand = CreateEventCommand.Create();
        CreateEventCommandHandler handler = new CreateEventCommandHandler(_eventRepository);

        // Act
        Result result =await handler.HandleAsync(createEventCommand);

        // Assert
        Assert.True(result.IsSuccess);
        // Make sure that the event is added to the repository
        Assert.Single( _eventRepository.Values);

    }
}