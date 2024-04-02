using VIAEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Guests;

public class GuestFirstName : ValueObject {
    public string Value { get; set; }

    private GuestFirstName(string firstName) {
        Value = char.ToUpper(firstName[0]) + firstName[1..].ToLower();

    }

    public static Result<GuestFirstName> Create(string firstName ) {
        firstName = firstName.Trim();

        if (!FirstNameBetween2And25Characters(firstName)) {
            return Error.BadRequest(ErrorMessage.FirstNameMustBeBetween2And25Chars);
        }

        if (!FirstNameIsOnlyLetters(firstName)) {
            return Error.BadRequest(ErrorMessage.FirstNameCanOnlyContainsLetters);
        }

        return new GuestFirstName(firstName);
    }

    private static bool FirstNameBetween2And25Characters(string name) {
        return !string.IsNullOrWhiteSpace(name) && name.Length is >= 2 and <= 25;            
    }

    private static bool FirstNameIsOnlyLetters(string name) {
        return !string.IsNullOrWhiteSpace(name) && name.All(char.IsLetter);
    }



    protected override IEnumerable<object> GetEqualityComponents() {
        yield return Value;
    }
}