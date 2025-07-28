using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuokkaServiceRegistry.Data;
using QuokkaServiceRegistry.Models;

namespace QuokkaServiceRegistry.Controllers;

public class CatalogVendorController : Controller
{
    private readonly ApplicationDbContext _context;

    public CatalogVendorController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: CatalogVendor
    public async Task<IActionResult> Index()
    {
        var vendors = await _context.Vendors
            .Include(v => v.BillingInformation)
                .ThenInclude(b => b.CostCenter)
            .ToListAsync();

        return View(vendors);
    }

    // GET: CatalogVendor/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var vendor = await _context.Vendors
            .Include(v => v.BillingInformation)
                .ThenInclude(b => b.CostCenter)
            .Include(v => v.BillingInformation)
                .ThenInclude(b => b.BillingAddress)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (vendor == null)
        {
            return NotFound();
        }

        // Get services using this vendor
        var services = await _context.Services
            .Where(s => s.Vendor.Id == id)
            .ToListAsync();

        ViewData["Services"] = services;

        return View(vendor);
    }

    // GET: CatalogVendor/Create
    public IActionResult Create()
    {
        ViewData["CostCenterId"] = new SelectList(_context.CostCenters, "Id", "Name");
        
        var viewModel = new VendorBillingViewModel();
        return View(viewModel);
    }

    // POST: CatalogVendor/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(VendorBillingViewModel viewModel)
    {
        // Remove validation errors for complex objects
        ModelState.Remove("BillingInformation");

        if (ModelState.IsValid)
        {
            // Get or create cost center
            CostCenter? costCenter = null;
            if (viewModel.CostCenterId.HasValue && viewModel.CostCenterId > 0)
            {
                costCenter = await _context.CostCenters.FindAsync(viewModel.CostCenterId.Value);
            }
            
            if (costCenter == null)
            {
                costCenter = new CostCenter { Name = "Default" };
                _context.CostCenters.Add(costCenter);
                await _context.SaveChangesAsync();
            }

            // Create billing information
            var billingInfo = new BillingInformation
            {
                CostCenter = costCenter
            };
            viewModel.UpdateBillingInformation(billingInfo, costCenter);
            _context.BillingInfo.Add(billingInfo);
            await _context.SaveChangesAsync();

            // Create vendor
            var vendor = new CatalogVendor
            {
                BillingInformation = billingInfo
            };
            viewModel.UpdateVendor(vendor);
            
            _context.Vendors.Add(vendor);
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }
        
        ViewData["CostCenterId"] = new SelectList(_context.CostCenters, "Id", "Name", viewModel.CostCenterId);
        
        return View(viewModel);
    }

    // GET: CatalogVendor/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var vendor = await _context.Vendors
            .Include(v => v.BillingInformation)
                .ThenInclude(b => b.CostCenter)
            .FirstOrDefaultAsync(v => v.Id == id);
            
        if (vendor == null)
        {
            return NotFound();
        }
        
        ViewData["CostCenterId"] = new SelectList(_context.CostCenters, "Id", "Name", vendor.BillingInformation?.CostCenter?.Id);
        
        var viewModel = VendorBillingViewModel.FromVendor(vendor);
        return View(viewModel);
    }

    // POST: CatalogVendor/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, VendorBillingViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return NotFound();
        }

        // Remove validation errors for complex objects
        ModelState.Remove("BillingInformation");

        if (ModelState.IsValid)
        {
            try
            {
                // Load existing vendor with billing information
                var vendor = await _context.Vendors
                    .Include(v => v.BillingInformation)
                        .ThenInclude(b => b.CostCenter)
                    .FirstOrDefaultAsync(v => v.Id == id);

                if (vendor == null)
                {
                    return NotFound();
                }

                // Update vendor properties
                viewModel.UpdateVendor(vendor);

                // Get or create cost center
                CostCenter? costCenter = null;
                if (viewModel.CostCenterId.HasValue && viewModel.CostCenterId > 0)
                {
                    costCenter = await _context.CostCenters.FindAsync(viewModel.CostCenterId.Value);
                }
                
                if (costCenter == null)
                {
                    costCenter = new CostCenter { Name = "Default" };
                    _context.CostCenters.Add(costCenter);
                    await _context.SaveChangesAsync();
                }

                // Update or create billing information
                if (vendor.BillingInformation == null)
                {
                    vendor.BillingInformation = new BillingInformation
                    {
                        CostCenter = costCenter
                    };
                    _context.BillingInfo.Add(vendor.BillingInformation);
                }

                viewModel.UpdateBillingInformation(vendor.BillingInformation, costCenter);
                
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VendorExists(viewModel.Id))
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
        
        ViewData["CostCenterId"] = new SelectList(_context.CostCenters, "Id", "Name", viewModel.CostCenterId);
        
        return View(viewModel);
    }

    // GET: CatalogVendor/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var vendor = await _context.Vendors
            .Include(v => v.BillingInformation)
            .FirstOrDefaultAsync(m => m.Id == id);
            
        if (vendor == null)
        {
            return NotFound();
        }

        // Check if vendor is used by any services
        var servicesCount = await _context.Services
            .CountAsync(s => s.Vendor.Id == id);
        
        ViewData["ServicesCount"] = servicesCount;

        return View(vendor);
    }

    // POST: CatalogVendor/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        // Check if vendor is used by any services
        var servicesCount = await _context.Services
            .CountAsync(s => s.Vendor.Id == id);
            
        if (servicesCount > 0)
        {
            TempData["Error"] = $"Cannot delete vendor. It is currently used by {servicesCount} service(s).";
            return RedirectToAction(nameof(Delete), new { id });
        }

        var vendor = await _context.Vendors.FindAsync(id);
        if (vendor != null)
        {
            _context.Vendors.Remove(vendor);
        }

        await _context.SaveChangesAsync();
        TempData["Success"] = "Vendor deleted successfully.";
        return RedirectToAction(nameof(Index));
    }

    private bool VendorExists(int id)
    {
        return _context.Vendors.Any(e => e.Id == id);
    }

    // AJAX endpoint for vendor quick create
    [HttpPost]
    public async Task<IActionResult> QuickCreate([FromBody] QuickCreateVendorRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest("Vendor name is required");
        }

        // Check if vendor already exists
        var existingVendor = await _context.Vendors
            .FirstOrDefaultAsync(v => v.Name.ToLower() == request.Name.ToLower());
            
        if (existingVendor != null)
        {
            return BadRequest("A vendor with this name already exists");
        }

        // Create basic billing info
        var costCenter = new CostCenter { Name = "Default" };
        _context.CostCenters.Add(costCenter);
        await _context.SaveChangesAsync();

        var billingInfo = new BillingInformation
        {
            Type = BillingType.Free,
            CostCenter = costCenter,
            IsActive = true
        };
        _context.BillingInfo.Add(billingInfo);
        await _context.SaveChangesAsync();

        var vendor = new CatalogVendor
        {
            Name = request.Name,
            WebsiteUrl = request.WebsiteUrl ?? "",
            Country = request.Country,
            City = request.City ?? "Unknown",
            BillingInformation = billingInfo
        };

        _context.Vendors.Add(vendor);
        await _context.SaveChangesAsync();

        return Json(new { id = vendor.Id, name = vendor.Name });
    }

    public class QuickCreateVendorRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? WebsiteUrl { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
    }
}