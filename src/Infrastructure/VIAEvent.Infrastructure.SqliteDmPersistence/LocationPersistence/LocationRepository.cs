using VIAEventAssociation.Core.Domain.Aggregates.Locations;

namespace VIAEvent.Infrastructure.SqliteDmPersistence.LocationPersistence;

public class LocationRepository : RepositoryEfcBase<Location, LocationId>, ILocationRepository {
    public LocationRepository(SqliteWriteDbContext context) : base(context) {
    }
}