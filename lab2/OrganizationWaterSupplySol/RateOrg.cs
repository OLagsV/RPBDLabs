using System;
using System.Collections.Generic;

namespace OrganizationWaterSupplySol;

public partial class RateOrg
{
    public int RateId { get; set; }

    public int? OrganizationId { get; set; }

    public virtual ICollection<CountersDatum> CountersData { get; set; } = new List<CountersDatum>();

    public virtual Organization? Organization { get; set; }

    public virtual Rate Rate { get; set; } = null!;
}
