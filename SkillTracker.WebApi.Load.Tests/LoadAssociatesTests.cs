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
    public class LoadAssociatesTests : PerformanceTestStuite<LoadAssociatesTests>
    {
        #region Variables
        private SkillTrackerEntities _context;
        private IUnitOfWork _unitOfWork;
        private IAssociatesRepository _associatesRepository;
        private IAssociatesService _associatesService;
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
            _associatesRepository = new AssociatesRepository(_context);
            _associatesService = new AssociatesService(_unitOfWork, _associatesRepository);
            _logManager = new LogManager();
        }

        [PerfBenchmark(RunMode = RunMode.Iterations, NumberOfIterations = 500, TestMode = TestMode.Test, SkipWarmups = true)]
        [ElapsedTimeAssertion(MaxTimeMilliseconds = 600000)]
        public void GetAssociateBenchmark()
        {
            var response = new List<AssociateModel>();
            var associateController = new AssociateController(_associatesService, _logManager)
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(ServiceBaseURL + "associate/getAssociates")
                }
            };
            associateController.Configuration = new HttpConfiguration();
            _response = associateController.GetAssociates();
            response = JsonConvert.DeserializeObject<List<AssociateModel>>(_response.Content.ReadAsStringAsync().Result);
            if (_response.StatusCode == HttpStatusCode.OK)
            {
               
            }
        }

        [PerfCleanup]
        public void Cleanup(BenchmarkContext context)
        {
            _context = null;
            _unitOfWork = null;
            _associatesRepository = null;
            _associatesService = null;
            _logManager = null;
            if (_response != null)
                _response.Dispose();
        }
    }
}
