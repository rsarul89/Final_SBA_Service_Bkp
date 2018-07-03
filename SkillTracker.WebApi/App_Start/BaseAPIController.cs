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
            response.Content = new StringContent(JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, NullValueHandling = NullValueHandling.Include, ContractResolver = new CustomResolver() }), System.Text.Encoding.UTF8, "application/json");
            //response.Content = new StringContent(Helper.Serialize(obj), System.Text.Encoding.UTF8, "application/json");
            return response;
        }
    }
}