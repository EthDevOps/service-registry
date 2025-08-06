using System.Collections.Generic;

namespace QuokkaServiceRegistry.Services
{
    public interface IAuthorizedUsersService
    {
        bool IsUserAuthorized(string email);
        IEnumerable<string> GetAuthorizedUsers();
        void AddUser(string email);
        void RemoveUser(string email);
        string GetConfigurationString();
    }
}