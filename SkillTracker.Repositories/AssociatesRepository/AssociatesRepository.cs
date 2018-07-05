using SkillTracker.Entities;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System;

namespace SkillTracker.Repositories
{
    public class AssociatesRepository : GenericRepository<Associate>, IAssociatesRepository
    {
        public AssociatesRepository(DbContext context)
           : base(context) { }
        public Associate AddAssociate(Associate associate)
        {
            Add(associate);
            return associate;
        }

        public void DeleteAssociate(Associate associate)
        {
            var result = FindBy(a => a.Associate_Id == associate.Associate_Id).FirstOrDefault();
            Delete(result);
        }

        public IEnumerable<Associate> GetAllAssociates()
        {
            return GetAll()
                .AsQueryable()
                .Include(s => s.Associate_Skills)
                .Include(a => a.Associate_Skills.Select(x => x.Skill))
                .AsEnumerable();
        }

        public Associate GetAssociate(int associate_id)
        {
            return FindBy(a => a.Associate_Id == associate_id)
               .AsQueryable()
               .Include(a => a.Associate_Skills)
               .Include(a => a.Associate_Skills.Select(x => x.Skill))
               .FirstOrDefault();
        }

        public Associate GetAssociate(string associate_email)
        {
            return FindBy(a => a.Email.Equals(associate_email, StringComparison.InvariantCultureIgnoreCase))
               .AsQueryable()
               .Include(a => a.Associate_Skills)
               .Include(a => a.Associate_Skills.Select(x => x.Skill))
               .FirstOrDefault();
        }

        public void UpdateAssociate(Associate associate)
        {
            var entry = _entities.Entry<Associate>(associate);
            if (entry.State == System.Data.EntityState.Detached)
            {
                var set = _entities.Set<Associate>();
                var attachedEntity = set.Find(associate.Associate_Id);
                if (attachedEntity != null)
                {
                    var attachedEntry = _entities.Entry(attachedEntity);
                    attachedEntry.CurrentValues.SetValues(associate);                }
                else
                {
                    entry.State = System.Data.EntityState.Modified;
                }
            }
            if (associate.Associate_Skills != null && associate.Associate_Skills.Count > 0)
            {
                foreach (Associate_Skills ass in associate.Associate_Skills)
                {
                    var entry1 = _entities.Entry<Associate_Skills>(ass);
                    if (entry1.State == System.Data.EntityState.Detached)
                    {
                        var set1 = _entities.Set<Associate_Skills>();
                        var attachedEntity1 = set1.Find(ass.Id);
                        if (attachedEntity1 != null)
                        {
                            var attachedEntry1 = _entities.Entry(attachedEntity1);
                            attachedEntry1.CurrentValues.SetValues(ass);
                        }
                        else
                        {
                            entry.State = System.Data.EntityState.Modified;
                        }
                    }
                }
            }
        }
    }
}
