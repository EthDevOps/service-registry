namespace QuokkaServiceRegistry.Models;

public class CatalogService
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsPropriety { get; set; }
    
    // Hosting properties (moved from HostingInformation)
    public CatalogHostingType HostingType { get; set; }
    public string HostingCountry { get; set; } = string.Empty;
    public string? SaasRegionReference { get; set; }
    
    // Foreign key properties
    public int VendorId { get; set; }
    public int LicenseId { get; set; }
    public int? SubscriptionId { get; set; }
    public int? GdprRegisterId { get; set; }
    public int LifecycleId { get; set; }
    
    // Navigation properties
    public CatalogVendor Vendor { get; set; } = null!;
    public SoftwareLicense License { get; set; } = null!;
    public ServiceSubscription? Subscription { get; set; }
    public GdprDataRegister? GdprRegister { get; set; }
    public ServiceLifecycleInfo Lifecycle { get; set; } = null!;
    public List<OnpremiseHost>? OnpremiseHosts { get; set; }
}