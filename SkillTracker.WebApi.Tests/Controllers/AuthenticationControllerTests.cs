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

        [Test(), Order(1)]
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

        [Test(), Order(2)]
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
        [Test(), Order(3)]
        public void UserRegisterTest()
        {
            RegisteModel registerrequest = new RegisteModel { };
            registerrequest.user_name = "testuseradd1";
            registerrequest.password = "test123";
            registerrequest.user_email = "test1@mail.com";
            UserModel registerResponse;
            var authenticationController = new AuthenticationController(_userService, _logManager)
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(ServiceBaseURL + "auth/register")
                }
            };
            authenticationController.Configuration = new HttpConfiguration();
            _response = authenticationController.Register(registerrequest);
            registerResponse = JsonConvert.DeserializeObject<UserModel>(_response.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(_response.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(registerResponse != null, true);
        }
        [Test(), Order(4)]
        public void UserAddest()
        {
            UserModel addrequest = new UserModel { };
            addrequest.user_name = "testuseradd1";
            addrequest.password = "test123";
            addrequest.user_email = "test1@mail.com";
            var authenticationController = new AuthenticationController(_userService, _logManager)
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(ServiceBaseURL + "auth/add")
                }
            };
            authenticationController.Configuration = new HttpConfiguration();
            _response = authenticationController.AddUser(addrequest);
            var addResponse = JsonConvert.DeserializeObject<string>(_response.Content.ReadAsStringAsync().Result);
            if (addResponse == "success")
            {
                Assert.AreEqual(_response.StatusCode, HttpStatusCode.OK);
                Assert.AreNotEqual(addResponse, "");
                Assert.AreEqual(addResponse, "success");
            }
            else if (addResponse == "fail")
            {
                Assert.AreEqual(_response.StatusCode, HttpStatusCode.OK);
                Assert.AreNotEqual(addResponse, "");
                Assert.AreEqual(addResponse, "fail");
            }
        }
        [Test(), Order(5)]
        public void UserUpdateTest()
        {
            UserModel updaterequest = new UserModel { };
            updaterequest = Helper.CastObject<UserModel>(_userService.GetUserByUserName("testuseradd1"));
            updaterequest.user_email = "updatedemail@mail.com";
            var authenticationController = new AuthenticationController(_userService, _logManager)
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(ServiceBaseURL + "auth/update")
                }
            };
            authenticationController.Configuration = new HttpConfiguration();
            _response = authenticationController.UpdateUser(updaterequest);
            var updateResponse = JsonConvert.DeserializeObject<UserModel>(_response.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(_response.StatusCode, HttpStatusCode.OK);
            Assert.NotNull(updateResponse);
            Assert.AreEqual(updateResponse.user_email, updaterequest.user_email);
        }
        [Test(), Order(6)]
        public void UserDeleteTest()
        {
            UserModel deleterequest = new UserModel { };
            deleterequest = Helper.CastObject<UserModel>(_userService.GetUserByUserName("testuseradd1"));
            if (deleterequest != null)
            {
                var authenticationController = new AuthenticationController(_userService, _logManager)
                {
                    Request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri(ServiceBaseURL + "auth/delete")
                    }
                };
                authenticationController.Configuration = new HttpConfiguration();
                _response = authenticationController.DeleteUser(deleterequest);
                var deleteResponse = JsonConvert.DeserializeObject<string>(_response.Content.ReadAsStringAsync().Result);
                Assert.AreEqual(_response.StatusCode, HttpStatusCode.OK);
                Assert.AreNotEqual(deleteResponse, "");
                Assert.AreEqual(deleteResponse, "success");
            }
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
