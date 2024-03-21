using VIAEventAssociation.Core.Domain.Common.Repository;

namespace VIAEventAssociation.Core.Domain.Aggregates.Guests;

public interface IGuestRepository : IRepository<Guest, GuestId> {
    
}