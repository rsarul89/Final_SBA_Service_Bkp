using System.Collections.Generic;
using System.Linq;
using SkillTracker.Entities;
using SkillTracker.Repositories;
using NUnit.Framework;
using static NUnit.Framework.Assert;
using System;

namespace SkillTracker.Services.Tests
{
    [TestFixture]
    public class UserServiceTests
    {
        private SkillTrackerEntities _context;
        IUnitOfWork unitOfWork;
        IUserRepository userRepository;
        IUserService userService;

        [SetUp]
        public void Setup()
        {
            _context = new SkillTrackerEntities();
            unitOfWork = new UnitOfWork(_context);
            userRepository = new UserRepository(_context);
            userService = new UserService(unitOfWork, userRepository);
        }

        [Test(), Order(1)]
        public void CreateUserTest()
        {
            User _user = new User()
            {
                user_id = 0,
                user_name = "TestServiceUser",
                password = "Demopassword",
                user_email="test@mail.com"
            };
            var usrList = userService.GetUsers().Where(x => x.user_name.Equals(_user.user_name, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            if (usrList != null)
            { }
            else
            {
                userService.CreateUser(_user);
                var result = userService.GetUsers().Where(x => x.user_name.Equals(_user.user_name, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                Assert.AreNotEqual(0, result.user_id);
            }
        }

        [Test(), Order(2)]
        public void GetUserTest()
        {
            var user = userService.GetUserByUserName("TestServiceUser");
            Assert.AreNotEqual(null, user);
        }

        [Test(), Order(3)]
        public void GetUserByUserNameTest()
        {
            var user = userService.GetUserByUserName("TestServiceUser");
            Assert.AreNotEqual(null, user);
        }

        [Test(), Order(4)]
        public void GetUsersTest()
        {
            var users = userService.GetUsers().ToList();
            Assert.AreNotEqual(null, users);
            Assert.AreNotEqual(0, users.Count());
        }

        [Test(), Order(5)]
        public void UpdateUserTest()
        {
            var uname = "TestServiceUser";
            var usrList = userService.GetUsers().Where(x => x.user_name.Equals(uname, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            if (usrList != null)
            {
                usrList.password = "DemoPassword1";
                userService.UpdateUser(usrList);
                var result = userService.GetUsers().Where(x => x.user_name.Equals(uname, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                Assert.AreEqual("DemoPassword1", result.password);
            }
        }

        [Test(), Order(6)]
        public void DeleteUserTest()
        {
            var uname = "TestServiceUser";
            var usrList = userService.GetUsers().Where(x => x.user_name.Equals(uname, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            if (usrList != null)
            {
                userService.DeleteUser(usrList);
                var result = userService.GetUsers().Where(x => x.user_name.Equals(uname, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
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
            userService = null; 
        }
        #endregion

    }
}
