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
            .Include(v => v.PaymentMethod)
                .ThenInclude(p => p.CostCenter)
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
            .Include(v => v.PaymentMethod)
                .ThenInclude(p => p.CostCenter)
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
        var paymentMethods = _context.PaymentMethods
            .Include(p => p.CostCenter)
            .Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = $"{p.Name} ({p.Type})",
                Group = new SelectListGroup { Name = p.Type.ToString() }
            })
            .ToList();
            
        ViewData["PaymentMethodId"] = paymentMethods;
        
        var viewModel = new VendorBillingViewModel();
        return View(viewModel);
    }

    // POST: CatalogVendor/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(VendorBillingViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            // Create vendor
            var vendor = new CatalogVendor();
            viewModel.UpdateVendor(vendor);
            
            _context.Vendors.Add(vendor);
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }
        
        // Repopulate ViewData on validation failure
        var paymentMethods = _context.PaymentMethods
            .Include(p => p.CostCenter)
            .Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = $"{p.Name} ({p.Type})",
                Group = new SelectListGroup { Name = p.Type.ToString() }
            })
            .ToList();
            
        ViewData["PaymentMethodId"] = paymentMethods;
        
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
            .Include(v => v.PaymentMethod)
                .ThenInclude(p => p.CostCenter)
            .FirstOrDefaultAsync(v => v.Id == id);
            
        if (vendor == null)
        {
            return NotFound();
        }
        
        var paymentMethods = _context.PaymentMethods
            .Include(p => p.CostCenter)
            .Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = $"{p.Name} ({p.Type})",
                Group = new SelectListGroup { Name = p.Type.ToString() },
                Selected = p.Id == vendor.PaymentMethodId
            })
            .ToList();
            
        ViewData["PaymentMethodId"] = paymentMethods;
        
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

        if (ModelState.IsValid)
        {
            try
            {
                // Load existing vendor
                var vendor = await _context.Vendors.FindAsync(id);

                if (vendor == null)
                {
                    return NotFound();
                }

                // Update vendor properties
                viewModel.UpdateVendor(vendor);
                
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
        
        // Repopulate ViewData on validation failure
        var paymentMethods = _context.PaymentMethods
            .Include(p => p.CostCenter)
            .Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = $"{p.Name} ({p.Type})",
                Group = new SelectListGroup { Name = p.Type.ToString() },
                Selected = p.Id == viewModel.PaymentMethodId
            })
            .ToList();
            
        ViewData["PaymentMethodId"] = paymentMethods;
        
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