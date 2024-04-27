
using System.Text.Json.Serialization;

namespace VIAEventAssociation.Infrastructure.EfcQueries;

public partial class EventInvitation
{
    public string Id { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string GuestId { get; set; } = null!;

    public string VeaEventId { get; set; } = null!;

    [JsonIgnore]
    public virtual Guest Guest { get; set; } = null!;

    [JsonIgnore]
    public virtual VeaEvent VeaEvent { get; set; } = null!;
}
