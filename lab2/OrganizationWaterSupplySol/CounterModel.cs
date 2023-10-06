using System;
using System.Collections.Generic;

namespace OrganizationWaterSupplySol;

public partial class CounterModel
{
    public int ModelId { get; set; }

    public string? ModelName { get; set; }

    public string? Manufacturer { get; set; }

    public int? ServiceTime { get; set; }

    public virtual ICollection<Counter> Counters { get; set; } = new List<Counter>();
}
