using VIAEventAssociation.Core.Domain.Common.Repository;

namespace VIAEventAssociation.Core.Domain.Aggregates.Events;

public interface IEventRepository : IRepository<VeaEvent, EventId> {
    
}