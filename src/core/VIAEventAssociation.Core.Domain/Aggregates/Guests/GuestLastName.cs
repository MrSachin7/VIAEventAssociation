using VIAEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Guests;

public class GuestLastName : ValueObject{
    public string Value { get; set; }

    private GuestLastName(string lastName) {
        lastName = lastName.Trim();
        Value = char.ToUpper(lastName[0]) + lastName[1..].ToLower();

    }

    public static Result<GuestLastName> Create(string lastName ) {
        lastName = lastName.Trim();

        if (!FirstNameBetween2And25Characters(lastName)) {
            return Error.BadRequest(ErrorMessage.FirstNameMustBeBetween2And25Chars);
        }

        if (!FirstNameIsOnlyLetters(lastName)) {
            return Error.BadRequest(ErrorMessage.FirstNameCanOnlyContainsLetters);
        }

        return new GuestLastName(lastName);
    }

    private static bool FirstNameBetween2And25Characters(string name) {
        name = name.Trim();
        return !string.IsNullOrWhiteSpace(name) && name.Length is >= 2 and <= 25;            
    }

    private static bool FirstNameIsOnlyLetters(string name) {
        return !string.IsNullOrWhiteSpace(name) && name.All(char.IsLetter);
    }



    protected override IEnumerable<object> GetEqualityComponents() {
        yield return Value;
    } 
}