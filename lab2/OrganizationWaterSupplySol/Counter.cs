using System;
using System.Collections.Generic;

namespace OrganizationWaterSupplySol;

public partial class Counter
{
    public int RegistrationNumber { get; set; }

    public int? ModelId { get; set; }

    public DateTime? TimeOfInstallation { get; set; }

    public int? OrganizationId { get; set; }

    public virtual ICollection<CountersCheck> CountersChecks { get; set; } = new List<CountersCheck>();

    public virtual ICollection<CountersDatum> CountersData { get; set; } = new List<CountersDatum>();

    public virtual CounterModel? Model { get; set; }

    public virtual Organization? Organization { get; set; }
}
