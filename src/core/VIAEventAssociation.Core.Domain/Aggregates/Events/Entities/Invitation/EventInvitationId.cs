using VIAEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Events.Entities.Invitation;

public class EventInvitationId : Id {

    private EventInvitationId(Guid value) {
        Value = value;
    }

    public static EventInvitationId New() {
        return new EventInvitationId(Guid.NewGuid());
    }

    public static Result<EventInvitationId> Create(string id) {
        Result<Guid> result = CanParseGuid(id);
        return result.IsFailure ? result.Error! : new EventInvitationId(result.Payload);
    }


    
}