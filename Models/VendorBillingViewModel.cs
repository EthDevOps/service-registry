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

    // Billing Information properties
    public int? BillingInformationId { get; set; }
    public BillingType BillingType { get; set; } = BillingType.Free;
    public int? CostCenterId { get; set; }
    public bool IsActive { get; set; } = true;

    // Credit Card Information
    public string? CardHolderName { get; set; }
    public string? CardNumber { get; set; }
    public string? ExpiryDate { get; set; }
    public string? Cvv { get; set; }

    // Bank Transfer Information
    public string? BankName { get; set; }
    public string? AccountNumber { get; set; }
    public string? RoutingNumber { get; set; }
    public string? Iban { get; set; }
    public string? Swift { get; set; }

    // SEPA Information
    public string? SepaMandateId { get; set; }
    public DateTime? SepaMandateDate { get; set; }
    public string? SepaCreditorId { get; set; }

    // PrePaid Information
    public decimal? PrepaidBalance { get; set; }
    public DateTime? PrepaidExpiryDate { get; set; }
    public string? PrepaidVoucherCode { get; set; }

    // General Payment Information
    public string? PaymentMethodId { get; set; }
    public DateTime? LastPaymentDate { get; set; }
    public DateTime? NextBillingDate { get; set; }
    public decimal? OutstandingBalance { get; set; }

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
            GdprProcessingAgreementExists = vendor.GdprProcessingAgreementExists,
            ProcessingAgreementLink = vendor.ProcessingAgreementLink,
            BillingInformationId = vendor.BillingInformationId
        };

        if (vendor.BillingInformation != null)
        {
            var billing = vendor.BillingInformation;
            viewModel.BillingType = billing.Type;
            viewModel.CostCenterId = billing.CostCenter?.Id;
            viewModel.IsActive = billing.IsActive;
            viewModel.CardHolderName = billing.CardHolderName;
            viewModel.CardNumber = billing.CardNumber;
            viewModel.ExpiryDate = billing.ExpiryDate;
            viewModel.BankName = billing.BankName;
            viewModel.AccountNumber = billing.AccountNumber;
            viewModel.RoutingNumber = billing.RoutingNumber;
            viewModel.Iban = billing.Iban;
            viewModel.Swift = billing.Swift;
            viewModel.SepaMandateId = billing.SepaMandateId;
            viewModel.SepaMandateDate = billing.SepaMandateDate;
            viewModel.SepaCreditorId = billing.SepaCreditorId;
            viewModel.PrepaidBalance = billing.PrepaidBalance;
            viewModel.PrepaidExpiryDate = billing.PrepaidExpiryDate;
            viewModel.PrepaidVoucherCode = billing.PrepaidVoucherCode;
            viewModel.PaymentMethodId = billing.PaymentMethodId;
            viewModel.LastPaymentDate = billing.LastPaymentDate;
            viewModel.NextBillingDate = billing.NextBillingDate;
            viewModel.OutstandingBalance = billing.OutstandingBalance;
        }

        return viewModel;
    }

    // Helper method to update vendor and billing information
    public void UpdateVendor(CatalogVendor vendor)
    {
        vendor.Name = Name;
        vendor.WebsiteUrl = WebsiteUrl;
        vendor.Country = Country;
        vendor.City = City;
        vendor.GdprProcessingAgreementExists = GdprProcessingAgreementExists;
        vendor.ProcessingAgreementLink = ProcessingAgreementLink;
    }

    public void UpdateBillingInformation(BillingInformation billing, CostCenter costCenter)
    {
        billing.Type = BillingType;
        billing.CostCenter = costCenter;
        billing.IsActive = IsActive;
        billing.CardHolderName = CardHolderName;
        billing.CardNumber = CardNumber;
        billing.ExpiryDate = ExpiryDate;
        billing.BankName = BankName;
        billing.AccountNumber = AccountNumber;
        billing.RoutingNumber = RoutingNumber;
        billing.Iban = Iban;
        billing.Swift = Swift;
        billing.SepaMandateId = SepaMandateId;
        billing.SepaMandateDate = SepaMandateDate;
        billing.SepaCreditorId = SepaCreditorId;
        billing.PrepaidBalance = PrepaidBalance;
        billing.PrepaidExpiryDate = PrepaidExpiryDate;
        billing.PrepaidVoucherCode = PrepaidVoucherCode;
        billing.PaymentMethodId = PaymentMethodId;
        billing.LastPaymentDate = LastPaymentDate;
        billing.NextBillingDate = NextBillingDate;
        billing.OutstandingBalance = OutstandingBalance;
    }
}