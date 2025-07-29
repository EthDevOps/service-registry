using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuokkaServiceRegistry.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QuokkaServiceRegistry.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthorizedUsersService _authorizedUsersService;
        private readonly IConfiguration _configuration;

        public AccountController(IAuthorizedUsersService authorizedUsersService, IConfiguration configuration)
        {
            _authorizedUsersService = authorizedUsersService;
            _configuration = configuration;
        }

        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            ViewData["AllowLocalBypass"] = _configuration.GetValue<bool>("Authentication:AllowLocalAdminBypass");
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> LocalLogin(string username, string password, string? returnUrl = null)
        {
            var allowLocalBypass = _configuration.GetValue<bool>("Authentication:AllowLocalAdminBypass");
            var adminUsername = _configuration["Authentication:LocalAdmin:Username"] ?? "admin";
            var adminPassword = _configuration["Authentication:LocalAdmin:Password"] ?? "admin";

            if (!allowLocalBypass)
            {
                return BadRequest("Local admin bypass is not enabled");
            }

            if (username == adminUsername && password == adminPassword)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "admin"),
                    new Claim(ClaimTypes.Email, "admin@localhost"),
                    new Claim("iss", "local")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = false,
                    RedirectUri = returnUrl ?? "/"
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
                    new ClaimsPrincipal(claimsIdentity), authProperties);

                return LocalRedirect(returnUrl ?? "/");
            }

            ViewData["Error"] = "Invalid username or password";
            ViewData["ReturnUrl"] = returnUrl;
            ViewData["AllowLocalBypass"] = allowLocalBypass;
            return View("Login");
        }

        [AllowAnonymous]
        public IActionResult GoogleLogin(string? returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(GoogleResponse), "Account", new { ReturnUrl = returnUrl });
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [AllowAnonymous]
        public async Task<IActionResult> GoogleResponse(string? returnUrl = null)
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            
            if (!result.Succeeded)
            {
                return RedirectToAction(nameof(Login));
            }

            var emailClaim = result.Principal.FindFirst(ClaimTypes.Email)?.Value;
            
            if (string.IsNullOrEmpty(emailClaim) || !_authorizedUsersService.IsUserAuthorized(emailClaim))
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                ViewData["Error"] = "Access denied. Your email address is not authorized to access this application.";
                ViewData["Email"] = emailClaim;
                return View("AccessDenied");
            }

            return LocalRedirect(returnUrl ?? "/");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}