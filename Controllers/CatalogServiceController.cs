using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuokkaServiceRegistry.Data;
using QuokkaServiceRegistry.Models;

namespace QuokkaServiceRegistry.Controllers;

[Authorize]
public class CatalogServiceController : Controller
{
    private readonly ApplicationDbContext _context;

    public CatalogServiceController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: CatalogService
    public async Task<IActionResult> Index(string? hostingType, string? hostingCountry, string? searchTerm)
    {
        var servicesQuery = _context.Services
            .Include(s => s.License)
            .Include(s => s.OnpremiseHosts)
            .Include(s => s.Lifecycle)
            .Include(s => s.Subscription)
            .Include(s => s.GdprRegister)
            .Include(s => s.BillingInformation)
            .Include(s => s.PaymentMethod)
            .AsQueryable();

        // Apply hosting type filter
        if (!string.IsNullOrEmpty(hostingType) && Enum.TryParse<CatalogHostingType>(hostingType, out var hostingTypeEnum))
        {
            servicesQuery = servicesQuery.Where(s => s.HostingType == hostingTypeEnum);
        }

        // Apply hosting country filter
        if (!string.IsNullOrEmpty(hostingCountry))
        {
            servicesQuery = servicesQuery.Where(s => s.HostingCountry.Contains(hostingCountry));
        }

        // Apply search term filter
        if (!string.IsNullOrEmpty(searchTerm))
        {
            servicesQuery = servicesQuery.Where(s => 
                s.Name.Contains(searchTerm) ||
                s.VendorName.Contains(searchTerm) ||
                s.License.Name.Contains(searchTerm) ||
                s.HostingCountry.Contains(searchTerm));
        }

        var services = await servicesQuery.ToListAsync();

        // Populate filter dropdowns
        ViewBag.HostingTypes = Enum.GetValues<CatalogHostingType>()
            .Select(ht => new SelectListItem 
            { 
                Value = ht.ToString(), 
                Text = ht.ToString(),
                Selected = ht.ToString() == hostingType 
            }).ToList();

        ViewBag.HostingCountries = await _context.Services
            .Select(s => s.HostingCountry)
            .Distinct()
            .OrderBy(c => c)
            .Select(c => new SelectListItem 
            { 
                Value = c, 
                Text = c,
                Selected = c == hostingCountry 
            }).ToListAsync();

        ViewBag.CurrentFilters = new
        {
            HostingType = hostingType,
            HostingCountry = hostingCountry,
            SearchTerm = searchTerm
        };

        return View(services);
    }

    // GET: CatalogService/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var catalogService = await _context.Services
            .Include(s => s.BillingInformation)
                .ThenInclude(b => b.CostCenter)
            .Include(s => s.License)
            .Include(s => s.OnpremiseHosts)
            .Include(s => s.Lifecycle)
                .ThenInclude(l => l.Stages)
            .Include(s => s.Subscription)
            .Include(s => s.GdprRegister)
                .ThenInclude(l => l.Controller)
            .Include(s => s.GdprRegister)
                .ThenInclude(l => l.DpoOrganisation)
                
            .FirstOrDefaultAsync(m => m.Id == id);

        if (catalogService == null)
        {
            return NotFound();
        }

        // Get data controllers for dropdown
        ViewBag.DataControllers = await _context.GdprControllers
            .Include(c => c.CostCenter)
            .OrderBy(c => c.Name)
            .ToListAsync();

