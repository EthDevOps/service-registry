using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuokkaServiceRegistry.Data;
using QuokkaServiceRegistry.Models;

namespace QuokkaServiceRegistry.Controllers;

public class PublicCatalogController : Controller
{
    private readonly ApplicationDbContext _context;

    public PublicCatalogController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: PublicCatalog - Public catalog for non-authenticated users
    public async Task<IActionResult> Index(string? hostingType, string? hostingCountry, string? searchTerm)
    {
        var servicesQuery = _context.Services
            .Include(s => s.License)
            .Where(s => s.IsPublic == true)  // Only show public services
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

        var services = await servicesQuery.OrderBy(s => s.Name).ToListAsync();

        // Set up ViewBag for filters (same as authenticated version)
        ViewBag.HostingTypes = Enum.GetValues<CatalogHostingType>()
            .Select(ht => new SelectListItem 
            { 
                Value = ht.ToString(), 
                Text = ht.ToString(),
                Selected = ht.ToString() == hostingType 
            }).ToList();

        ViewBag.HostingCountries = await _context.Services
            .Where(s => s.IsPublic == true && !string.IsNullOrEmpty(s.HostingCountry))
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

    // GET: PublicCatalog/Details/5 - Public service details for non-authenticated users
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var catalogService = await _context.Services
            .Include(s => s.License)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (catalogService == null)
        {
            return NotFound();
        }

        // Check if the service is public
        if (!catalogService.IsPublic)
        {
            return NotFound(); // Don't reveal non-public services to unauthenticated users
        }

        return View(catalogService);
    }
}