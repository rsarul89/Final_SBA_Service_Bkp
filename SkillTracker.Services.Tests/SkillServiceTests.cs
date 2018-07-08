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
    public class SkillServiceTests
    {
        private SkillTrackerEntities _context;
        IUnitOfWork unitOfWork;
        ISkillsRepository skillsRepository;
        ISkillsService skillsService;

        [SetUp]
        public void Setup()
        {
            _context = new SkillTrackerEntities();
            unitOfWork = new UnitOfWork(_context);
            skillsRepository = new SkillsRepository(_context);
            skillsService = new SkillsService(unitOfWork, skillsRepository);
        }
        [Test(), Order(1)]
        public void GetAllSkills()
        {
            var skillList = skillsService.GetAllSkills().ToList();
            Assert.NotNull(skillList);
            Assert.GreaterOrEqual(skillList.Count(), 0);
        }
        [Test(), Order(2)]
        public void GetSkill()
        {
            var input = new Skill
            {
                Skill_Id = 1,
                Skill_Name = "HTML5",
                Associate_Skills = null
            };
            var skill = skillsService.GetSkill(input.Skill_Id);
            if (skill == null)
                Assert.Null(skill);
            Assert.NotNull(skill);
            Assert.AreEqual(skill.Skill_Id, input.Skill_Id);
        }
        [Test(), Order(3)]
        public void GetSkillByName()
        {
            var input = new Skill
            {
                Skill_Id = 1,
                Skill_Name = "HTML5",
                Associate_Skills = null
            };
            var skill = skillsService.GetSkillByName(input.Skill_Name);
            if (skill == null)
                Assert.Null(skill);
            Assert.NotNull(skill);
            Assert.AreEqual(skill.Skill_Name, input.Skill_Name);
        }
        [Test(), Order(4)]
        public void CreateSkillTest()
        {
            Skill skill = new Skill
            {
                Skill_Id = 0,
                Skill_Name ="TestSkill",
                Associate_Skills = null
            };
            var sk = skillsService.CreateSkill(skill);
            Assert.NotNull(sk);
            Assert.Greater(sk.Skill_Id, 0);
        }
        [Test(), Order(5)]
        public void UpdateSkillTest()
        {
            var skill = skillsService.GetSkillByName("TestSkill");
            skill.Skill_Name = "TestSkillUpdated";
            var sk = skillsService.UpdateSkill(skill);
            Assert.NotNull(sk);
            Assert.AreEqual(sk.Skill_Name, skill.Skill_Name);
        }
        [Test(), Order(6)]
        public void DeleteSkillTest()
        {
            var skill = skillsService.GetSkillByName("TestSkillUpdated");
            skillsService.RemoveSkill(skill);
            var sk = skillsService.GetSkillByName("TestSkillUpdated");
            Assert.Null(sk);
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
            skillsRepository = null;
            skillsService = null;
        }
        #endregion
    }
}
