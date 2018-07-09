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
    public class LoadSkillsTest : PerformanceTestStuite<LoadSkillsTest>
    {
        #region Variables
        private SkillTrackerEntities _context;
        private IUnitOfWork _unitOfWork;
        private ISkillsRepository _skillsRepository;
        private ISkillsService _skillsService;
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
            _skillsRepository = new SkillsRepository(_context);
            _skillsService = new SkillsService(_unitOfWork, _skillsRepository);
            _logManager = new LogManager();
        }

        [PerfBenchmark(RunMode = RunMode.Iterations, NumberOfIterations = 500, TestMode = TestMode.Test, SkipWarmups = true)]
        [ElapsedTimeAssertion(MaxTimeMilliseconds = 600000)]
        public void GetSkillBenchmark()
        {
            var response = new List<SkillModel>();
            var skillController = new SkillController(_skillsService, _logManager)
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(ServiceBaseURL + "skill/getSkills")
                }
            };
            skillController.Configuration = new HttpConfiguration();
            _response = skillController.GetSkills();
            response = JsonConvert.DeserializeObject<List<SkillModel>>(_response.Content.ReadAsStringAsync().Result);
            if (_response.StatusCode == HttpStatusCode.OK)
            {
                
            }
        }

        [PerfCleanup]
        public void Cleanup(BenchmarkContext context)
        {
            _context = null;
            _unitOfWork = null;
            _skillsRepository = null;
            _skillsService = null;
            _logManager = null;
            if (_response != null)
                _response.Dispose();
        }
    }
}
