using VIAEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Events;

public class EventDescription : ValueObject {
    internal string Value { get; private init; }

    private EventDescription(string description) {
        Value = description;
    }


    internal static EventDescription Default() {
        const string defaultDescription ="";
        return new EventDescription(defaultDescription);
    }

    internal static Result<EventDescription> From(string description) {
        return Result<EventDescription>.AsBuilder(ErrorCode.BadRequest, new EventDescription(description))
            .AssertWithError(() => LengthBetween0And250(description), ErrorMessage.DescriptionMustBeLessThan250Chars)
            .Build();
    }


    private static bool LengthBetween0And250(string description) {
        return description.Length <= 250;
    }


    protected override IEnumerable<object> GetEqualityComponents() {
        yield return Value;
    }
}