using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuokkaServiceRegistry.Data;
using QuokkaServiceRegistry.Models;

namespace QuokkaServiceRegistry.Controllers;

[Authorize]
public class LicenseController : Controller
{
    private readonly ApplicationDbContext _context;

    public LicenseController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var licenses = await _context.Licenses.ToListAsync();
        return View(licenses);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var license = await _context.Licenses
            .FirstOrDefaultAsync(m => m.Id == id);
        if (license == null)
        {
            return NotFound();
        }

        return View(license);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Abbreviation,LicenseUrl,Type,IsCopyleft,IsOsiApproved,Description,RequiresAttribution,RequiresSourceDisclosure,CompatibilityLevel")] SoftwareLicense license)
    {
        if (ModelState.IsValid)
        {
            license.CreatedAt = DateTime.UtcNow;
            _context.Add(license);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(license);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var license = await _context.Licenses.FindAsync(id);
        if (license == null)
        {
            return NotFound();
        }
        return View(license);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Abbreviation,LicenseUrl,Type,IsCopyleft,IsOsiApproved,Description,RequiresAttribution,RequiresSourceDisclosure,CompatibilityLevel,CreatedAt")] SoftwareLicense license)
    {
        if (id != license.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                license.UpdatedAt = DateTime.UtcNow;
                _context.Update(license);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LicenseExists(license.Id))
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
        return View(license);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var license = await _context.Licenses
            .FirstOrDefaultAsync(m => m.Id == id);
        if (license == null)
        {
            return NotFound();
        }

        return View(license);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var license = await _context.Licenses.FindAsync(id);
        if (license != null)
        {
            _context.Licenses.Remove(license);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool LicenseExists(int id)
    {
        return _context.Licenses.Any(e => e.Id == id);
    }
}