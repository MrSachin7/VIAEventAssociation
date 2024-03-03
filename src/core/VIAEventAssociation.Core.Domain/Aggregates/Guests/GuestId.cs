using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Guests;

public class GuestId : Id  {

   
    private GuestId(Guid value) {
        Value = value;
    }

    internal static GuestId New() {
        return new GuestId(Guid.NewGuid());
    }

    internal static Result<GuestId> From(string id) {
        Result<Guid> result = CanParseGuid(id);
        return result.IsFailure ? result.Error! : new GuestId(result.Payload);
    }
}