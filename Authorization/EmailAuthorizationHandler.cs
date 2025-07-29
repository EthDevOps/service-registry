using Microsoft.AspNetCore.Authorization;
using QuokkaServiceRegistry.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QuokkaServiceRegistry.Authorization
{
    public class EmailAuthorizationRequirement : IAuthorizationRequirement
    {
    }

    public class EmailAuthorizationHandler : AuthorizationHandler<EmailAuthorizationRequirement>
    {
        private readonly IAuthorizedUsersService _authorizedUsersService;
        private readonly IConfiguration _configuration;

        public EmailAuthorizationHandler(IAuthorizedUsersService authorizedUsersService, IConfiguration configuration)
        {
            _authorizedUsersService = authorizedUsersService;
            _configuration = configuration;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, EmailAuthorizationRequirement requirement)
        {
            // Allow bypass for local development admin user
            var allowLocalBypass = _configuration.GetValue<bool>("Authentication:AllowLocalAdminBypass");
            if (allowLocalBypass && context.User.Identity?.Name == "admin")
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            // Check if user has email claim
            var emailClaim = context.User.FindFirst(ClaimTypes.Email) ?? context.User.FindFirst("email");
            if (emailClaim == null)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            // Check if email is in authorized list
            if (_authorizedUsersService.IsUserAuthorized(emailClaim.Value))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}