using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuokkaServiceRegistry.Data;
using QuokkaServiceRegistry.Models;
using QuokkaServiceRegistry.Services;

namespace QuokkaServiceRegistry.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthorizedUsersService _authorizedUsersService;
        private readonly IConfiguration _configuration;

        public AdminController(ApplicationDbContext context, IAuthorizedUsersService authorizedUsersService, IConfiguration configuration)
        {
            _context = context;
            _authorizedUsersService = authorizedUsersService;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Users()
        {
            var authorizedUsers = _authorizedUsersService.GetAuthorizedUsers().ToList();
            return View(authorizedUsers);
        }

        [HttpPost]
        public IActionResult AddUser(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                TempData["Error"] = "Email address is required.";
                return RedirectToAction(nameof(Users));
            }

            if (_authorizedUsersService.IsUserAuthorized(email))
            {
                TempData["Error"] = "User is already authorized.";
                return RedirectToAction(nameof(Users));
            }

            _authorizedUsersService.AddUser(email);
            var updatedUsersString = _authorizedUsersService.GetConfigurationString();
            
            TempData["Success"] = $"User '{email}' added successfully.";
            TempData["Info"] = "Note: This change is temporary and requires updating the configuration permanently.";
            TempData["ConfigUpdate"] = $"Authentication:AuthorizedUsers={updatedUsersString}";
            
            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        public IActionResult RemoveUser(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                TempData["Error"] = "Email address is required.";
                return RedirectToAction(nameof(Users));
            }

            if (!_authorizedUsersService.IsUserAuthorized(email))
            {
                TempData["Error"] = "User is not in the authorized list.";
                return RedirectToAction(nameof(Users));
            }

            _authorizedUsersService.RemoveUser(email);
            var updatedUsersString = _authorizedUsersService.GetConfigurationString();
            
            TempData["Success"] = $"User '{email}' removed successfully.";
            TempData["Info"] = "Note: This change is temporary and requires updating the configuration permanently.";
            TempData["ConfigUpdate"] = $"Authentication:AuthorizedUsers={updatedUsersString}";
            
            return RedirectToAction(nameof(Users));
        }

        public async Task<IActionResult> CostCenters()
        {
            var costCenters = await _context.CostCenters.ToListAsync();
            return View(costCenters);
        }

        [HttpGet]
        public IActionResult CreateCostCenter()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCostCenter([Bind("Name,Code,Description")] CostCenter costCenter)
        {
            if (ModelState.IsValid)
            {
                _context.Add(costCenter);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Cost center created successfully.";
                return RedirectToAction(nameof(CostCenters));
            }
            return View(costCenter);
        }

        [HttpGet]
        public async Task<IActionResult> EditCostCenter(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var costCenter = await _context.CostCenters.FindAsync(id);
            if (costCenter == null)
            {
                return NotFound();
            }
            return View(costCenter);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCostCenter(int id, [Bind("Id,Name,Code,Description")] CostCenter costCenter)
        {
            if (id != costCenter.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(costCenter);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Cost center updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CostCenterExists(costCenter.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(CostCenters));
            }
            return View(costCenter);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCostCenter(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var costCenter = await _context.CostCenters
                .FirstOrDefaultAsync(m => m.Id == id);
            if (costCenter == null)
            {
                return NotFound();
            }

            return View(costCenter);
        }

        [HttpPost, ActionName("DeleteCostCenter")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCostCenterConfirmed(int id)
        {
            var costCenter = await _context.CostCenters.FindAsync(id);
            if (costCenter != null)
            {
                _context.CostCenters.Remove(costCenter);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Cost center deleted successfully.";
            }

            return RedirectToAction(nameof(CostCenters));
        }

        private bool CostCenterExists(int id)
        {
            return _context.CostCenters.Any(e => e.Id == id);
        }

        // GDPR Data Controllers Management
        public async Task<IActionResult> DataControllers()
        {
            var controllers = await _context.GdprControllers
                .Include(c => c.CostCenter)
                .ToListAsync();
            return View(controllers);
        }

        [HttpGet]
        public async Task<IActionResult> CreateDataController()
        {
            ViewBag.CostCenters = await _context.CostCenters.ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDataController(IFormCollection form)
        {
            // Manual model binding from form
            var controller = new GdprController
            {
                Name = form["Name"].ToString() ?? "",
                Email = form["Email"].ToString() ?? "",
                Address = form["Address"].ToString() ?? "",
                ZipCode = form["ZipCode"].ToString() ?? "",
                City = form["City"].ToString() ?? "",
                Country = form["Country"].ToString() ?? "",
                Phone = form["Phone"].ToString() ?? "",
                DataOwner = form["DataOwner"].ToString() ?? "",
                CostCenterId = int.TryParse(form["CostCenterId"], out var costCenterId) ? costCenterId : null
            };

            if (ModelState.IsValid)
            {
                _context.Add(controller);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Data controller created successfully.";
                return RedirectToAction(nameof(DataControllers));
            }
            ViewBag.CostCenters = await _context.CostCenters.ToListAsync();
            return View(controller);
        }

        [HttpGet]
        public async Task<IActionResult> EditDataController(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var controller = await _context.GdprControllers
                .Include(c => c.CostCenter)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (controller == null)
            {
                return NotFound();
            }
            ViewBag.CostCenters = await _context.CostCenters.ToListAsync();
            return View(controller);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDataController(int id, IFormCollection form)
        {
            // Manual model binding from form
            var controller = new GdprController
            {
                Id = int.TryParse(form["Id"], out var parsedId) ? parsedId : 0,
                Name = form["Name"].ToString() ?? "",
                Email = form["Email"].ToString() ?? "",
                Address = form["Address"].ToString() ?? "",
                ZipCode = form["ZipCode"].ToString() ?? "",
                City = form["City"].ToString() ?? "",
                Country = form["Country"].ToString() ?? "",
                Phone = form["Phone"].ToString() ?? "",
                DataOwner = form["DataOwner"].ToString() ?? "",
                CostCenterId = int.TryParse(form["CostCenterId"], out var costCenterId) ? costCenterId : null
            };
            
            if (id != controller.Id)
            {
                TempData["Error"] = $"Route ID ({id}) does not match controller ID ({controller.Id})";
                return RedirectToAction(nameof(DataControllers));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(controller);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Data controller updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DataControllerExists(controller.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(DataControllers));
            }
            ViewBag.CostCenters = await _context.CostCenters.ToListAsync();
            return View(controller);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteDataController(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var controller = await _context.GdprControllers
                .Include(c => c.CostCenter)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (controller == null)
            {
                return NotFound();
            }

            return View(controller);
        }

        [HttpPost, ActionName("DeleteDataController")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDataControllerConfirmed(int id)
        {
            var controller = await _context.GdprControllers.FindAsync(id);
            if (controller != null)
            {
                _context.GdprControllers.Remove(controller);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Data controller deleted successfully.";
            }

            return RedirectToAction(nameof(DataControllers));
        }

        private bool DataControllerExists(int id)
        {
            return _context.GdprControllers.Any(e => e.Id == id);
        }
    }
}