namespace QuokkaServiceRegistry.Models;

public class CatalogService
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsPropriety { get; set; }
    public bool IsPublic { get; set; } = false;
    public string? Description { get; set; }
    
    // Hosting properties (moved from HostingInformation)
    public CatalogHostingType HostingType { get; set; }
    public string HostingCountry { get; set; } = string.Empty;
    public string? SaasRegionReference { get; set; }
    
    // Vendor Information (integrated from CatalogVendor)
    public string VendorName { get; set; } = string.Empty;
    public string? VendorWebsiteUrl { get; set; }
    public string? VendorCountry { get; set; }
    public string? VendorCity { get; set; }
    
    // Foreign key properties
    public int LicenseId { get; set; }
    public int? SubscriptionId { get; set; }
    public int? GdprRegisterId { get; set; }
    public int LifecycleId { get; set; }
    public int? BillingInformationId { get; set; }
    public int? PaymentMethodId { get; set; }
    
    // Navigation properties
    public SoftwareLicense License { get; set; } = null!;
    public ServiceSubscription? Subscription { get; set; }
    public GdprDataRegister? GdprRegister { get; set; }
    public ServiceLifecycleInfo Lifecycle { get; set; } = null!;
    public BillingInformation? BillingInformation { get; set; }
    public PaymentMethod? PaymentMethod { get; set; }
    public List<OnpremiseHost>? OnpremiseHosts { get; set; }
}