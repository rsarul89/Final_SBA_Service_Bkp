
namespace SkillTracker.WebApi.Models
{
    public class AssociateSkillsModel
    {
        public int Id { get; set; }

        public int Associate_Id { get; set; }

        public int Skill_Id { get; set; }

        public long Rating { get; set; }

        public AssociateModel Associate { get; set; }

        public SkillModel Skill { get; set; }
    }
}