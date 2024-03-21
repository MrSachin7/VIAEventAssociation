using VIAEventAssociation.Core.Domain.Common.Bases;
using VIAEventAssociation.Core.Domain.Contracts;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Guests;

public class Guest : Aggregate<GuestId> {
    internal ProfilePictureUrl? ProfilePictureUrl { get; private set; }

    internal GuestFirstName FirstName { get; private set; }
    internal GuestLastName LastName { get; private set; }

    internal ViaEmail Email { get; private set; }

    private Guest(GuestFirstName firstName, GuestLastName lastName, ViaEmail email,
        ProfilePictureUrl? profilePictureUrl) {
        Id = GuestId.New();
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        ProfilePictureUrl = profilePictureUrl;
    }

    private Guest(GuestFirstName firstName, GuestLastName lastName, ViaEmail email) {
        Id = GuestId.New();
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    public static Result<Guest> Create(GuestFirstName firstName, GuestLastName lastName, ViaEmail email,
        IUniqueEmailChecker emailChecker) {
        bool isUnique = emailChecker.IsUnique(email.Value).Result;
        if (!isUnique) {
            return Error.Conflict(ErrorMessage.EmailAlreadyAssociatedWithAnotherGuest);
        }

        return new Guest(firstName, lastName, email);
    }

    public static Result<Guest> Create(GuestFirstName firstName, GuestLastName lastName, ViaEmail email,
        ProfilePictureUrl profilePictureUrl, IUniqueEmailChecker uniqueEmailChecker) {
        Result<Guest> guestResult = Create(firstName, lastName, email, uniqueEmailChecker);
        if (guestResult.IsFailure) {
            return guestResult;
        }

        guestResult.Payload!.ProfilePictureUrl = profilePictureUrl;
        return guestResult.Payload!;
    }
}