using System.Text.RegularExpressions;
using VIAEventAssociation.Core.Domain.Common.Bases;
using VIAEventAssociation.Core.Domain.Contracts;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Guests;

public class ViaEmail : ValueObject {
    internal string Value { get; }

    private ViaEmail(string email) {
        Value = email.ToLower();
    }

    public static Result<ViaEmail> Create(string email) {
        Result<ViaEmail> validEmailResult = Result.ToBuilder(ErrorCode.BadRequest)
            .AssertWithError(() => EmailEndsWithViaDk(email), ErrorMessage.EmailMustEndWithViaDk)
            .AssertWithError(() => EmailIsInCorrectFormat(email), ErrorMessage.EmailNotInCorrectFormat)
            .WithPayload(new ViaEmail(email))
            .Build();

        if (validEmailResult.IsFailure) {
            return validEmailResult;
        }

        ViaEmail viaEmail = validEmailResult.Payload!;
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


    protected override IEnumerable<object> GetEqualityComponents() {
        yield return Value;
    }
}