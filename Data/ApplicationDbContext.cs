using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuokkaServiceRegistry.Models;

namespace QuokkaServiceRegistry.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<CatalogService> Services { get; set; }
    public DbSet<ServiceSubscription> Subscriptions { get; set; }
    public DbSet<SoftwareLicense> Licenses { get; set; }
    public DbSet<OnpremiseHost> OnpremiseHosts { get; set; }
    public DbSet<BillingInformation> BillingInfo { get; set; }
    public DbSet<CostCenter> CostCenters { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<GdprDataRegister> GdprRegisters { get; set; }
    public DbSet<GdprController> GdprControllers { get; set; }
    public DbSet<GdprDpoOrganisation> GdprDpoOrganisations { get; set; }
    public DbSet<PaymentMethod> PaymentMethods { get; set; }
    public DbSet<ServiceLifecycleInfo> ServiceLifecycles { get; set; }
    public DbSet<ServiceLifecycleStage> LifecycleStages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure CatalogService
        modelBuilder.Entity<CatalogService>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            
            // Configure hosting properties
            entity.Property(e => e.HostingType).IsRequired();
            entity.Property(e => e.HostingCountry).IsRequired().HasMaxLength(100);
            entity.Property(e => e.SaasRegionReference).HasMaxLength(200);
            
            // Configure vendor properties (integrated from CatalogVendor)
            entity.Property(e => e.VendorName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.VendorWebsiteUrl).HasMaxLength(500);
            entity.Property(e => e.VendorCountry).HasMaxLength(100);
            entity.Property(e => e.VendorCity).HasMaxLength(100);
            
            // Configure 1-to-many relationship with OnpremiseHosts
            entity.HasMany(e => e.OnpremiseHosts).WithOne(h => h.CatalogService).HasForeignKey(h => h.CatalogServiceId);
            
            // Configure 1-to-1 relationships (each service has its own instances)
            entity.HasOne(e => e.License).WithOne().HasForeignKey<CatalogService>(e => e.LicenseId);
            entity.HasOne(e => e.Lifecycle).WithOne().HasForeignKey<CatalogService>(e => e.LifecycleId);
            entity.HasOne(e => e.GdprRegister).WithOne().HasForeignKey<CatalogService>(e => e.GdprRegisterId).IsRequired(false);
            entity.HasOne(e => e.Subscription).WithOne().HasForeignKey<CatalogService>(e => e.SubscriptionId).IsRequired(false);
            
            // Configure billing and payment relationships
            entity.HasOne(e => e.BillingInformation).WithMany().HasForeignKey(e => e.BillingInformationId).IsRequired(false);
            entity.HasOne(e => e.PaymentMethod).WithMany().HasForeignKey(e => e.PaymentMethodId).IsRequired(false);
        });


        // Configure ServiceSubscription
        modelBuilder.Entity<ServiceSubscription>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.CostPerSeatUsd).HasColumnType("decimal(18,2)");
        });

        // Configure SoftwareLicense
        modelBuilder.Entity<SoftwareLicense>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Abbreviation).IsRequired().HasMaxLength(50);
            entity.Property(e => e.LicenseUrl).HasMaxLength(500);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Type).IsRequired();
            entity.Property(e => e.IsCopyleft).IsRequired();
            entity.Property(e => e.IsOsiApproved).IsRequired();
            entity.Property(e => e.RequiresAttribution).IsRequired();
            entity.Property(e => e.RequiresSourceDisclosure).IsRequired();
            entity.Property(e => e.CompatibilityLevel).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
        });

        // Configure OnpremiseHost
        modelBuilder.Entity<OnpremiseHost>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Region).IsRequired().HasMaxLength(100);
            entity.Property(e => e.NetboxReferenceUrl).IsRequired().HasMaxLength(500);
            entity.Property(e => e.CloudProvider).IsRequired().HasMaxLength(200);
            entity.HasOne(e => e.CatalogService).WithMany(s => s.OnpremiseHosts).HasForeignKey(e => e.CatalogServiceId);
        });

        // Configure BillingInformation
        modelBuilder.Entity<BillingInformation>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.CostCenter).WithMany().HasForeignKey("CostCenterId");
            entity.Property(e => e.CardNumber).HasMaxLength(20);
            entity.Property(e => e.CardHolderName).HasMaxLength(200);
            entity.Property(e => e.ExpiryDate).HasMaxLength(7);
            entity.Property(e => e.BankName).HasMaxLength(200);
            entity.Property(e => e.AccountNumber).HasMaxLength(50);
            entity.Property(e => e.RoutingNumber).HasMaxLength(20);
            entity.Property(e => e.Iban).HasMaxLength(34);
            entity.Property(e => e.Swift).HasMaxLength(11);
            entity.Property(e => e.SepaMandateId).HasMaxLength(35);
            entity.Property(e => e.SepaCreditorId).HasMaxLength(35);
            entity.Property(e => e.PrepaidVoucherCode).HasMaxLength(100);
            entity.Property(e => e.PaymentMethodId).HasMaxLength(100);
            entity.HasOne(e => e.BillingAddress).WithMany().HasForeignKey("BillingAddressId").IsRequired(false);
        });

        // Configure Address
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Country).IsRequired().HasMaxLength(100);
            entity.Property(e => e.City).IsRequired().HasMaxLength(100);
        });

        // Configure GdprDataRegister
        modelBuilder.Entity<GdprDataRegister>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.DataCategories).HasConversion(
                v => string.Join(',', v.Select(e => e.ToString())),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(e => Enum.Parse<DataCategory>(e)).ToList()
            );
            entity.Property(e => e.ProcessingPurposes).HasConversion(
                v => string.Join(',', v.Select(e => e.ToString())),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(e => Enum.Parse<ProcessingPurpose>(e)).ToList()
            );
            entity.Property(e => e.SecurityMeasures).HasConversion(
                v => string.Join(',', v.Select(e => e.ToString())),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(e => Enum.Parse<SecurityMeasure>(e)).ToList()
            );
            entity.Property(e => e.DataTransfers).HasConversion(
                v => string.Join('|', v),
                v => v.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList()
            );
            entity.Property(e => e.PurposesOfUse).HasConversion(
                v => string.Join('|', v),
                v => v.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList()
            );
            entity.Property(e => e.SensitiveDataTypes).HasConversion(
                v => string.Join('|', v),
                v => v.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList()
            );
            entity.Property(e => e.DataDeletionProcess).HasMaxLength(1000);
            entity.Property(e => e.DataProtectionOfficerContact).HasMaxLength(200);
            entity.Property(e => e.ComplianceNotes).HasMaxLength(1000);
        });

        // Configure ServiceLifecycleInfo
        modelBuilder.Entity<ServiceLifecycleInfo>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasMany(e => e.Stages).WithOne().HasForeignKey("ServiceLifecycleInfoId");
            entity.Property(e => e.MigrationPath).HasMaxLength(1000);
            entity.Property(e => e.GeneralNotes).HasConversion(
                v => string.Join('|', v),
                v => v.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList()
            );
        });

        // Configure ServiceLifecycleStage
        modelBuilder.Entity<ServiceLifecycleStage>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Sponsor).HasMaxLength(200);
            entity.Property(e => e.ApprovedBy).HasMaxLength(200);
            entity.Property(e => e.Outcome).HasMaxLength(1000);
            entity.Property(e => e.Feedback).HasMaxLength(1000);
            entity.Property(e => e.Participants).HasConversion(
                v => string.Join('|', v),
                v => v.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList()
            );
            entity.Property(e => e.Departments).HasConversion(
                v => string.Join('|', v),
                v => v.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList()
            );
            entity.Property(e => e.Criteria).HasConversion(
                v => string.Join('|', v),
                v => v.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList()
            );
            entity.Property(e => e.Notes).HasConversion(
                v => string.Join('|', v),
                v => v.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList()
            );
            entity.Property(e => e.CustomProperties).HasConversion(
                v => string.Join('|', v.Select(kvp => $"{kvp.Key}:{kvp.Value}")),
                v => v.Split(new char[] { '|' }).Where(x => x.Contains(':')).ToDictionary(x => x.Split(new char[] { ':' })[0], x => x.Split(new char[] { ':' })[1])
            );
        });

        // Configure CostCenter
        modelBuilder.Entity<CostCenter>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(500);
        });

        // Configure PaymentMethod
        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Type).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.CardNumber).HasMaxLength(20);
            entity.Property(e => e.CardHolderName).HasMaxLength(200);
            entity.Property(e => e.ExpiryDate).HasMaxLength(7);
            entity.Property(e => e.BankName).HasMaxLength(200);
            entity.Property(e => e.AccountNumber).HasMaxLength(50);
            entity.Property(e => e.RoutingNumber).HasMaxLength(20);
            entity.Property(e => e.Iban).HasMaxLength(34);
            entity.Property(e => e.Swift).HasMaxLength(11);
            entity.Property(e => e.SepaMandateId).HasMaxLength(35);
            entity.Property(e => e.SepaCreditorId).HasMaxLength(35);
            entity.Property(e => e.PrepaidVoucherCode).HasMaxLength(100);
            entity.Property(e => e.PaymentMethodId).HasMaxLength(100);
            entity.HasOne(e => e.CostCenter).WithMany().HasForeignKey(e => e.CostCenterId);
            entity.HasMany(e => e.Services).WithOne(s => s.PaymentMethod).HasForeignKey(s => s.PaymentMethodId);
        });

        // Configure GdprController
        modelBuilder.Entity<GdprController>(entity =>
        {
            entity.Property(e => e.DataOwner).HasMaxLength(200);
            entity.HasOne(e => e.CostCenter).WithMany().HasForeignKey(e => e.CostCenterId);
        });
    }
}