using VIAEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Events;

// Todo : Ask troels.. Is id an value object ??
public class EventId : Id {


    private EventId(Guid value) {
        Value = value;
    }


    internal static EventId New() {
        return new EventId(Guid.NewGuid());
    }

    // Todo : Ask troels is this considered a business logic ?? Or should i throw an exception in this case
    internal static Result<EventId> From(string id) {
        Result<Guid> result = CanParseGuid(id);
        return result.IsFailure ? result.Error! : new EventId(result.Payload);
    }


 

}