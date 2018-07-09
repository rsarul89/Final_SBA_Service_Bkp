using System;
using System.Collections.Generic;
using System.Linq;
using SkillTracker.Entities;
using SkillTracker.Repositories;

namespace SkillTracker.Services
{
    public class UserService : EntityService<User>, IUserService
    {
        IUnitOfWork unitOfWork;
        IUserRepository userRepository;

        public UserService(IUnitOfWork unitOfWork, IUserRepository userRepository) : base(unitOfWork, userRepository)
        {
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
        }
        public void CreateUser(User user)
        {
            var usr = userRepository.FindBy(u => u.user_name.Equals(user.user_name, StringComparison.InvariantCultureIgnoreCase))
                .FirstOrDefault();
            if (usr == null)
            {
                Create(user);
                unitOfWork.Commit();
            }
        }

        public void DeleteUser(User user)
        {
            var usr = userRepository.FindBy(u => u.user_name.Equals(user.user_name, StringComparison.InvariantCultureIgnoreCase))
                .FirstOrDefault();
            if (usr != null)
            {
                userRepository.DeleteUser(user);
                unitOfWork.Commit();
            }
        }

        public User GetUser(string userName, string password)
        {
            return userRepository.FindBy(u => u.user_name.Equals(userName, StringComparison.InvariantCultureIgnoreCase) && u.password == password)
                .FirstOrDefault();
        }

        public User GetUserByUserName(string userName)
        {
            return userRepository.FindBy(u => u.user_name.Equals(userName, StringComparison.InvariantCultureIgnoreCase))
                .FirstOrDefault();
        }

        public IEnumerable<User> GetUsers()
        {
            return userRepository.GetAll();
        }
        public void UpdateUser(User user)
        {
            var usr = userRepository.FindBy(u => u.user_name.Equals(user.user_name, StringComparison.InvariantCultureIgnoreCase))
                .FirstOrDefault();
            if (usr != null)
            {
                userRepository.UpdateUser(user);
                unitOfWork.Commit();
            }
        }
    }
}
