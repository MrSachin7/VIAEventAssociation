using VIAEventAssociation.Core.Domain.Aggregates.Events;

namespace VIAEvent.Infrastructure.SqliteDmPersistence.VeaEventPersistence;

public class VeaEventRepository : RepositoryEfcBase<VeaEvent, EventId>, IEventRepository {

    public VeaEventRepository(SqliteWriteDbContext context) : base(context) {

    }
}