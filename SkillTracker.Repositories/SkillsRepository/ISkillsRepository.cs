using SkillTracker.Entities;
using System.Collections.Generic;

namespace SkillTracker.Repositories
{
    public interface ISkillsRepository : IGenericRepository<Skill>
    {
        Skill AddSkill(Skill skill);
        void DeleteSkill(Skill skill);
        Skill UpdateSkill(Skill skill);
        IEnumerable<Skill> GetAllSkills();
        Skill GetSkill(int skill_Id);
    }
}
