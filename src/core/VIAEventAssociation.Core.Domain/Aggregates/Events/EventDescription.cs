using VIAEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Events;

internal class EventDescription : ValueObject {
    internal string Value { get; private init; }

    private EventDescription() {
    }


    internal static EventDescription InitEmpty() {
        return new EventDescription() {
            Value = string.Empty
        };
    }

    internal static Result<EventDescription> From(string description) {
        return Result<EventDescription>.AsBuilder(ErrorCode.BadRequest, new EventDescription() {
                Value = description
            }).AssertWithError(() => LengthBetween0And250(description), ErrorMessage.DescriptionMustBeLessThan250Chars)
            .Build();
    }


    private static bool LengthBetween0And250(string description) {
        return description.Length <= 250;
    }


    protected override IEnumerable<object> GetEqualityComponents() {
        yield return Value;
    }
}