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
    public class SkillsRepositoryTests
    {
        private SkillTrackerEntities _context;
        IUnitOfWork unitOfWork;
        ISkillsRepository skillsRepository;

        [SetUp]
        public void Setup()
        {
            _context = new SkillTrackerEntities();
            unitOfWork = new UnitOfWork(_context);
            skillsRepository = new SkillsRepository(_context);
        }
        [Test(), Order(1)]
        public void GetAllSkills()
        {
            var skillsList = skillsRepository.GetAllSkills().ToList();
            if (skillsList == null)
                Assert.AreEqual(null, skillsList);
            Assert.GreaterOrEqual(skillsList.Count(), 0);
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
            var skill = skillsRepository.GetSkill(input.Skill_Id);
            unitOfWork.Commit();
            if (skill == null)
                Assert.Null(skill);
            Assert.NotNull(skill);
            Assert.AreEqual(skill.Skill_Id, input.Skill_Id);
        }
        [Test(), Order(3)]
        public void CreateSkillTest()
        {
            Skill skill = new Skill
            {
                Skill_Id = 0,
                Skill_Name = "TestSkill",
                Associate_Skills = null
            };
            var sk = skillsRepository.AddSkill(skill);
            unitOfWork.Commit();
            Assert.NotNull(sk);
            Assert.Greater(sk.Skill_Id, 0);
        }
        [Test(), Order(4)]
        public void UpdateSkillTest()
        {
            var skill = skillsRepository.FindBy(s => s.Skill_Name == "TestSkill").FirstOrDefault();
            skill.Skill_Name = "TestSkillUpdated";
            var sk = skillsRepository.UpdateSkill(skill);
            unitOfWork.Commit();
            Assert.NotNull(sk);
            Assert.AreEqual(sk.Skill_Name, skill.Skill_Name);
        }
        [Test(), Order(5)]
        public void DeleteSkillTest()
        {
            var skill = skillsRepository.FindBy(s => s.Skill_Name == "TestSkillUpdated").FirstOrDefault();
            skillsRepository.DeleteSkill(skill);
            unitOfWork.Commit();
            var sk = skillsRepository.FindBy(s => s.Skill_Name == "TestSkillUpdated").FirstOrDefault();
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
        }
        #endregion
    }
}
