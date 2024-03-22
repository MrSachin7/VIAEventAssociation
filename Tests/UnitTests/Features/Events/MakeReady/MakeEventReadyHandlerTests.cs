using UnitTests.Common.Factories;
using UnitTests.Common.Stubs;
using UnitTests.Fakes;
using VIAEventAssociation.Core.AppEntry.Commands.Events;
using VIAEventAssociation.Core.Application.CommandHandlers.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Locations;
using VIAEventAssociation.Core.Domain.Common.UnitOfWork;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Events.MakeReady;

public class MakeEventReadyHandlerTests {
    private readonly IUnitOfWork _unitOfWork = new TestUnitOfWork();
    private readonly ISystemTime _systemTime = new TestSystemTime();

    [Fact]
    public async Task MakeEventReadyHandler_MakesEventActive() {
        // Arrange

        VeaEvent veaEvent = EventFactory.GetDraftEvent();
        veaEvent.UpdateEventDescription(EventFactory.GetValidEventDescription());
        veaEvent.UpdateEventTitle(EventFactory.GetValidEventTitle());
        veaEvent.UpdateEventDuration(EventFactory.GetValidEventDuration());
        veaEvent.UpdateLocation(Location.Create(LocationName.Create("C02.03").Payload!,
            LocationMaxGuests.Create(50).Payload!));

        // Make sure the repo contains the event 
        TestEventRepo eventRepo = TestEventRepo.With(veaEvent);

        MakeEventReadyCommand makeEventReadyCommand =
            MakeEventReadyCommand.Create(veaEvent.Id.Value.ToString()).Payload!;

        // Act
        MakeEventReadyCommandHandler handler = new(eventRepo, _unitOfWork, _systemTime);
        Result result = await handler.Handle(makeEventReadyCommand);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(EventStatus.Ready, veaEvent.Status);
    }

    [Fact]
    public async Task MakeEventReadyHandler_WhenEventDoesNotExist_Fails() {
        // Arrange

        VeaEvent veaEvent = EventFactory.GetDraftEvent();
        veaEvent.UpdateEventDescription(EventFactory.GetValidEventDescription());
        veaEvent.UpdateEventTitle(EventFactory.GetValidEventTitle());
        veaEvent.UpdateEventDuration(EventFactory.GetValidEventDuration());
        veaEvent.UpdateLocation(Location.Create(LocationName.Create("C02.03").Payload!,
            LocationMaxGuests.Create(50).Payload!));

        // Make sure the repo DOES NOT contains the event 
        TestEventRepo eventRepo = new TestEventRepo();

        MakeEventReadyCommand makeEventReadyCommand =
            MakeEventReadyCommand.Create(veaEvent.Id.Value.ToString()).Payload!;

        // Act
        MakeEventReadyCommandHandler handler = new(eventRepo, _unitOfWork, _systemTime);
        Result result = await handler.Handle(makeEventReadyCommand);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessage.EventNotFound(veaEvent.Id.Value), result.Error!.Messages);
    }
}