using VIAEventAssociation.Core.Domain.Aggregates.Guests;

namespace VIAEvent.Infrastructure.SqliteDmPersistence.GuestPersistence;

public class GuestRepository : RepositoryEfcBase<Guest, GuestId>, IGuestRepository {

    public GuestRepository(SqliteWriteDbContext context) : base(context) {
    }
}