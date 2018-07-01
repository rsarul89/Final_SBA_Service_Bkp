using System.Data.Entity;
using SkillTracker.Entities;

namespace SkillTracker.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(DbContext context)
           : base(context) { }

        public void AddUser(User user)
        {
            Add(user);
        }

        public void DeleteUser(User user)
        {
            Delete(user);
        }

        public void UpdateUser(User user)
        {
            Update(user);
        }
    }
}
