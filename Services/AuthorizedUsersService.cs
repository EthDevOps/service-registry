using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace QuokkaServiceRegistry.Services
{
    public class AuthorizedUsersService : IAuthorizedUsersService
    {
        private readonly HashSet<string> _authorizedUsers;

        public AuthorizedUsersService(IConfiguration configuration)
        {
            var authorizedUsersString = configuration["Authentication:AuthorizedUsers"] ?? "";
            _authorizedUsers = authorizedUsersString
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(email => email.Trim().ToLowerInvariant())
                .ToHashSet();
        }

        public bool IsUserAuthorized(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            return _authorizedUsers.Contains(email.Trim().ToLowerInvariant());
        }

        public IEnumerable<string> GetAuthorizedUsers()
        {
            return _authorizedUsers.ToList();
        }
    }
}