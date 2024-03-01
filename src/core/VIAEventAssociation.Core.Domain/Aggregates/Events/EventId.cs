using VIAEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Events;

// Todo : Ask troels.. Is id an value object ??
public class EventId : ValueObject {
    internal Guid Value { get; private init; }


    private EventId(Guid value) {
        Value = value;
    }

    internal static EventId New() {
        return new EventId(Guid.NewGuid());
    }

    // Todo : Ask troels is this considered a business logic ?? Or should i throw an exception in this case
    internal static Result<EventId> From(string id) {
        Result<Guid> result = CanParseGuid(id);
        if (result.IsFailure) {
            return Error.BadRequest(ErrorMessage.UnParsableGuid);
        }

        return new EventId(result.Payload);
    }


    private static Result<Guid> CanParseGuid(string id) {
        bool canBeParsed = Guid.TryParse(id, out Guid guid);
        return canBeParsed ? guid : Error.BadRequest(ErrorMessage.UnParsableGuid);
    }

    protected override IEnumerable<object> GetEqualityComponents() {
        yield return Value;
    }
}