using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuokkaServiceRegistry.Data;
using QuokkaServiceRegistry.Models;

namespace QuokkaServiceRegistry.Controllers;

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
            .Include(s => s.Vendor)
            .Include(s => s.License)
            .Include(s => s.OnpremiseHosts)
            .ThenInclude(h => h.CloudProvider)
            .Include(s => s.Lifecycle)
            .Include(s => s.Subscription)
            .Include(s => s.GdprRegister)
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
                s.Vendor.Name.Contains(searchTerm) ||
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
            .Include(s => s.Vendor)
                .ThenInclude(v => v.BillingInformation)
                    .ThenInclude(b => b.CostCenter)
            .Include(s => s.License)
            .Include(s => s.OnpremiseHosts)
                .ThenInclude(h => h.CloudProvider)
            .Include(s => s.Lifecycle)
                .ThenInclude(l => l.Stages)
            .Include(s => s.Subscription)
            .Include(s => s.GdprRegister)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (catalogService == null)
        {
            return NotFound();
        }

        return View(catalogService);
    }

    // GET: CatalogService/Create
    public IActionResult Create()
    {
        ViewData["VendorId"] = new SelectList(_context.Vendors, "Id", "Name");
        ViewData["LicenseId"] = new SelectList(_context.Licenses, "Id", "Name");
        ViewData["SubscriptionId"] = new SelectList(_context.Subscriptions, "Id", "Name");
        ViewData["GdprRegisterId"] = new SelectList(_context.GdprRegisters, "Id", "Id");
        
        return View();
    }

    // POST: CatalogService/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,IsPropriety,VendorId,LicenseId,HostingType,HostingCountry,SaasRegionReference,SubscriptionId,GdprRegisterId")] CatalogService catalogService, ServiceLifecycleStageType initialStage = ServiceLifecycleStageType.ProofOfConcept)
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
        
        ViewData["VendorId"] = new SelectList(_context.Vendors, "Id", "Name", catalogService.VendorId);
        ViewData["LicenseId"] = new SelectList(_context.Licenses, "Id", "Name", catalogService.LicenseId);
        ViewData["SubscriptionId"] = new SelectList(_context.Subscriptions, "Id", "Name", catalogService.SubscriptionId);
        ViewData["GdprRegisterId"] = new SelectList(_context.GdprRegisters, "Id", "Id", catalogService.GdprRegisterId);
        
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
        
        ViewData["VendorId"] = new SelectList(_context.Vendors, "Id", "Name", catalogService.Vendor?.Id);
        ViewData["LicenseId"] = new SelectList(_context.Licenses, "Id", "Name", catalogService.License?.Id);
        // Hosting properties are now directly on CatalogService model
        ViewData["LifecycleId"] = new SelectList(_context.ServiceLifecycles, "Id", "CurrentStage", catalogService.Lifecycle?.Id);
        ViewData["SubscriptionId"] = new SelectList(_context.Subscriptions, "Id", "Name", catalogService.Subscription?.Id);
        ViewData["GdprRegisterId"] = new SelectList(_context.GdprRegisters, "Id", "Id", catalogService.GdprRegister?.Id);
        
        return View(catalogService);
    }

    // POST: CatalogService/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,IsPropriety,VendorId,LicenseId,HostingId,LifecycleId,SubscriptionId,GdprRegisterId")] CatalogService catalogService)
    {
        if (id != catalogService.Id)
        {
            return NotFound();
        }

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
        
        ViewData["VendorId"] = new SelectList(_context.Vendors, "Id", "Name", catalogService.Vendor?.Id);
        ViewData["LicenseId"] = new SelectList(_context.Licenses, "Id", "Name", catalogService.License?.Id);
        // Hosting properties are now directly on CatalogService model
        ViewData["LifecycleId"] = new SelectList(_context.ServiceLifecycles, "Id", "CurrentStage", catalogService.Lifecycle?.Id);
        ViewData["SubscriptionId"] = new SelectList(_context.Subscriptions, "Id", "Name", catalogService.Subscription?.Id);
        ViewData["GdprRegisterId"] = new SelectList(_context.GdprRegisters, "Id", "Id", catalogService.GdprRegister?.Id);
        
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
            .Include(s => s.Vendor)
            .Include(s => s.License)
            .Include(s => s.OnpremiseHosts)
            .ThenInclude(h => h.CloudProvider)
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

    private bool CatalogServiceExists(int id)
    {
        return _context.Services.Any(e => e.Id == id);
    }
}