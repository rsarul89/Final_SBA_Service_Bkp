using System.Collections.Generic;
using SkillTracker.Entities;

namespace SkillTracker.Services
{
    public interface IUserService : IEntityService<User>
    {
        IEnumerable<User> GetUsers();
        User GetUserByUserName(string userName);
        User GetUser(string userName, string password);
        void CreateUser(User user);
        void UpdateUser(User user);
        void DeleteUser(User user);
    }
}
