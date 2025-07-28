namespace QuokkaServiceRegistry.Models;

public class VendorBillingViewModel
{
    // Vendor properties
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string WebsiteUrl { get; set; } = string.Empty;
    public string? Country { get; set; }
    public string? City { get; set; }
    public bool GdprProcessingAgreementExists { get; set; }
    public string? ProcessingAgreementLink { get; set; }

    // Payment Information
    public int? PaymentMethodId { get; set; }
    public bool IsActive { get; set; } = true;

    // Helper method to convert from CatalogVendor
    public static VendorBillingViewModel FromVendor(CatalogVendor vendor)
    {
        var viewModel = new VendorBillingViewModel
        {
            Id = vendor.Id,
            Name = vendor.Name,
            WebsiteUrl = vendor.WebsiteUrl,
            Country = vendor.Country,
            City = vendor.City,
            PaymentMethodId = vendor.PaymentMethodId,
            IsActive = vendor.IsActive
        };

        return viewModel;
    }

    // Helper method to update vendor
    public void UpdateVendor(CatalogVendor vendor)
    {
        vendor.Name = Name;
        vendor.WebsiteUrl = WebsiteUrl;
        vendor.Country = Country;
        vendor.City = City;
        vendor.PaymentMethodId = PaymentMethodId;
        vendor.IsActive = IsActive;
    }
}