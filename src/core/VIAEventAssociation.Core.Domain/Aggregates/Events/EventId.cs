using VIAEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Events;

public class EventId : Id {


    private EventId(Guid value) {
        Value = value;
    }


    internal static EventId New() {
        return new EventId(Guid.NewGuid());
    }

    public static Result<EventId> Create(string id) {
        Result<Guid> result = CanParseGuid(id);
        return result.IsFailure ? result.Error! : new EventId(result.Payload);
    }


 

}