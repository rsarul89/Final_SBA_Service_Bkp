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
using System.Collections.Generic;

namespace SkillTracker.WebApi.Tests.Controllers
{
    [TestFixture]
    public class SkillControllerTests
    {
        #region Variables
        private SkillTrackerEntities _context;
        private IUnitOfWork _unitOfWork;
        private ISkillsRepository _skillsRepository;
        private ISkillsService _skillsService;
        private ILogManager _logManager;
        private HttpResponseMessage _response;
        private const string ServiceBaseURL = "http://localhost:64686/api/";
        #endregion
        [OneTimeSetUp]
        public void SetUp()
        {
            _context = new SkillTrackerEntities();
            _unitOfWork = new UnitOfWork(_context);
            _skillsRepository = new SkillsRepository(_context);
            _skillsService = new SkillsService(_unitOfWork, _skillsRepository);
            _logManager = new LogManager();
        }

        [Test(), Order(1)]
        public void GetAllSkillsTest()
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
            Assert.AreEqual(_response.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(response != null, true);
            Assert.GreaterOrEqual(response.Count, 0);
        }

        [Test(), Order(2)]
        public void GetSkillTest()
        {
            var request = new SkillModel
            {
                Skill_Id = 1,
                Skill_Name = "HTML5",
                Associate_Skills = null
            };
            var response = new SkillModel();
            var skillController = new SkillController(_skillsService, _logManager)
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(ServiceBaseURL + "skill/getSkill")
                }
            };
            skillController.Configuration = new HttpConfiguration();
            _response = skillController.GetSkill(request);
            response = JsonConvert.DeserializeObject<SkillModel>(_response.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(_response.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(response != null, true);
        }

        [Test(), Order(3)]
        public void AddSkillTest()
        {
            var request = new SkillModel
            {
                Skill_Id = 0,
                Skill_Name = "TestSkill",
                Associate_Skills = null
            };
            var response = new SkillModel();
            var skillController = new SkillController(_skillsService, _logManager)
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(ServiceBaseURL + "skill/addSkill")
                }
            };
            skillController.Configuration = new HttpConfiguration();
            _response = skillController.AddSkill(request);
            response = JsonConvert.DeserializeObject<SkillModel>(_response.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(_response.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(response != null, true);
            Assert.Greater(response.Skill_Id, 0);
        }

        [Test(), Order(4)]
        public void UpdateSkillTest()
        {
            var skill = _skillsService.GetSkillByName("TestSkill");
            var response = new SkillModel();
            var request = Helper.CastObject<SkillModel>(skill);
            request.Skill_Name = "TestSkillUpdated";
            var skillController = new SkillController(_skillsService, _logManager)
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(ServiceBaseURL + "skill/updateSkill")
                }
            };
            skillController.Configuration = new HttpConfiguration();
            _response = skillController.UpdateSkill(request);
            response = JsonConvert.DeserializeObject<SkillModel>(_response.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(_response.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(response != null, true);
            Assert.AreEqual(response.Skill_Name, "TestSkillUpdated");
        }

        [Test(), Order(5)]
        public void DeleteSkillTest()
        {
            var skill = _skillsService.GetSkillByName("TestSkillUpdated");
            var response = new SkillModel();
            var request = Helper.CastObject<SkillModel>(skill);
            var skillController = new SkillController(_skillsService, _logManager)
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(ServiceBaseURL + "skill/deleteSkill")
                }
            };
            skillController.Configuration = new HttpConfiguration();
            _response = skillController.DeleteSkill(request);
            response = JsonConvert.DeserializeObject<SkillModel>(_response.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(_response.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(response != null, true);
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
            _skillsRepository = null;
            _skillsService = null;
            _logManager = null;
            if (_response != null)
                _response.Dispose();
        }
        #endregion

    }
}
