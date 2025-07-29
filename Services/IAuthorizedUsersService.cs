using System.Collections.Generic;

namespace QuokkaServiceRegistry.Services
{
    public interface IAuthorizedUsersService
    {
        bool IsUserAuthorized(string email);
        IEnumerable<string> GetAuthorizedUsers();
    }
}