using System;
using System.Collections.Generic;

namespace VIAEventAssociation.Infrastructure.EfcQueries;

public partial class Location
{
    public string Id { get; set; } = null!;

    public int LocationMaxGuests { get; set; }

    public string LocationName { get; set; } = null!;

    public virtual ICollection<VeaEvent> VeaEvents { get; set; } = new List<VeaEvent>();
}
