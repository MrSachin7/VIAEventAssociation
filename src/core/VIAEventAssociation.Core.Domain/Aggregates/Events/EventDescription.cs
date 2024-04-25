using VIAEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Events;

public class EventDescription : ValueObject {
    internal string Value { get; }

    private EventDescription(string description) {
        Value = description;
    }

    private EventDescription() {
        // Efc needs this
    }

    internal static EventDescription Default() {
        const string defaultDescription = "";
        return new EventDescription(defaultDescription);
    }

    public static Result<EventDescription> Create(string description) {
        return Result.ToBuilder(ErrorCode.BadRequest)
            .AssertWithError(() => LengthBetween0And250(description), ErrorMessage.DescriptionMustBeLessThan250Chars)
            .WithPayload(new EventDescription(description))
            .Build();
    }


    private static bool LengthBetween0And250(string description) {
        return description.Length <= 250;
    }


    protected override IEnumerable<object> GetEqualityComponents() {
        yield return Value;
    }
}