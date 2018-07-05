using SkillTracker.Entities;
using System.Collections.Generic;

namespace SkillTracker.Repositories
{
    public interface IAssociatesRepository : IGenericRepository<Associate>
    {
        Associate AddAssociate(Associate associate);
        void DeleteAssociate(Associate associate);
        void UpdateAssociate(Associate associate);
        IEnumerable<Associate> GetAllAssociates();
        Associate GetAssociate(int associate_id);
        Associate GetAssociate(string associate_email);
    }
}
