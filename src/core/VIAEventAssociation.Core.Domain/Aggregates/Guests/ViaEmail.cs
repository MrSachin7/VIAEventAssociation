using System.Text.RegularExpressions;
using VIAEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Guests;

public class ViaEmail : ValueObject {
    internal string Value { get; }

    private ViaEmail(string email) {
        Value = email.ToLower();
    }

    internal static Result<ViaEmail> From(string email) {
        Result<ViaEmail> validEmailResult = Result<ViaEmail>.AsBuilder(ErrorCode.BadRequest, new ViaEmail(email))
            .AssertWithError(() => EmailEndsWithViaDk(email), ErrorMessage.EmailMustEndWithViaDk)
            .AssertWithError(() => EmailIsInCorrectFormat(email), ErrorMessage.EmailNotInCorrectFormat)
            .AssertWithError((() => EmailIsUnique(email)), ErrorMessage.EmailAlreadyAssociatedWithAnotherGuest)
            .Build();

        if (validEmailResult.IsFailure) {
            return validEmailResult;
        }

        ViaEmail viaEmail = validEmailResult.Payload!;

        // This is not concatenated with the above result because they have different error codes and can never coexist together
        if (!EmailIsUnique(viaEmail.Value)) {
            return Error.Conflict(ErrorMessage.EmailAlreadyAssociatedWithAnotherGuest);
        }

        return viaEmail;
    }

    private static bool EmailEndsWithViaDk(string email) {
        return email.EndsWith("@via.dk");
    }

    private static bool EmailIsInCorrectFormat(string email) {
        // This regex makes sure that it can either be a 6 digit number or a 3-4 letter word followed by @via.dk
        const string emailPattern = @"^((\d{6})|([a-zA-Z]{3,4}))@via\.dk$";
        return Regex.IsMatch(email, emailPattern);
    }

    // Todo : Ask troels if this is belongs here ??
    private static bool EmailIsUnique(string email) {
        // Not implemented for now
        return true;
    }

    protected override IEnumerable<object> GetEqualityComponents() {
        yield return Value;
    }
}