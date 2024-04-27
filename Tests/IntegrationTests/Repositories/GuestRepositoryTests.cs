using VIAEvent.Infrastructure.SqliteDmPersistence;
using VIAEvent.Infrastructure.SqliteDmPersistence.Contracts;
using VIAEvent.Infrastructure.SqliteDmPersistence.GuestPersistence;
using VIAEvent.Infrastructure.SqliteDmPersistence.UnitOfWork;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using VIAEventAssociation.Core.Domain.Common.UnitOfWork;
using VIAEventAssociation.Core.Domain.Contracts;

namespace IntegrationTests.Repositories;

public class GuestRepositoryTests : WriteContextTestBase {
    private readonly IGuestRepository _guestRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUniqueEmailChecker _uniqueEmailChecker;

    public GuestRepositoryTests() {
        SqliteWriteDbContext context = SetupContext();
        _guestRepository = new GuestRepository(context);
        _unitOfWork = new SqliteUnitOfWork(context);
        _uniqueEmailChecker = new UniqueEmailChecker(context);
    }

    [Fact]
    public async Task AddGuestAsync_AddsGuestToDatabase() {
        // Arrange
        Guest guest = Guest.Create(
            GuestFirstName.Create("Sachin").Payload!,
            GuestLastName.Create("Baral").Payload!,
            (await ViaEmail.Create("310628@via.dk", _uniqueEmailChecker)).Payload!).Payload!;

        // Act
        await _guestRepository.AddAsync(guest);
        await _unitOfWork.SaveChangesAsync();

        // Assert
        Guest? addedGuest = await _guestRepository.FindAsync(guest.Id);
        Assert.NotNull(addedGuest);
        Assert.Equal(guest, addedGuest);
    }


    [Fact]
    public async Task FindAsync_ReturnsNull_WhenGuestDoesNotExist() {
        // Act
        Guest? guest = await _guestRepository.FindAsync(GuestId.New());
        // Assert
        Assert.Null(guest);
    }

    [Fact]
    public async Task Remove_RemovesGuestFromDatabase() {
        // Arrange
        Guest guest = Guest.Create(
            GuestFirstName.Create("Sachin").Payload!,
            GuestLastName.Create("Baral").Payload!,
            (await ViaEmail.Create("310628@via.dk", _uniqueEmailChecker)).Payload!).Payload!;

        await _guestRepository.AddAsync(guest);
        await _unitOfWork.SaveChangesAsync();

        // Act
        _guestRepository.Remove(guest);
        await _unitOfWork.SaveChangesAsync();

        // Assert
        Guest? removedGuest = await _guestRepository.FindAsync(guest.Id);
        Assert.Null(removedGuest);
    }
}