using System.Collections.Generic;

namespace SkillTracker.WebApi.Models
{
    public class SkillModel
    {
        public SkillModel()
        {
            this.Associate_Skills = new List<AssociateSkillsModel>();
        }

        public int Skill_Id { get; set; }

        public string Skill_Name { get; set; }

        public IList<AssociateSkillsModel> Associate_Skills { get; set; }
    }
}