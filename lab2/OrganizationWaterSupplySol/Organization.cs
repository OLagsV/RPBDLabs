using System;
using System.Collections.Generic;

namespace OrganizationWaterSupplySol;

public partial class Organization
{
    public int OrganizationId { get; set; }

    public string? OrgName { get; set; }

    public string? OwnershipType { get; set; }

    public string? Adress { get; set; }

    public string? DirectorFullname { get; set; }

    public string? DirectorPhone { get; set; }

    public string? ResponsibleFullname { get; set; }

    public string? ResponsiblePhone { get; set; }

    public virtual ICollection<Counter> Counters { get; set; } = new List<Counter>();

    public virtual ICollection<RateOrg> RateOrgs { get; set; } = new List<RateOrg>();
}
