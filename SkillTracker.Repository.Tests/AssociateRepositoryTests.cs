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
    public class AssociateRepositoryTests
    {
        private SkillTrackerEntities _context;
        IUnitOfWork unitOfWork;
        IAssociatesRepository associatesRepository;

        [SetUp]
        public void Setup()
        {
            _context = new SkillTrackerEntities();
            unitOfWork = new UnitOfWork(_context);
            associatesRepository = new AssociatesRepository(_context);
        }
        [Test(), Order(1)]
        public void GetAllAssociates()
        {
            var associatesList = associatesRepository.GetAllAssociates().ToList();
            if (associatesList == null)
                Assert.Null(associatesList);
            Assert.GreaterOrEqual(associatesList.Count(), 0);
        }
        [Test(), Order(2)]
        public void GetAssociateById()
        {
            var input = new Associate
            {
                Associate_Id = 579946
            };
            var associate = associatesRepository.GetAssociate(input.Associate_Id);
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
            var associate = associatesRepository.GetAssociate(input.Email);
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
                Associate_Id = 567894,
                Name = "TestRepositoryAssociate",
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
            var associate = associatesRepository.AddAssociate(input);
            unitOfWork.Commit();
            Assert.NotNull(associate);
            Assert.AreEqual(associate.Associate_Id, input.Associate_Id);
        }
        [Test(), Order(5)]
        public void UpdateAssociateTest()
        {
            var associate = associatesRepository.GetAssociate(567894);
            associate.Name = "TestServiceAssociateUpdated";
            associatesRepository.UpdateAssociate(associate);
            unitOfWork.Commit();
            var assoc = associatesRepository.GetAssociate(567894);
            Assert.NotNull(assoc);
            Assert.AreEqual(assoc.Name, associate.Name);
        }
        [Test(), Order(6)]
        public void DeleteAssociateTest()
        {
            var associate = associatesRepository.GetAssociate(567894);
            associatesRepository.DeleteAssociate(associate);
            unitOfWork.Commit();
            var assoc = associatesRepository.GetAssociate(567894);
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
            associatesRepository = null;
        }
        #endregion
    }
}
