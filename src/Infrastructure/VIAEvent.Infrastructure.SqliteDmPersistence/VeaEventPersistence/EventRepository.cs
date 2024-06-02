using VIAEventAssociation.Core.Domain.Aggregates.Events;

namespace VIAEvent.Infrastructure.SqliteDmPersistence.VeaEventPersistence;

public class EventRepository : RepositoryEfcBase<VeaEvent, EventId>, IEventRepository {

    public EventRepository(SqliteWriteDbContext context) : base(context) {

    }
}