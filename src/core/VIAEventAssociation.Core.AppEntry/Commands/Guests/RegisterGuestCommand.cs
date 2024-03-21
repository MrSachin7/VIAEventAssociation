// using VIAEventAssociation.Core.Domain.Aggregates.Guests;
// using ViaEventAssociation.Core.Tools.OperationResult;
//
// namespace VIAEventAssociation.Core.AppEntry.Commands.Guests;
//
// public class RegisterGuestCommand {
//     public Guest Guest { get; init; }
//
//     private RegisterGuestCommand(Guest guest) {
//         Guest = guest;
//     }
//
//     // Now all of a sudden this needs to be async , how to fix it ??
//     public static async Result<RegisterGuestCommand> Create(string firstname, string lastname,
//         string viaEmail, string profilePictureUrl) {
//         Result<GuestFirstName> firstnameResult = GuestFirstName.Create(firstname);
//         Result<GuestLastName> lastnameResult = GuestLastName.Create(lastname);
//         Result<ViaEmail> viaEmailResult = await ViaEmail.Create(viaEmail, someEmailUniqueCheckService);
//         Result<ProfilePictureUrl> profilePictureUrlResult = ProfilePictureUrl.Create(profilePictureUrl);
//
//         Result combinedResult = firstnameResult
//             .Combine(lastnameResult)
//             // .Combine(viaEmailResult)
//             .Combine(profilePictureUrlResult);
//         if (combinedResult.IsFailure) {                                                   
//             return combinedResult.Error!;
//         }
//
//         Guest guest  = Guest.Create(firstnameResult.Payload!, lastnameResult.Payload!, viaEmailResult.Payload!, profilePictureUrlResult.Payload!);
//         return new RegisterGuestCommand(guest);
//     }
// }