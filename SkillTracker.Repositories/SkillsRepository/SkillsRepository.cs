using SkillTracker.Entities;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;

namespace SkillTracker.Repositories
{
    public class SkillsRepository : GenericRepository<Skill>, ISkillsRepository
    {
        public SkillsRepository(DbContext context)
           : base(context) { }

        public Skill AddSkill(Skill skill)
        {
            Add(skill);
            return skill;
        }

        public void DeleteSkill(Skill skill)
        {
            var result = FindBy(s => s.Skill_Id == skill.Skill_Id).FirstOrDefault();
            Delete(result);
        }

        public IEnumerable<Skill> GetAllSkills()
        {
            var skills = GetAll()
                .AsQueryable()
                .Include(s => s.Associate_Skills)
                .AsEnumerable();
            return skills;
        }

        public Skill GetSkill(int skill_Id)
        {
            var skill = FindBy(sk => sk.Skill_Id == skill_Id)
                .AsQueryable()
                .Include(s => s.Associate_Skills)
                .FirstOrDefault();
            return skill;
        }

        public Skill UpdateSkill(Skill skill)
        {
            var entry = _entities.Entry<Skill>(skill);
            if (entry.State == System.Data.EntityState.Detached)
            {
                var set = _entities.Set<Skill>();
                var attachedEntity = set.Find(skill.Skill_Id);
                if (attachedEntity != null)
                {
                    var attachedEntry = _entities.Entry(attachedEntity);
                    attachedEntry.CurrentValues.SetValues(skill);
                }
                else
                {
                    entry.State = System.Data.EntityState.Modified;
                }
            }
            var result = FindBy(s => s.Skill_Id == skill.Skill_Id).FirstOrDefault();
            return result;
        }
    }
}
