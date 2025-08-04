namespace QuokkaServiceRegistry.Models;

public class GdprStakeholder
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class GdprController : GdprStakeholder
{
    public int? CostCenterId { get; set; }
    public CostCenter? CostCenter { get; set; }
    public string? DataOwner { get; set; }
}

public class GdprDpoOrganisation : GdprStakeholder
{
}