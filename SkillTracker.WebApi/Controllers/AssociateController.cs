using SkillTracker.Common.Exception;
using SkillTracker.Entities;
using SkillTracker.Services;
using SkillTracker.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace SkillTracker.WebApi.Controllers
{
    [RoutePrefix("api/associate")]
    [Authentication]
    public class AssociateController : BaseAPIController
    {
        private IAssociatesService _associatesService;
        private ILogManager _logManager;
        public AssociateController(IAssociatesService associatesService, ILogManager logManager)
        {
            _associatesService = associatesService;
            _logManager = logManager;
        }

        [HttpGet]
        [Route("getAssociates")]
        public HttpResponseMessage GetAssociates()
        {
            IEnumerable<AssociateModel> result = null;
            try
            {
                var res = _associatesService.GetAllAssociates();
                result = Helper.CastObject<IEnumerable<AssociateModel>>(res);
            }
            catch (Exception ex)
            {
                _logManager.WriteLog(ex);
            }
            return ToJson(result);
        }

        [HttpPost]
        [Route("addAssociate")]
        public HttpResponseMessage AddAssociate([FromBody] AssociateModel assocaite)
        {
            AssociateModel result = new AssociateModel();
            try
            {
                var alreadyExists = _associatesService.GetAssociate(assocaite.Associate_Id);
                if (alreadyExists == null)
                {
                    var input = Helper.CastObject<Associate>(assocaite);
                    var res = _associatesService.CreateAssociate(input);
                    result = Helper.CastObject<AssociateModel>(res);
                }
            }
            catch (Exception ex)
            {
                _logManager.WriteLog(ex);
            }
            return ToJson(result);
        }

        [HttpPost]
        [Route("getAssociate")]
        public HttpResponseMessage GetAssociate([FromBody] AssociateModel associate)
        {
            AssociateModel result = new AssociateModel();
            try
            {
                var res = _associatesService.GetAssociate(associate.Associate_Id);
                result = Helper.CastObject<AssociateModel>(res);
            }
            catch (Exception ex)
            {
                _logManager.WriteLog(ex);
            }
            return ToJson(result);
        }

        [HttpGet]
        [Route("getDashBoardData")]
        public HttpResponseMessage GetDashBoardData()
        {
            DashBoardDataModel result = new DashBoardDataModel();
            try
            {
                var data = _associatesService.GetAllAssociates().ToList();
                if (data != null && data.Count > 0)
                {
                    int totalCandidates = data.Count();
                    result.registeredUsers = totalCandidates;
                    if (totalCandidates > 0)
                    {
                        var candFreshers = data.Where(c => c.Level_1 == true).Count();
                        result.candidateFreshers = candFreshers > 0 ? (candFreshers * 100 / totalCandidates) : 0;
                        var l1Candidates = data.Where(c => c.Level_1 == true).Count();
                        result.level1candidates = l1Candidates > 0 ? (l1Candidates * 100 / totalCandidates) : 0;
                        var l2Candidates = data.Where(c => c.Level_2 == true).Count();
                        result.level2candidates = l2Candidates > 0 ? (l2Candidates * 100 / totalCandidates) : 0;
                        var l3Candidates = data.Where(c => c.Level_3 == true).Count();
                        result.level3candidates = l3Candidates > 0 ? (l3Candidates * 100 / totalCandidates) : 0;
                        result.candidatesRated = data.Where(c => c.Associate_Skills.Any(x => x.Rating > 0)).Count();
                        var femaleCandidates = data.Where(c => c.Gender.Equals("female", StringComparison.InvariantCultureIgnoreCase)).Count();
                        result.femaleCandidates = femaleCandidates > 0 ? (femaleCandidates * 100 / totalCandidates) : 0;
                        var femaleCandidatesRated = data.Where(c => c.Gender.Equals("female", StringComparison.InvariantCultureIgnoreCase)
                           && c.Associate_Skills.Any(x => x.Rating > 0)).Count();
                        result.femaleCandidatesRated = femaleCandidatesRated > 0 ? (femaleCandidatesRated * 100 / totalCandidates) : 0;
                        var maleCandidates = data.Where(c => c.Gender.Equals("male", StringComparison.InvariantCultureIgnoreCase)).Count();
                        result.maleCandidates = maleCandidates > 0 ? (maleCandidates * 100 / totalCandidates) : 0;
                        var maleCandidatesRated = data.Where(c => c.Gender.Equals("male", StringComparison.InvariantCultureIgnoreCase)
                           && c.Associate_Skills.Any(x => x.Rating > 0)).Count();
                        result.maleCandidatesRated = maleCandidatesRated > 0 ? (maleCandidatesRated * 100 / totalCandidates) : 0;

                        var countByName = data
                            .SelectMany(x => x.Associate_Skills)
                            .Where(y => y.Rating > 0)
                            .GroupBy(c => c.Skill.Skill_Name)
                            .Select(g => new
                            {
                                name = g.Key,
                                count = g.Count()
                            }).OrderBy(x => x.name);

                        var totalCount = countByName.Sum(x => x.count);
                        var random = new Random();
                        result.chartData = countByName.Select(pair => new ChartData
                        {
                            name = pair.name,
                            color = string.Format("#{0:X6}", random.Next(0x1000000)),
                            percentage = (pair.count / (double)totalCount * 100)
                        }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                _logManager.WriteLog(ex);
            }
            return ToJson(result);
        }

        [HttpPut]
        [Route("updateAssociate")]
        public HttpResponseMessage UpdateAssociate([FromBody] AssociateModel associate)
        {
            AssociateModel result = new AssociateModel();
            try
            {
                var input = Helper.CastObject<Associate>(associate);
                var res = _associatesService.UpdateAssociate(input);
                result = Helper.CastObject<AssociateModel>(res);
            }
            catch (Exception ex)
            {
                _logManager.WriteLog(ex);
            }
            return ToJson(result);
        }

        [HttpPost]
        [Route("deleteAssociate")]
        public HttpResponseMessage DeleteAssociate([FromBody] AssociateModel associate)
        {
            try
            {
                var input = Helper.CastObject<Associate>(associate);
                _associatesService.RemoveAssociate(input);
            }
            catch (Exception ex)
            {
                _logManager.WriteLog(ex);
            }
            return ToJson(associate);
        }
    }
}
