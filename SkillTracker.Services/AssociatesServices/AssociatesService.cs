using SkillTracker.Entities;
using SkillTracker.Repositories;
using System.Collections.Generic;
using System;

namespace SkillTracker.Services
{
    public class AssociatesService : EntityService<Associate>, IAssociatesService
    {
        IUnitOfWork unitOfWork;
        IAssociatesRepository associatesRepository;

        public AssociatesService(IUnitOfWork unitOfWork, IAssociatesRepository associatesRepository) : base(unitOfWork, associatesRepository)
        {
            this.unitOfWork = unitOfWork;
            this.associatesRepository = associatesRepository;
        }
        public Associate CreateAssociate(Associate associate)
        {
           var addedAssociate = associatesRepository.AddAssociate(associate);
            unitOfWork.Commit();
            return addedAssociate;
        }

        public IEnumerable<Associate> GetAllAssociates()
        {
            return associatesRepository.GetAllAssociates();
        }

        public Associate GetAssociate(int associate_Id)
        {
            return associatesRepository.GetAssociate(associate_Id);
        }

        public Associate GetAssociate(string associate_email)
        {
            return associatesRepository.GetAssociate(associate_email);
        }

        public void RemoveAssociate(Associate associate)
        {
            associatesRepository.DeleteAssociate(associate);
            unitOfWork.Commit();
        }

        public Associate UpdateAssociate(Associate associate)
        {
            var updatedAssociate = associatesRepository.UpdateAssociate(associate);
            unitOfWork.Commit();
            return updatedAssociate;
        }
    }
}
