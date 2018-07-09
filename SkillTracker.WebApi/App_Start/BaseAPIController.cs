﻿using Newtonsoft.Json;
using System.Net.Http;
using System.Web.Http;
namespace SkillTracker.WebApi
{
    public class BaseAPIController : ApiController
    {
        protected HttpResponseMessage ToJson(dynamic obj)
        {
            var response = Request.CreateResponse(System.Net.HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, NullValueHandling = NullValueHandling.Include, PreserveReferencesHandling = PreserveReferencesHandling.None }), System.Text.Encoding.UTF8, "application/json");
            return response;
        }
        protected HttpResponseMessage Unauthorized()
        {
            var response = Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized);
            response.Content = null;
            return response;
        }
        protected HttpResponseMessage RequestBad()
        {
            var response = Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
            response.Content = null;
            return response;
        }
    }
}