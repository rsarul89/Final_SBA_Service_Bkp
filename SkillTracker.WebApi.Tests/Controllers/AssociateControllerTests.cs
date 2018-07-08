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
using System.Linq;

namespace SkillTracker.WebApi.Tests.Controllers
{
    [TestFixture]
    public class AssociateControllerTests
    {
        #region Variables
        private SkillTrackerEntities _context;
        private IUnitOfWork _unitOfWork;
        private IAssociatesRepository _associatesRepository;
        private IAssociatesService _associatesService;
        private ILogManager _logManager;
        private HttpResponseMessage _response;
        private const string ServiceBaseURL = "http://localhost:64686/api/";
        #endregion
        [OneTimeSetUp]
        public void SetUp()
        {
            _context = new SkillTrackerEntities();
            _unitOfWork = new UnitOfWork(_context);
            _associatesRepository = new AssociatesRepository(_context);
            _associatesService = new AssociatesService(_unitOfWork, _associatesRepository);
            _logManager = new LogManager();
        }

        [Test(), Order(1)]
        public void GetAllAssociatesTest()
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
            Assert.AreEqual(_response.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(response != null, true);
            Assert.GreaterOrEqual(response.Count, 0);
        }

        [Test(), Order(2)]
        public void GetAssociateTest()
        {
            var request = new AssociateModel
            {
               Associate_Id = 579946
            };
            var response = new AssociateModel();
            var associateController = new AssociateController(_associatesService, _logManager)
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(ServiceBaseURL + "associate/getAssociate")
                }
            };
            associateController.Configuration = new HttpConfiguration();
            _response = associateController.GetAssociate(request);
            response = JsonConvert.DeserializeObject<AssociateModel>(_response.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(_response.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(response != null, true);
        }

        [Test(), Order(3)]
        public void AddAssociateTest()
        {
            var request = new AssociateModel
            {
                Associate_Id = 575566,
                Name = "TestAssociate",
                Email = "test@gmail.com",
                Mobile = "1234567890",
                Gender = "Male",
                Status_Blue = false,
                Status_Green = true,
                Status_Red = false,
                Level_1 = true,
                Level_2 = false,
                Level_3 = false,
                Pic = "",
                Remark = "Na",
                Strength = "Na",
                Other = "Na",
                Weakness = "",
                Associate_Skills = 
                {
                    new AssociateSkillsModel { Associate_Id = 575566, Rating =5, Skill_Id = 5 },
                   new AssociateSkillsModel { Associate_Id = 575566, Rating =5, Skill_Id = 6 }

                }
            };
            var response = new AssociateModel();
            var associateController = new AssociateController(_associatesService, _logManager)
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(ServiceBaseURL + "associate/addAssociate")
                }
            };
            associateController.Configuration = new HttpConfiguration();
            _response = associateController.AddAssociate(request);
            response = JsonConvert.DeserializeObject<AssociateModel>(_response.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(_response.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(response != null, true);
            Assert.Greater(response.Associate_Id, 0);
        }

        [Test(), Order(4)]
        public void UpdateAssociateTest()
        {
            var associate = _associatesService.GetAllAssociates().Where(a => a.Associate_Id == 575566).FirstOrDefault();
            var response = new AssociateModel();
            var request = Helper.CastObject<AssociateModel>(associate);
            request.Name = "TestAssociateUpdated";
            var associateController = new AssociateController(_associatesService, _logManager)
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(ServiceBaseURL + "associate/updateAssociate")
                }
            };
            associateController.Configuration = new HttpConfiguration();
            _response = associateController.UpdateAssociate(request);
            response = JsonConvert.DeserializeObject<AssociateModel>(_response.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(_response.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(response != null, true);
            Assert.AreEqual(response.Name, "TestAssociateUpdated");
        }

        [Test(), Order(5)]
        public void DeleteAssociateTest()
        {
            var associate = _associatesService.GetAllAssociates().Where(a => a.Associate_Id == 575566).FirstOrDefault();
            var response = new AssociateModel();
            var request = Helper.CastObject<AssociateModel>(associate);
            var associateController = new AssociateController(_associatesService, _logManager)
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(ServiceBaseURL + "associate/deleteAssociate")
                }
            };
            associateController.Configuration = new HttpConfiguration();
            _response = associateController.DeleteAssociate(request);
            response = JsonConvert.DeserializeObject<AssociateModel>(_response.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(_response.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(response != null, true);
        }

        [Test(), Order(6)]
        public void GetDashBoardData()
        {
            var response = new DashBoardDataModel();
            var associateController = new AssociateController(_associatesService, _logManager)
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(ServiceBaseURL + "associate/getDashBoardData")
                }
            };
            associateController.Configuration = new HttpConfiguration();
            _response = associateController.GetDashBoardData();
            response = JsonConvert.DeserializeObject<DashBoardDataModel>(_response.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(_response.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(response != null, true);        }

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
            _associatesRepository = null;
            _associatesService = null;
            _logManager = null;
            if (_response != null)
                _response.Dispose();
        }
        #endregion
    }
}
