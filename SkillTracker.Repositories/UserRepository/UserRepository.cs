using System.Data.Entity;
using SkillTracker.Entities;
using System.Linq;

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
            var result = FindBy(a => a.user_id == user.user_id).FirstOrDefault();
            Delete(result);
        }

        public void UpdateUser(User user)
        {
            var entry = _entities.Entry<User>(user);
            if (entry.State == System.Data.EntityState.Detached)
            {
                var set = _entities.Set<User>();
                var attachedEntity = set.Find(user.user_id);
                if (attachedEntity != null)
                {
                    var attachedEntry = _entities.Entry(attachedEntity);
                    attachedEntry.CurrentValues.SetValues(user);
                }
                else
                {
                    entry.State = System.Data.EntityState.Modified;
                }
            }
            else
            {
                Update(user);
            }
        }
    }
}
