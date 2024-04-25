using VIAEventAssociation.Core.Domain.Common.Bases;

namespace VIAEventAssociation.Core.Domain.Aggregates.Locations;

public class Location : Aggregate<LocationId> {
    internal LocationName LocationName { get; private set; }
    internal LocationMaxGuests LocationMaxGuests { get; private set; }

    private Location(LocationId locationId, LocationName locationName, LocationMaxGuests locationMaxGuests) {
        Id = locationId;
        LocationName = locationName;
        LocationMaxGuests = locationMaxGuests;
    }

    private Location() {
        // Efc needs this
    }

    public static Location Create(LocationName locationName, LocationMaxGuests locationMaxGuests) {
        return new Location(LocationId.New(), locationName, locationMaxGuests);
    }

    public void UpdateLocationName(LocationName locationName) {
        LocationName = locationName;
    }

    public void UpdateLocationMaxGuests(LocationMaxGuests locationMaxGuests) {
        LocationMaxGuests = locationMaxGuests;
    }
}