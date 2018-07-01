using SkillTracker.Entities;

namespace SkillTracker.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        void AddUser(User user);
        void DeleteUser(User user);
        void UpdateUser(User user);
    }
}
