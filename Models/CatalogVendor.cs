namespace QuokkaServiceRegistry.Models;

public class CatalogVendor
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string WebsiteUrl { get; set; }
    
    // Integrated address properties
    public string? Country { get; set; }
    public string? City { get; set; }
    
    // Status
    public bool IsActive { get; set; } = true;
    
    // Foreign key properties
    public int? BillingInformationId { get; set; }
    public int? PaymentMethodId { get; set; }
    
    // Navigation properties
    public BillingInformation? BillingInformation { get; set; }
    public PaymentMethod? PaymentMethod { get; set; }
}