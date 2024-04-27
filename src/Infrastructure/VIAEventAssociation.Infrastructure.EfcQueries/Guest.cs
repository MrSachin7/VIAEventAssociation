

namespace VIAEventAssociation.Infrastructure.EfcQueries;

public partial class Guest
{
    public string Id { get; set; } = null!;

    public string? ProfilePictureUrl { get; set; }

    public string Email { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public virtual ICollection<EventInvitation> EventInvitations { get; set; } = new List<EventInvitation>();

    public virtual ICollection<VeaEvent> Events { get; set; } = new List<VeaEvent>();
}
