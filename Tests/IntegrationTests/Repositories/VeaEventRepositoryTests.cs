using VIAEvent.Infrastructure.SqliteDmPersistence;
using VIAEvent.Infrastructure.SqliteDmPersistence.UnitOfWork;
using VIAEvent.Infrastructure.SqliteDmPersistence.VeaEventPersistence;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Common.UnitOfWork;

namespace IntegrationTests.Repositories;


public class EventRepositoryTests : WriteContextTestBase {

    private readonly IEventRepository _eventRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EventRepositoryTests() {
        SqliteWriteDbContext context = SetupContext();
        _eventRepository = new EventRepository(context);
        _unitOfWork = new SqliteUnitOfWork(context);
    }

    [Fact]
    public async Task AddEventAsync_AddsEventToDatabase() {
        // Arrange
        VeaEvent veaEvent = VeaEvent.Empty(EventId.New());
        // Act
        await _eventRepository.AddAsync(veaEvent);
        await _unitOfWork.SaveChangesAsync();
        // Assert
        VeaEvent? addedEvent = await _eventRepository.FindAsync(veaEvent.Id);
        Assert.NotNull(addedEvent);
        Assert.Equal(veaEvent, addedEvent);
    }

    [Fact]
    public async Task FindAsync_ReturnsNull_WhenEventDoesNotExist() {
        // Act
        VeaEvent? veaEvent = await _eventRepository.FindAsync(EventId.New());
        // Assert
        Assert.Null(veaEvent);
    }

    [Fact]
    public async Task Remove_RemovesEventFromDatabase() {
        // Arrange
        VeaEvent veaEvent = VeaEvent.Empty(EventId.New());
        await _eventRepository.AddAsync(veaEvent);
        await _unitOfWork.SaveChangesAsync();
        // Act
        _eventRepository.Remove(veaEvent);
        await _unitOfWork.SaveChangesAsync();
        // Assert
        VeaEvent? removedEvent = await _eventRepository.FindAsync(veaEvent.Id);
        Assert.Null(removedEvent);
    }


}