using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuokkaServiceRegistry.Data;
using QuokkaServiceRegistry.Models;

namespace QuokkaServiceRegistry.Controllers;

[Authorize]
public class PaymentController : Controller
{
    private readonly ApplicationDbContext _context;

    public PaymentController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Payment
    public async Task<IActionResult> Index()
    {
        var paymentMethods = await _context.PaymentMethods
            .Include(p => p.CostCenter)
            .Include(p => p.Vendors)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
            
        return View(paymentMethods);
    }

    // GET: Payment/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var paymentMethod = await _context.PaymentMethods
            .Include(p => p.CostCenter)
            .Include(p => p.Vendors)
            .FirstOrDefaultAsync(m => m.Id == id);
            
        if (paymentMethod == null)
        {
            return NotFound();
        }

        return View(paymentMethod);
    }

    // GET: Payment/Create
    public IActionResult Create()
    {
        ViewData["CostCenterId"] = new SelectList(_context.CostCenters, "Id", "Name");
        return View(new PaymentMethod());
    }

    // POST: Payment/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PaymentMethod paymentMethod)
    {
        // Remove navigation property from validation since it's not bound
        ModelState.Remove("CostCenter");
        
        if (ModelState.IsValid)
        {
            paymentMethod.CreatedAt = DateTime.UtcNow;
            _context.Add(paymentMethod);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Payment method created successfully.";
            return RedirectToAction(nameof(Index));
        }
        
        // Log validation errors for debugging
        foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
        {
            Console.WriteLine($"Validation Error: {modelError.ErrorMessage}");
        }
        
        ViewData["CostCenterId"] = new SelectList(_context.CostCenters, "Id", "Name", paymentMethod.CostCenterId);
        return View(paymentMethod);
    }

    // GET: Payment/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var paymentMethod = await _context.PaymentMethods.FindAsync(id);
        if (paymentMethod == null)
        {
            return NotFound();
        }
        
        ViewData["CostCenterId"] = new SelectList(_context.CostCenters, "Id", "Name", paymentMethod.CostCenterId);
        return View(paymentMethod);
    }

    // POST: Payment/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Type,CostCenterId,Description,IsActive,CreatedAt,CardNumber,CardHolderName,ExpiryDate,BankName,AccountNumber,RoutingNumber,Iban,Swift,SepaMandateId,SepaCreditorId,SepaMandateDate,PrepaidVoucherCode,PrepaidBalance,PrepaidExpiryDate,PaymentMethodId,OutstandingBalance,LastPaymentDate,NextBillingDate")] PaymentMethod paymentMethod)
    {
        if (id != paymentMethod.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                paymentMethod.UpdatedAt = DateTime.UtcNow;
                _context.Update(paymentMethod);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Payment method updated successfully.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentMethodExists(paymentMethod.Id))
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
        
        ViewData["CostCenterId"] = new SelectList(_context.CostCenters, "Id", "Name", paymentMethod.CostCenterId);
        return View(paymentMethod);
    }

    // GET: Payment/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var paymentMethod = await _context.PaymentMethods
            .Include(p => p.CostCenter)
            .Include(p => p.Vendors)
            .FirstOrDefaultAsync(m => m.Id == id);
            
        if (paymentMethod == null)
        {
            return NotFound();
        }

        return View(paymentMethod);
    }

    // POST: Payment/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var paymentMethod = await _context.PaymentMethods
            .Include(p => p.Vendors)
            .FirstOrDefaultAsync(p => p.Id == id);
            
        if (paymentMethod != null)
        {
            if (paymentMethod.Vendors.Any())
            {
                TempData["Error"] = "Cannot delete payment method that is still linked to vendors.";
                return RedirectToAction(nameof(Delete), new { id });
            }
            
            _context.PaymentMethods.Remove(paymentMethod);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Payment method deleted successfully.";
        }

        return RedirectToAction(nameof(Index));
    }

    private bool PaymentMethodExists(int id)
    {
        return _context.PaymentMethods.Any(e => e.Id == id);
    }
}