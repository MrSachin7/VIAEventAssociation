using VIAEventAssociation.Core.Domain.Common.Bases;

namespace VIAEventAssociation.Core.Domain.Aggregates.Guests;

public class Guest : Aggregate<GuestId> {

    // Todo: Id -10. Is profile picture mandatory?
    internal ProfilePictureUrl? ProfilePictureUrl { get; private set; }

    internal GuestFirstName FirstName { get; private set; }
    internal GuestLastName LastName { get; private set; }

    internal string FullName => $"{FirstName.Value} {LastName.Value}";

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

    public static Guest From(GuestFirstName firstName, GuestLastName lastName, ViaEmail email, ProfilePictureUrl? profilePictureUrl) {
        return new Guest(firstName, lastName, email, profilePictureUrl);
    }

    public static Guest From(GuestFirstName firstName, GuestLastName lastName, ViaEmail email) {
        return new Guest(firstName, lastName, email);
    }
}