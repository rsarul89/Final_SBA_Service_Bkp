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
                var alreadyExists = _associatesService.GetAssociate(assocaite.Email);
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
