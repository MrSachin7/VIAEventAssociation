using VIAEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Locations;

public class LocationId : Id {
    private LocationId(Guid value) {
        Value = value;
    }

    internal static LocationId New() {
        return new LocationId(Guid.NewGuid());
    }

    internal static Result<LocationId> From(string id) {
        Result<Guid> result = CanParseGuid(id);
        return result.IsFailure ? result.Error! : new LocationId(result.Payload);
    }

    public Guid GetValue() {
        return Value;
    }


    public static LocationId FromGuid(Guid value) {
        return new LocationId(value);
    }
}