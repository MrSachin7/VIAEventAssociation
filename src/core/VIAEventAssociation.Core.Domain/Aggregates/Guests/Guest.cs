using VIAEventAssociation.Core.Domain.Common.Bases;

namespace VIAEventAssociation.Core.Domain.Aggregates.Guests;

public class Guest : Aggregate<GuestId> {

    internal ProfilePictureUrl? ProfilePictureUrl { get; private set; }

    internal GuestFirstName FirstName { get; private set; }
    internal GuestLastName LastName { get; private set; }

    internal ViaEmail Email { get; private set; }

    private Guest(GuestFirstName firstName,GuestLastName lastName, ViaEmail email, ProfilePictureUrl? profilePictureUrl) {
        Id = GuestId.New();
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        ProfilePictureUrl = profilePictureUrl;
    }

    private Guest(GuestFirstName firstName,GuestLastName lastName, ViaEmail email) {
        Id = GuestId.New();
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    public static Guest Create(GuestFirstName firstName, GuestLastName lastName, ViaEmail email) {
        return new Guest(firstName, lastName, email);
    }
}