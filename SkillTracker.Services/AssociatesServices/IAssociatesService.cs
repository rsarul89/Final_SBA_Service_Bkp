using SkillTracker.Entities;
using System.Collections.Generic;

namespace SkillTracker.Services
{
    public interface IAssociatesService : IEntityService<Associate>
    {
        IEnumerable<Associate> GetAllAssociates();
        Associate CreateAssociate(Associate associate);
        Associate UpdateAssociate(Associate associate);
        void RemoveAssociate(Associate associate);
        Associate GetAssociate(int associate_Id);
        Associate GetAssociate(string associate_email);
    }
}
