using System;
using System.Collections.Generic;

namespace OrganizationWaterSupplySol;

public partial class ViewOrganizationsCounter
{
    public int OrganizationId { get; set; }

    public string? OrgName { get; set; }

    public string? OwnershipType { get; set; }

    public string? Adress { get; set; }

    public string? DirectorFullname { get; set; }

    public string? DirectorPhone { get; set; }

    public string? ResponsibleFullname { get; set; }

    public string? ResponsiblePhone { get; set; }

    public int ModelId { get; set; }

    public string? ModelName { get; set; }

    public string? Manufacturer { get; set; }

    public int? ServiceTime { get; set; }

    public int RegistrationNumber { get; set; }

    public DateTime? TimeOfInstallation { get; set; }
}
