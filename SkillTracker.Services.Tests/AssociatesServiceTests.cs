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
    public class AssociatesServiceTests
    {
        private SkillTrackerEntities _context;
        IUnitOfWork unitOfWork;
        IAssociatesRepository associatesRepository;
        IAssociatesService associatesService;

        [SetUp]
        public void Setup()
        {
            _context = new SkillTrackerEntities();
            unitOfWork = new UnitOfWork(_context);
            associatesRepository = new AssociatesRepository(_context);
            associatesService = new AssociatesService(unitOfWork, associatesRepository);
        }
        [Test(), Order(1)]
        public void GetAllAssociates()
        {
            var associatesList = associatesService.GetAllAssociates().ToList();
            if (associatesList == null)
                Assert.Null(associatesList);
            Assert.NotNull(associatesList);
            Assert.GreaterOrEqual(associatesList.Count(), 0);
        }
        [Test(), Order(2)]
        public void GetAssociateById()
        {
            var input = new Associate
            {
                Associate_Id = 579946
            };
            var associate = associatesService.GetAssociate(input.Associate_Id);
            if (associate == null)
                Assert.Null(associate);
            Assert.NotNull(associate);
            Assert.AreEqual(associate.Associate_Id, input.Associate_Id);
        }
        [Test(), Order(3)]
        public void GetAssociateByEmail()
        {
            var input = new Associate
            {
                Email = "arulprakash.s@cognizant.com"
            };
            var associate = associatesService.GetAssociate(input.Email);
            if (associate == null)
                Assert.Null(associate);
            Assert.NotNull(associate);
            Assert.AreEqual(associate.Email, input.Email);
        }
        [Test(), Order(4)]
        public void CreateAssociateTest()
        {
            var input = new Associate
            {
                Associate_Id = 586790,
                Name = "TestServiceAssociate",
                Email = "testservice@gmail.com",
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
                   new Associate_Skills { Associate_Id = 575566, Rating =5, Skill_Id = 5 },
                   new Associate_Skills { Associate_Id = 575566, Rating =5, Skill_Id = 6 }
                }
            };
            var associate = associatesService.CreateAssociate(input);
            Assert.NotNull(associate);
            Assert.AreEqual(associate.Associate_Id, input.Associate_Id);
        }
        [Test(), Order(5)]
        public void UpdateAssociateTest()
        {
            var associate = associatesService.GetAssociate(586790);
            associate.Name = "TestServiceAssociateUpdated";
            var assoc = associatesService.UpdateAssociate(associate);
            Assert.NotNull(assoc);
            Assert.AreEqual(assoc.Name, associate.Name);
        }
        [Test(), Order(6)]
        public void DeleteAssociateTest()
        {
            var associate = associatesService.GetAssociate(586790);
            associatesService.RemoveAssociate(associate);
            var assoc = associatesService.GetAssociate(586790);
            Assert.Null(assoc);
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
            associatesService = null;
            associatesService = null;
        }
        #endregion
    }
}

