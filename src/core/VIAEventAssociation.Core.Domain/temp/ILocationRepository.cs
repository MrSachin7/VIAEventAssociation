using VIAEventAssociation.Core.Domain.Aggregates.Locations;

namespace VIAEventAssociation.Core.Domain.temp;

// Todo: Pretty sure this doesnt belong here, will be moved later
public interface ILocationRepository {
    Location FindById(Guid locationIdValue);
}