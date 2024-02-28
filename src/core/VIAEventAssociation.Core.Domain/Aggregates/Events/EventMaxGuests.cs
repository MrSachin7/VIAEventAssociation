using VIAEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Events;

internal class EventMaxGuests : ValueObject {
    internal int Value { get; private init; }

    private EventMaxGuests() {
    }

    internal static Result<EventMaxGuests> From(int maximumGuests) {
        return Result<EventMaxGuests>.AsBuilder(ErrorCode.BadRequest, new EventMaxGuests() {
                Value = maximumGuests
            })
            .AssertWithError(() => NoLessThan5(maximumGuests), ErrorMessage.MaxGuestsNotLessThan5)
            .AssertWithError(() => NoMoreThan50(maximumGuests), ErrorMessage.MaxGuestsNotMoreThan50)
            .Build();
    }

    private static bool NoLessThan5(int maxGuests) {
        return maxGuests >= 5;
    }

    private static bool NoMoreThan50(int maxGuests) {
        return maxGuests <= 50;
    }

    protected override IEnumerable<object> GetEqualityComponents() {
        yield return Value;
    }
}