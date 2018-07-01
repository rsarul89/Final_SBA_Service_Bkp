using System;
using System.Collections.Generic;
using SkillTracker.Entities;
using SkillTracker.Repositories;
using System.Linq;

namespace SkillTracker.Services
{
    public class SkillsService : EntityService<Skill>, ISkillsService
    {
        IUnitOfWork unitOfWork;
        ISkillsRepository skillsRepository;

        public SkillsService(IUnitOfWork unitOfWork, ISkillsRepository skillsRepository) : base(unitOfWork, skillsRepository)
        {
            this.unitOfWork = unitOfWork;
            this.skillsRepository = skillsRepository;
        }
        public Skill CreateSkill(Skill skill)
        {
            var addedSkill = skillsRepository.AddSkill(skill);
            unitOfWork.Commit();
            return addedSkill;
        }

        public IEnumerable<Skill> GetAllSkills()
        {
            return skillsRepository.GetAllSkills();
        }

        public Skill GetSkill(int skill_Id)
        {
            return skillsRepository.GetSkill(skill_Id);
        }

        public Skill GetSkillByName(string skill_Name)
        {
            return skillsRepository
                .FindBy(s => s.Skill_Name.Equals(skill_Name, StringComparison.InvariantCultureIgnoreCase))
                .FirstOrDefault();
        }

        public void RemoveSkill(Skill skill)
        {
            skillsRepository.DeleteSkill(skill);
            unitOfWork.Commit();
        }

        public Skill UpdateSkill(Skill skill)
        {
            var result = skillsRepository.UpdateSkill(skill);
            unitOfWork.Commit();
            return result;
        }
    }
}
