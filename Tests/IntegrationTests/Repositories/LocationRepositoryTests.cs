using VIAEvent.Infrastructure.SqliteDmPersistence;
using VIAEvent.Infrastructure.SqliteDmPersistence.LocationPersistence;
using VIAEvent.Infrastructure.SqliteDmPersistence.UnitOfWork;
using VIAEventAssociation.Core.Domain.Aggregates.Locations;
using VIAEventAssociation.Core.Domain.Common.UnitOfWork;

namespace IntegrationTests.Repositories;

public class LocationRepositoryTests : DatabaseSetupTestHelper {
    
    private readonly ILocationRepository _locationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LocationRepositoryTests() {
        SqliteWriteDbContext context = SetupContext();
        _locationRepository = new LocationRepository(context);
        _unitOfWork = new SqliteUnitOfWork(context);
    }

    [Fact]
    public async Task AddLocationAsync_AddsLocationToDatabase() {
        // Arrange
        Location location = Location.Create(
            LocationName.Create("A03.02").Payload!,
            LocationMaxGuests.Create(10).Payload!
            );
        // Act
        await _locationRepository.AddAsync(location);
        await _unitOfWork.SaveChangesAsync();
        // Assert
        Location? addedLocation = await _locationRepository.FindAsync(location.Id);
        Assert.NotNull(addedLocation);
        Assert.Equal(location, addedLocation);
    }

    [Fact]
    public async Task FindAsync_ReturnsNull_WhenLocationDoesNotExist() {
        // Act
        Location? location = await _locationRepository.FindAsync(LocationId.New());
        // Assert
        Assert.Null(location);
    }

    [Fact]
    public async Task Remove_RemovesLocationFromDatabase() {
        // Arrange
        Location location = Location.Create(
            LocationName.Create("A03.02").Payload!,
            LocationMaxGuests.Create(10).Payload!
            );
        await _locationRepository.AddAsync(location);
        await _unitOfWork.SaveChangesAsync();
        // Act
        _locationRepository.Remove(location);
        await _unitOfWork.SaveChangesAsync();
        // Assert
        Location? removedLocation = await _locationRepository.FindAsync(location.Id);
        Assert.Null(removedLocation);
    }
}