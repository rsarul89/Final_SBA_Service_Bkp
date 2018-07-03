﻿using System.Collections.Generic;

namespace SkillTracker.WebApi.Models
{
    public class AssociateModel
    {
        public AssociateModel()
        {
            this.Associate_Skills = new List<AssociateSkillsModel>();
        }

        public int Associate_Id { get; set; }

        public string Name { get; set; }

        public string Gender { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public string Pic { get; set; }

        public string Status_Green { get; set; }

        public string Status_Blue { get; set; }

        public string Status_Red { get; set; }

        public string Level_1 { get; set; }

        public string Level_2 { get; set; }

        public string Level_3 { get; set; }

        public string Remark { get; set; }

        public string Strength { get; set; }

        public string Weakness { get; set; }
        public string Other { get; set; }

        public IList<AssociateSkillsModel> Associate_Skills { get; set; }
    }
}