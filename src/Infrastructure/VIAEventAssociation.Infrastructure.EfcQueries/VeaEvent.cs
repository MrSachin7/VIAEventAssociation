
using System.Text.Json.Serialization;

namespace VIAEventAssociation.Infrastructure.EfcQueries;

public partial class VeaEvent
{
    public string Id { get; set; } = null!;

    public DateTime? StartDateTime { get; set; }

    public DateTime? EndDateTime { get; set; }

    public string? LocationId { get; set; }

    public string Status { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int MaxGuests { get; set; }

    public string Title { get; set; } = null!;

    public string Visibility { get; set; } = null!;

    public virtual ICollection<EventInvitation> EventInvitations { get; set; } = new List<EventInvitation>();

    [JsonIgnore]
    public virtual Location? Location { get; set; }

    public virtual ICollection<Guest> Guests { get; set; } = new List<Guest>();
}
