using NBench;
using SkillTracker.Services;
using SkillTracker.Repositories;
using SkillTracker.Entities;
using SkillTracker.Common.Exception;
using SkillTracker.WebApi.Models;
using System.Net.Http;
using System.Net;
using System.Web.Http;
using Newtonsoft.Json;
using SkillTracker.WebApi.Controllers;
using System;
using System.Collections.Generic;

namespace SkillTracker.WebApi.Load.Tests
{
    public class LoadUserTests: PerformanceTestStuite<LoadUserTests>
    {
        #region Variables
        private SkillTrackerEntities _context;
        private IUnitOfWork _unitOfWork;
        private IUserRepository _userRepository;
        private IUserService _userService;
        private ILogManager _logManager;
        private HttpResponseMessage _response;
        private const string ServiceBaseURL = "http://localhost:64686/api";

        private const int AcceptableMinAddThroughput = 500;
        #endregion

        [PerfSetup]
        public void SetUp(BenchmarkContext context)
        {
            _context = new SkillTrackerEntities();
            _unitOfWork = new UnitOfWork(_context);
            _userRepository = new UserRepository(_context);
            _userService = new UserService(_unitOfWork, _userRepository);
            _logManager = new LogManager();
        }

        [PerfBenchmark(RunMode = RunMode.Iterations, NumberOfIterations = 500, TestMode = TestMode.Test, SkipWarmups = true)]
        [ElapsedTimeAssertion(MaxTimeMilliseconds = 600000)]
        public void GetUserBenchmark()
        {
            LoginModel loginrequest = new LoginModel { };
            loginrequest.user_name = "demouser";
            loginrequest.password = "test123";
            var response = new List<UserModel>();
            var userController = new AuthenticationController(_userService, _logManager)
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(ServiceBaseURL + "auth/getToken")
                }
            };
            userController.Configuration = new HttpConfiguration();
            _response = userController.Authenticate(loginrequest);
            response = JsonConvert.DeserializeObject<List<UserModel>>(_response.Content.ReadAsStringAsync().Result);
            if (_response.StatusCode == HttpStatusCode.OK)
            {

            }
        }

        [PerfCleanup]
        public void Cleanup(BenchmarkContext context)
        {
            _context = null;
            _unitOfWork = null;
            _userRepository = null;
            _userService = null;
            _logManager = null;
            if (_response != null)
                _response.Dispose();
        }
    }
}
