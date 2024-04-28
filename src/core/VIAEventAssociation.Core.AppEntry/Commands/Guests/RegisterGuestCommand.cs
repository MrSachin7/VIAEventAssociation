using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using VIAEventAssociation.Core.Domain.Contracts;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.AppEntry.Commands.Guests;

public class RegisterGuestCommand  : ICommand{
    public Guest Guest { get; init; }

    private RegisterGuestCommand(Guest guest) {
        Guest = guest;
    }

    // Now all of a sudden this needs to be async , how to fix it ??
    public static async Task<Result<RegisterGuestCommand>> Create(string firstname, string lastname,
        string viaEmail, string? profilePictureUrl, IUniqueEmailChecker someEmailUniqueCheckService){

        Result<GuestFirstName> firstnameResult = GuestFirstName.Create(firstname);
        Result<GuestLastName> lastnameResult = GuestLastName.Create(lastname);
        Result<ViaEmail> viaEmailResult = await ViaEmail.Create(viaEmail, someEmailUniqueCheckService);

        Result combinedResult = firstnameResult
            .Combine(lastnameResult)
            .Combine(viaEmailResult);

        ProfilePictureUrl? guestProfilePictureUrl = null;

        if (!string.IsNullOrEmpty(profilePictureUrl)) {
            Result<ProfilePictureUrl> profilePictureUrlResult = ProfilePictureUrl.Create(profilePictureUrl);
             combinedResult = combinedResult
                .Combine(profilePictureUrlResult);
             if (profilePictureUrlResult.IsSuccess) {
                 guestProfilePictureUrl = profilePictureUrlResult.Payload!;
             }
        }

        if (combinedResult.IsFailure) {                                                   
            return combinedResult.Error!;
        }

        Result<Guest> guestResult  = Guest.Create(firstnameResult.Payload!, lastnameResult.Payload!, viaEmailResult.Payload!, guestProfilePictureUrl);
        if (guestResult.IsFailure) {
            return guestResult.Error!;
        }

        return new RegisterGuestCommand(guestResult.Payload!);




    }
}