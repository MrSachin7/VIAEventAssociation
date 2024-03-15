using VIAEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Events;

public class EventMaxGuests : ValueObject {
    internal int Value { get; private init; }

    private EventMaxGuests(int maxGuests) {
        Value = maxGuests;
    }

    internal static Result<EventMaxGuests> Create(int maximumGuests) {
        return Result.ToBuilder(ErrorCode.BadRequest)
            .AssertWithError(() => NoLessThan5(maximumGuests), ErrorMessage.MaxGuestsNotLessThan5)
            .AssertWithError(() => NoMoreThan50(maximumGuests), ErrorMessage.MaxGuestsNotMoreThan50)
            .WithPayload(new EventMaxGuests(maximumGuests))
            .Build();
    }

    internal static EventMaxGuests Default() {
        const int defaultMaxGuests = 5;
        return new EventMaxGuests(defaultMaxGuests);
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