using VIAEventAssociation.Core.Domain.Aggregates.Guests;

namespace UnitTests.Fakes;

public class TestGuestRepo : TestRepo<Guest, GuestId> , IGuestRepository {
    
    public static TestGuestRepo With(Guest guest) {
        TestGuestRepo repo = new();
        repo.Values.Add(guest);
        return repo;
    }
}