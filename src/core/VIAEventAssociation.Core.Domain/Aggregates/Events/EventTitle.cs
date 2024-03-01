using VIAEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Events;

public class EventTitle : ValueObject{
    internal string Value { get; private init; }

    private EventTitle(string title) => Value = title;

    internal static Result<EventTitle> From(string title) {
        return Result<EventTitle>.AsBuilder(ErrorCode.BadRequest, new EventTitle(title))
            .AssertWithError(() => TitleBetween3And75Inclusive(title), ErrorMessage.TitleMustBeBetween3And75Chars)
            .Build();
    }

    internal static EventTitle Default() {
        const string defaultTitle = "Working Title";
        return new EventTitle(defaultTitle);
    }

    private static bool TitleBetween3And75Inclusive(string title) {
        if (string.IsNullOrEmpty(title)) {
            return false;
        }

        if (title.Length is < 3 or > 75) {
            return false;
        }

        return true;
    }

    protected override IEnumerable<object> GetEqualityComponents() {
        yield return Value;
    }
}