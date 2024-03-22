using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using VIAEventAssociation.Core.Domain.Contracts;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.AppEntry.Commands.Guests;

public class RegisterGuestCommand {
    public Guest Guest { get; init; }

    private RegisterGuestCommand(Guest guest) {
        Guest = guest;
    }

    // Now all of a sudden this needs to be async , how to fix it ??
    public static async Task<Result<RegisterGuestCommand>> Create(string firstname, string lastname,
        string viaEmail, string profilePictureUrl, IUniqueEmailChecker someEmailUniqueCheckService){


        Result<GuestFirstName> firstnameResult = GuestFirstName.Create(firstname);
        Result<GuestLastName> lastnameResult = GuestLastName.Create(lastname);
        Result<ViaEmail> viaEmailResult = await ViaEmail.Create(viaEmail, someEmailUniqueCheckService);
        Result<ProfilePictureUrl> profilePictureUrlResult = ProfilePictureUrl.Create(profilePictureUrl);

        Result combinedResult = firstnameResult
            .Combine(lastnameResult)
            // .Combine(viaEmailResult)
            .Combine(profilePictureUrlResult);
        if (combinedResult.IsFailure) {                                                   
            return combinedResult.Error!;
        }

        Result<Guest> guestResult  = Guest.Create(firstnameResult.Payload!, lastnameResult.Payload!, viaEmailResult.Payload!, profilePictureUrlResult.Payload!);
        if (guestResult.IsFailure) {
            return guestResult.Error!;
        }

        return new RegisterGuestCommand(guestResult.Payload!);
    }
}