        return View(catalogService);
    }

    // GET: CatalogService/Create
    public IActionResult Create()
    {
        ViewData["LicenseId"] = new SelectList(_context.Licenses, "Id", "Name");
        ViewData["SubscriptionId"] = new SelectList(_context.Subscriptions, "Id", "Name");
        ViewData["GdprRegisterId"] = new SelectList(_context.GdprRegisters, "Id", "Id");
        ViewData["BillingInformationId"] = new SelectList(_context.BillingInfo, "Id", "Id");
        ViewData["PaymentMethodId"] = new SelectList(_context.PaymentMethods, "Id", "Name");
        
        return View();
    }

    // POST: CatalogService/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,IsPropriety,VendorName,VendorWebsiteUrl,VendorCountry,VendorCity,LicenseId,HostingType,HostingCountry,SaasRegionReference,SubscriptionId,GdprRegisterId,BillingInformationId,PaymentMethodId")] CatalogService catalogService, ServiceLifecycleStageType initialStage = ServiceLifecycleStageType.ProofOfConcept)
    {
        ModelState.Remove("License");
        ModelState.Remove("Lifecycle");

        if (ModelState.IsValid)
        {
            // Create initial lifecycle info with the specified stage
            var lifecycleInfo = new ServiceLifecycleInfo
            {
                CurrentStage = initialStage,
                Stages = new List<ServiceLifecycleStage>
                {
                    new ServiceLifecycleStage
                    {
                        StageType = initialStage,
                        Status = ServiceLifecycleStageStatus.InProgress,
                        StartDate = DateTime.UtcNow
                    }
                }
            };

            _context.ServiceLifecycles.Add(lifecycleInfo);
            await _context.SaveChangesAsync();

            catalogService.LifecycleId = lifecycleInfo.Id;

            _context.Add(catalogService);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["LicenseId"] = new SelectList(_context.Licenses, "Id", "Name", catalogService.LicenseId);
        ViewData["SubscriptionId"] = new SelectList(_context.Subscriptions, "Id", "Name", catalogService.SubscriptionId);
        ViewData["GdprRegisterId"] = new SelectList(_context.GdprRegisters, "Id", "Id", catalogService.GdprRegisterId);
        ViewData["BillingInformationId"] = new SelectList(_context.BillingInfo, "Id", "Id", catalogService.BillingInformationId);
        ViewData["PaymentMethodId"] = new SelectList(_context.PaymentMethods, "Id", "Name", catalogService.PaymentMethodId);
        
        return View(catalogService);
    }

    // GET: CatalogService/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var catalogService = await _context.Services.FindAsync(id);
        if (catalogService == null)
        {
            return NotFound();
        }
        
        ViewData["LicenseId"] = new SelectList(_context.Licenses, "Id", "Name", catalogService.LicenseId);
        ViewData["LifecycleId"] = new SelectList(_context.ServiceLifecycles, "Id", "CurrentStage", catalogService.LifecycleId);
        ViewData["SubscriptionId"] = new SelectList(_context.Subscriptions, "Id", "Name", catalogService.SubscriptionId);
        ViewData["GdprRegisterId"] = new SelectList(_context.GdprRegisters, "Id", "Id", catalogService.GdprRegisterId);
        
        return View(catalogService);
    }

    // POST: CatalogService/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,IsPropriety,VendorName,VendorWebsiteUrl,VendorCountry,VendorCity,LicenseId,HostingType,HostingCountry,SaasRegionReference,LifecycleId,SubscriptionId,GdprRegisterId,BillingInformationId,PaymentMethodId")] CatalogService catalogService)
    {
        if (id != catalogService.Id)
        {
            return NotFound();
        }
        ModelState.Remove("License");
        ModelState.Remove("Lifecycle");

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(catalogService);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CatalogServiceExists(catalogService.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        
        ViewData["LicenseId"] = new SelectList(_context.Licenses, "Id", "Name", catalogService.LicenseId);
        ViewData["LifecycleId"] = new SelectList(_context.ServiceLifecycles, "Id", "CurrentStage", catalogService.LifecycleId);
        ViewData["SubscriptionId"] = new SelectList(_context.Subscriptions, "Id", "Name", catalogService.SubscriptionId);
        ViewData["GdprRegisterId"] = new SelectList(_context.GdprRegisters, "Id", "Id", catalogService.GdprRegisterId);
        
        return View(catalogService);
    }

    // GET: CatalogService/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var catalogService = await _context.Services
            .Include(s => s.License)
            .Include(s => s.OnpremiseHosts)
            .Include(s => s.Lifecycle)
            .Include(s => s.Subscription)
            .Include(s => s.GdprRegister)
            .FirstOrDefaultAsync(m => m.Id == id);
            
        if (catalogService == null)
        {
            return NotFound();
        }

        return View(catalogService);
    }

    // POST: CatalogService/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var catalogService = await _context.Services.FindAsync(id);
        if (catalogService != null)
        {
            _context.Services.Remove(catalogService);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // POST: CatalogService/AddLifecycleStage/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddLifecycleStage(int serviceId, ServiceLifecycleStageType stageType, string? sponsor = null, string? notes = null)
    {
        var catalogService = await _context.Services
            .Include(s => s.Lifecycle)
            .ThenInclude(l => l.Stages)
            .FirstOrDefaultAsync(s => s.Id == serviceId);
            
        if (catalogService == null)
        {
            return NotFound();
        }

        // Check if stage already exists
        if (catalogService.Lifecycle.Stages.Any(s => s.StageType == stageType))
        {
            TempData["Error"] = $"Stage '{stageType}' already exists for this service.";
            return RedirectToAction(nameof(Details), new { id = serviceId });
        }

        // Complete the current stage if it's in progress
        var currentStage = catalogService.Lifecycle.Stages
            .FirstOrDefault(s => s.Status == ServiceLifecycleStageStatus.InProgress);
        if (currentStage != null)
        {
            currentStage.Status = ServiceLifecycleStageStatus.Completed;
            currentStage.EndDate = DateTime.UtcNow;
        }

        // Add the new stage
        var newStage = new ServiceLifecycleStage
        {
            StageType = stageType,
            Status = ServiceLifecycleStageStatus.InProgress,
            StartDate = DateTime.UtcNow,
            Sponsor = sponsor,
            Notes = !string.IsNullOrEmpty(notes) ? new List<string> { notes } : new List<string>()
        };

        catalogService.Lifecycle.Stages.Add(newStage);
        catalogService.Lifecycle.CurrentStage = stageType;

        await _context.SaveChangesAsync();
        
        TempData["Success"] = $"Successfully added new lifecycle stage: {stageType}";
        return RedirectToAction(nameof(Details), new { id = serviceId });
    }

    // POST: CatalogService/SetGdprCompliance/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetGdprCompliance(int serviceId, 
        List<DataCategory> dataCategories,
        List<ProcessingPurpose> processingPurposes,
        ProcessingFrequency processingFrequency,
        List<SecurityMeasure> securityMeasures,
        bool processesEmployeePii,
        bool processesExternalUserPii,
        bool processesSensitiveData,
        string? sensitiveDataTypes = null,
        bool hasProcessingAgreement = false,
        string? processingAgreementReference = null,
        DateTime? processingAgreementDate = null,
        int? dataRetentionDays = null,
        string? dataDeletionProcess = null,
        bool supportsDataPortability = false,
        bool supportsDataDeletion = false,
        bool supportsDataCorrection = false,
        string? dataProtectionOfficerContact = null,
        DateTime? lastGdprAssessment = null,
        string? complianceNotes = null,
        string? dataTransfers = null,
        string? purposesOfUse = null,
        // Controller parameters
        int? controllerId = null,
        // DPO parameters
        bool hasExternalDpo = false,
        string? dpoOrganisationName = null,
        string? dpoOrganisationEmail = null,
        string? dpoOrganisationAddress = null,
        string? dpoOrganisationZipCode = null,
        string? dpoOrganisationCity = null,
        string? dpoOrganisationCountry = null,
        string? dpoOrganisationPhone = null)
    {
        var catalogService = await _context.Services
            .Include(s => s.GdprRegister)
                .ThenInclude(g => g.Controller)
            .Include(s => s.GdprRegister)
                .ThenInclude(g => g.DpoOrganisation)
            .FirstOrDefaultAsync(s => s.Id == serviceId);
            
        if (catalogService == null)
        {
            return NotFound();
        }

        // Handle Controller information - get from database
        GdprController? controller = null;
        if (controllerId.HasValue)
        {
            controller = await _context.GdprControllers
                .Include(c => c.CostCenter)
                .FirstOrDefaultAsync(c => c.Id == controllerId.Value);
        }

        // Handle DPO Organisation information - create but don't save yet
        GdprDpoOrganisation? dpoOrganisation = null;
        if (hasExternalDpo && !string.IsNullOrEmpty(dpoOrganisationName))
        {
            dpoOrganisation = new GdprDpoOrganisation
            {
                Name = dpoOrganisationName,
                Email = dpoOrganisationEmail ?? "",
                Address = dpoOrganisationAddress ?? "",
                ZipCode = dpoOrganisationZipCode ?? "",
                City = dpoOrganisationCity ?? "",
                Country = dpoOrganisationCountry ?? "",
                Phone = dpoOrganisationPhone ?? ""
            };
        }

        // Create or update GDPR register
        if (catalogService.GdprRegister == null)
        {
            var gdprRegister = new GdprDataRegister
            {
                DataCategories = dataCategories ?? new List<DataCategory>(),
                ProcessingPurposes = processingPurposes ?? new List<ProcessingPurpose>(),
                ProcessingFrequency = processingFrequency,
                SecurityMeasures = securityMeasures ?? new List<SecurityMeasure>(),
                ProcessesEmployeePii = processesEmployeePii,
                ProcessesExternalUserPii = processesExternalUserPii,
                ProcessesSensitiveData = processesSensitiveData,
                SensitiveDataTypes = !string.IsNullOrEmpty(sensitiveDataTypes) 
                    ? sensitiveDataTypes.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList()
                    : new List<string>(),
                HasProcessingAgreement = hasProcessingAgreement,
                ProcessingAgreementReference = processingAgreementReference,
                ProcessingAgreementDate = processingAgreementDate?.ToUniversalTime() ?? (hasProcessingAgreement ? DateTime.UtcNow : null),
                DataRetentionPeriod = dataRetentionDays.HasValue ? TimeSpan.FromDays(dataRetentionDays.Value) : null,
                DataDeletionProcess = dataDeletionProcess,
                SupportsDataPortability = supportsDataPortability,
                SupportsDataDeletion = supportsDataDeletion,
                SupportsDataCorrection = supportsDataCorrection,
                DataProtectionOfficerContact = dataProtectionOfficerContact,
                LastGdprAssessment = lastGdprAssessment?.ToUniversalTime() ?? DateTime.UtcNow,
                ComplianceNotes = complianceNotes,
                DataTransfers = !string.IsNullOrEmpty(dataTransfers)
                    ? dataTransfers.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList()
                    : new List<string>(),
                PurposesOfUse = !string.IsNullOrEmpty(purposesOfUse)
                    ? purposesOfUse.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList()
                    : new List<string>(),
                Controller = controller,
                ControllerId = controllerId,
                DpoOrganisation = dpoOrganisation
            };

            _context.GdprRegisters.Add(gdprRegister);
            await _context.SaveChangesAsync();
            
            catalogService.GdprRegisterId = gdprRegister.Id;
        }
        else
        {
            // Update existing GDPR register
            catalogService.GdprRegister.DataCategories = dataCategories ?? new List<DataCategory>();
            catalogService.GdprRegister.ProcessingPurposes = processingPurposes ?? new List<ProcessingPurpose>();
            catalogService.GdprRegister.ProcessingFrequency = processingFrequency;
            catalogService.GdprRegister.SecurityMeasures = securityMeasures ?? new List<SecurityMeasure>();
            catalogService.GdprRegister.ProcessesEmployeePii = processesEmployeePii;
            catalogService.GdprRegister.ProcessesExternalUserPii = processesExternalUserPii;
            catalogService.GdprRegister.ProcessesSensitiveData = processesSensitiveData;
            catalogService.GdprRegister.SensitiveDataTypes = !string.IsNullOrEmpty(sensitiveDataTypes)
                ? sensitiveDataTypes.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList()
                : new List<string>();
            catalogService.GdprRegister.HasProcessingAgreement = hasProcessingAgreement;
            catalogService.GdprRegister.ProcessingAgreementReference = processingAgreementReference;
            catalogService.GdprRegister.ProcessingAgreementDate = processingAgreementDate?.ToUniversalTime() ?? catalogService.GdprRegister.ProcessingAgreementDate;
            catalogService.GdprRegister.DataRetentionPeriod = dataRetentionDays.HasValue ? TimeSpan.FromDays(dataRetentionDays.Value) : null;
            catalogService.GdprRegister.DataDeletionProcess = dataDeletionProcess;
            catalogService.GdprRegister.SupportsDataPortability = supportsDataPortability;
            catalogService.GdprRegister.SupportsDataDeletion = supportsDataDeletion;
            catalogService.GdprRegister.SupportsDataCorrection = supportsDataCorrection;
            catalogService.GdprRegister.DataProtectionOfficerContact = dataProtectionOfficerContact;
            catalogService.GdprRegister.LastGdprAssessment = lastGdprAssessment?.ToUniversalTime() ?? DateTime.UtcNow;
            catalogService.GdprRegister.ComplianceNotes = complianceNotes;
            catalogService.GdprRegister.DataTransfers = !string.IsNullOrEmpty(dataTransfers)
                ? dataTransfers.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList()
                : new List<string>();
            catalogService.GdprRegister.PurposesOfUse = !string.IsNullOrEmpty(purposesOfUse)
                ? purposesOfUse.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList()
                : new List<string>();

            // Update controller reference
            if (controllerId.HasValue && controller != null)
            {
                catalogService.GdprRegister.Controller = controller;
                catalogService.GdprRegister.ControllerId = controllerId.Value;
            }
            else
            {
                // Remove controller if no longer provided
                catalogService.GdprRegister.Controller = null;
                catalogService.GdprRegister.ControllerId = null;
            }

            // Update or create DPO organisation
            if (hasExternalDpo && !string.IsNullOrEmpty(dpoOrganisationName))
            {
                if (catalogService.GdprRegister.DpoOrganisation == null)
                {
                    catalogService.GdprRegister.DpoOrganisation = dpoOrganisation;
                }
                else
                {
                    catalogService.GdprRegister.DpoOrganisation.Name = dpoOrganisationName;
                    catalogService.GdprRegister.DpoOrganisation.Email = dpoOrganisationEmail ?? "";
                    catalogService.GdprRegister.DpoOrganisation.Address = dpoOrganisationAddress ?? "";
                    catalogService.GdprRegister.DpoOrganisation.ZipCode = dpoOrganisationZipCode ?? "";
                    catalogService.GdprRegister.DpoOrganisation.City = dpoOrganisationCity ?? "";
                    catalogService.GdprRegister.DpoOrganisation.Country = dpoOrganisationCountry ?? "";
                    catalogService.GdprRegister.DpoOrganisation.Phone = dpoOrganisationPhone ?? "";
                }
            }
            else if (!hasExternalDpo && catalogService.GdprRegister.DpoOrganisation != null)
            {
                // Remove DPO organisation if checkbox is unchecked
                catalogService.GdprRegister.DpoOrganisation = null;
            }
        }

        await _context.SaveChangesAsync();
        
        TempData["Success"] = "GDPR compliance information has been successfully updated.";
        return RedirectToAction(nameof(Details), new { id = serviceId });
    }

    private bool CatalogServiceExists(int id)
    {
        return _context.Services.Any(e => e.Id == id);
    }
}