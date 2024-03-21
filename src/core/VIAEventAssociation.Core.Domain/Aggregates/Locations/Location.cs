using VIAEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Locations;

public class Location : Aggregate<LocationId> {
    internal LocationName LocationName { get; private set; }
    internal LocationMaxGuests LocationMaxGuests { get; private set; }

    private Location(LocationId locationId, LocationName locationName) {
        Id = locationId;
        LocationName = locationName;
        LocationMaxGuests = LocationMaxGuests.Default();
    }

    public static Location Create(LocationName locationName) {
        return new Location(LocationId.New(), locationName);
    }

    public void UpdateLocationName(LocationName locationName) {
        LocationName = locationName;
    }

    public void UpdateLocationMaxGuests(LocationMaxGuests locationMaxGuests) {
        LocationMaxGuests = locationMaxGuests;
    }
}