using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Common.Bases;

public abstract class Id : ValueObject {
    public Guid Value { get; init; }

    protected Id() {

    }

    protected override IEnumerable<object> GetEqualityComponents() {
        yield return Value;
    }

    protected static Result<Guid> CanParseGuid(string id) {
        bool canBeParsed = Guid.TryParse(id, out Guid guid);
        return canBeParsed ? guid : Error.BadRequest(ErrorMessage.UnParsableGuid);
    }

}