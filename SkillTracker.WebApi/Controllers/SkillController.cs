using SkillTracker.Common.Exception;
using SkillTracker.Entities;
using SkillTracker.Services;
using SkillTracker.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;

namespace SkillTracker.WebApi.Controllers
{
    [RoutePrefix("api/skill")]
    [Authentication]
    public class SkillController : BaseAPIController
    {
        private ISkillsService _skillsService;
        private ILogManager _logManager;
        public SkillController(ISkillsService skillsService, ILogManager logManager)
        {
            _skillsService = skillsService;
            _logManager = logManager;
        }

        [HttpGet]
        [Route("getSkills")]
        public HttpResponseMessage GetSkills()
        {
            IEnumerable<SkillModel> result = null;
            try
            {
                var res = _skillsService.GetAllSkills();
                result = Helper.CastObject<IEnumerable<SkillModel>>(res);
            }
            catch (Exception ex)
            {
                _logManager.WriteLog(ex);
            }
            return ToJson(result);
        }

        [HttpPost]
        [Route("addSkill")]
        public HttpResponseMessage AddSkill([FromBody] SkillModel skill)
        {
            SkillModel result = new SkillModel();
            try
            {
                var alreadyExists = _skillsService.GetSkillByName(skill.Skill_Name);
                if (alreadyExists == null)
                {
                    var input = Helper.CastObject<Skill>(skill);
                    var res = _skillsService.CreateSkill(input);
                    result = Helper.CastObject<SkillModel>(res);
                }
            }
            catch (Exception ex)
            {
                _logManager.WriteLog(ex);
            }
            return ToJson(result);
        }

        [HttpPost]
        [Route("getSkill")]
        public HttpResponseMessage GetSkill([FromBody] SkillModel skill)
        {
            SkillModel result = new SkillModel();
            try
            {
                var res = _skillsService.GetSkill(skill.Skill_Id);
                result = Helper.CastObject<SkillModel>(res);
            }
            catch (Exception ex)
            {
                _logManager.WriteLog(ex);
            }
            return ToJson(result);
        }

        [HttpPut]
        [Route("updateSkill")]
        public HttpResponseMessage UpdateSkill([FromBody] SkillModel skill)
        {
            SkillModel result = new SkillModel();
            try
            {
                var input = Helper.CastObject<Skill>(skill);
                var res = _skillsService.UpdateSkill(input);
                result = Helper.CastObject<SkillModel>(res);
            }
            catch (Exception ex)
            {
                _logManager.WriteLog(ex);
            }
            return ToJson(result);
        }

        [HttpPost]
        [Route("deleteSkill")]
        public HttpResponseMessage DeleteSkill([FromBody] SkillModel skill)
        {
            try
            {
                var input = Helper.CastObject<Skill>(skill);
                _skillsService.RemoveSkill(input);
            }
            catch (Exception ex)
            {
                _logManager.WriteLog(ex);
            }
            return ToJson(skill);
        }
    }
}
