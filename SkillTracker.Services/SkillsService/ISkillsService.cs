using SkillTracker.Entities;
using System.Collections.Generic;

namespace SkillTracker.Services
{
    public interface ISkillsService : IEntityService<Skill>
    {
        IEnumerable<Skill> GetAllSkills();
        Skill CreateSkill(Skill skill);
        Skill UpdateSkill(Skill skill);
        void RemoveSkill(Skill skill);
        Skill GetSkill(int skill_Id);
        Skill GetSkillByName(string skill_Name);
    }
}
