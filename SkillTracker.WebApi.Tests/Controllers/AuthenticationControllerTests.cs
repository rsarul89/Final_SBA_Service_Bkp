using SkillTracker.Services;
using SkillTracker.Repositories;
using SkillTracker.Entities;
using SkillTracker.Common.Exception;
using SkillTracker.WebApi.Models;
using System.Net.Http;
using System.Net;
using System.Web.Http;
using NUnit.Framework;
using Newtonsoft.Json;
using SkillTracker.WebApi.Controllers;
using System;

namespace SkillTracker.WebApi.Tests.Controllers
{
    [TestFixture]
    public class AuthenticationControllerTests
    {
        #region Variables
        private SkillTrackerEntities _context;
        private IUnitOfWork _unitOfWork;
        private IUserRepository _userRepository;
        private IUserService _userService;
        private ILogManager _logManager;
        private HttpResponseMessage _response;
        private const string ServiceBaseURL = "http://localhost:64686/api/";
        #endregion

        [OneTimeSetUp]
        public void SetUp()
        {
            _context = new SkillTrackerEntities();
            _unitOfWork = new UnitOfWork(_context);
            _userRepository = new UserRepository(_context);
            _userService = new UserService(_unitOfWork, _userRepository);
            _logManager = new LogManager();
        }

        [Test()]
        public void AuthenticateInvalidUserTest()
        {
            LoginModel loginrequest = new LoginModel { };
            loginrequest.user_name = "demouser1";
            loginrequest.password = "demopassword";
            var authenticationController = new AuthenticationController(_userService, _logManager)
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(ServiceBaseURL + "auth/getToken")
                }
            };
            authenticationController.Configuration = new HttpConfiguration();
            _response = authenticationController.Authenticate(loginrequest);
            Assert.AreEqual(_response.StatusCode, HttpStatusCode.Unauthorized);
        }

        [Test()]
        public void AuthenticateValidUserTest()
        {
            LoginModel loginrequest = new LoginModel { };
            loginrequest.user_name = "demouser";
            loginrequest.password = "test123";
            UserModel loginResponse;
            var authenticationController = new AuthenticationController(_userService, _logManager)
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(ServiceBaseURL + "auth/getToken")
                }
            };
            authenticationController.Configuration = new HttpConfiguration();
            _response = authenticationController.Authenticate(loginrequest);
            loginResponse = JsonConvert.DeserializeObject<UserModel>(_response.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(_response.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(loginResponse != null, true);
        }

        #region Tear Down
        /// <summary>
        /// Tears down each test data
        /// </summary>
        [TearDown]
        public void DisposeTest()
        {
            if (_response != null)
                _response.Dispose();
        }

        #endregion

        #region TestFixture TearDown.

        /// <summary>
        /// TestFixture teardown
        /// </summary>
        [OneTimeTearDown]
        public void DisposeAllObjects()
        {
            _context = null;
            _unitOfWork = null;
            _userRepository = null;
            _userService = null;
            _logManager = null;
            if (_response != null)
                _response.Dispose();
        }
        #endregion
    }
}
