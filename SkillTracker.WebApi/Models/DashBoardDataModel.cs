
using System.Collections.Generic;

namespace SkillTracker.WebApi.Models
{
    public class DashBoardDataModel
    {
        public DashBoardDataModel()
        {
            this.registeredUsers = 0;
            this.femaleCandidates = 0;
            this.maleCandidates = 0;
            this.candidateFreshers = 0;
            this.candidatesRated = 0;
            this.femaleCandidatesRated = 0;
            this.maleCandidatesRated = 0;
            this.level1candidates = 0;
            this.level2candidates = 0;
            this.level3candidates = 0;
            this.chartData = new List<ChartData>();
        }
        public int registeredUsers { get; set; }
        public int femaleCandidates { get; set; }
        public int maleCandidates { get; set; }
        public int candidateFreshers { get; set; }
        public int candidatesRated { get; set; }
        public int femaleCandidatesRated { get; set; }
        public int maleCandidatesRated { get; set; }
        public int level1candidates { get; set; }
        public int level2candidates { get; set; }
        public int level3candidates { get; set; }
        public List<ChartData> chartData { get; set; }

    }
}

public class ChartData
{
    public string name { get; set; }
    public string color { get; set; }
    public double percentage { get; set; }
}