using System.Collections.Generic;
using System.Linq;
using SkillTracker.Entities;
using SkillTracker.Repositories;
using NUnit.Framework;
using static NUnit.Framework.Assert;
using System;

namespace SkillTracker.Repository.Tests
{
    [TestFixture]
    public class UserRepositoryTests
    {
        private SkillTrackerEntities _context;
        IUnitOfWork unitOfWork;
        IUserRepository userRepository;

        [SetUp]
        public void Setup()
        {
            _context = new SkillTrackerEntities();
            unitOfWork = new UnitOfWork(_context);
            userRepository = new UserRepository(_context);
        }
        [Test(), Order(1)]
        public void AddUserTest()
        {
            User _user = new User()
            {
                user_id = 0,
                user_name = "TestRepositoryUser",
                password = "Demopassword",
                user_email = "test@mail.com"

            };
            var usrList = userRepository.FindBy(x => x.user_name.Equals(_user.user_name, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            if (usrList != null)
            { }
            else
            {
                userRepository.AddUser(_user);
                unitOfWork.Commit();
                var result = userRepository.FindBy(x => x.user_name.Equals(_user.user_name, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                Assert.AreNotEqual(0, result.user_id);
            }
        }
        [Test(), Order(2)]
        public void UserAlreadyExistsTest()
        {
            User _user = new User()
            {
                user_id = 0,
                user_name = "TestRepositoryUser",
                password = "Demopassword",
                user_email = "test@mail.com"

            };
            var usrList = userRepository.FindBy(x => x.user_name.Equals(_user.user_name, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            if (usrList != null)
            { }
            else
            {
                userRepository.AddUser(_user);
                unitOfWork.Commit();
                var result = userRepository.FindBy(x => x.user_name.Equals(_user.user_name, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                Assert.AreEqual(_user.user_name, result.user_name);
            }
        }

        [Test(), Order(3)]
        public void UpdateUserTest()
        {
            var uname = "TestRepositoryUser";
            var usrList = userRepository.FindBy(x => x.user_name.Equals(uname, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            if (usrList != null)
            {
                usrList.password = "DemoPassword1";
                userRepository.UpdateUser(usrList);
                unitOfWork.Commit();
                var result = userRepository.FindBy(x => x.user_name.Equals(uname, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                Assert.AreEqual("DemoPassword1", result.password);
            }
        }

        [Test(), Order(4)]
        public void DeleteUserTest()
        {
            var uname = "TestRepositoryUser";
            var usrList = userRepository.FindBy(x => x.user_name.Equals(uname, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            if (usrList != null)
            {
                userRepository.DeleteUser(usrList);
                unitOfWork.Commit();
                var result = userRepository.FindBy(x => x.user_name.Equals(uname, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                Assert.AreEqual(null, result);
            }
        }
        #region TestFixture TearDown.

        /// <summary>
        /// TestFixture teardown
        /// </summary>
        [OneTimeTearDown]
        public void DisposeAllObjects()
        {
            _context = null;
            unitOfWork = null;
            userRepository = null;
        }
        #endregion
    }
}
