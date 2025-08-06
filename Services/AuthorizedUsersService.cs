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

        public void AddUser(string email)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                _authorizedUsers.Add(email.Trim().ToLowerInvariant());
            }
        }

        public void RemoveUser(string email)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                _authorizedUsers.Remove(email.Trim().ToLowerInvariant());
            }
        }

        public string GetConfigurationString()
        {
            return string.Join(",", _authorizedUsers);
        }
    }
}