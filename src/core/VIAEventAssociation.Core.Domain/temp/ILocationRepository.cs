using VIAEventAssociation.Core.Domain.Aggregates.Locations;

namespace VIAEventAssociation.Core.Domain.temp;

// Todo: Pretty sure this doesnt belong here, will be moved later
public interface ILocationRepository {
    
    // Todo: I need to make it async , so does that mean everything that uses this must be async as well ?
    Task<Location> FindById(Guid locationIdValue);
}