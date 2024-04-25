using VIAEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Events;

public class EventTitle : ValueObject{
    internal string Value { get; private init; }

    private EventTitle(string title) => Value = title;

    private EventTitle() {
        // Efc needs this
    }

    public static Result<EventTitle> Create(string title) {
        return Result.ToBuilder(ErrorCode.BadRequest)
            .AssertWithError(() => TitleBetween3And75Inclusive(title), ErrorMessage.TitleMustBeBetween3And75Chars)
            .WithPayload(new EventTitle(title))
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