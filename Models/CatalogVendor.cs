namespace QuokkaServiceRegistry.Models;

public class CatalogVendor
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string WebsiteUrl { get; set; }
    
    // Integrated address properties
    public string? Country { get; set; }
    public string? City { get; set; }
    
    // Foreign key properties
    public int? BillingInformationId { get; set; }
    
    // Navigation properties
    public BillingInformation? BillingInformation { get; set; }
    
    public bool GdprProcessingAgreementExists { get; set; }
    public string? ProcessingAgreementLink { get; set; }
}