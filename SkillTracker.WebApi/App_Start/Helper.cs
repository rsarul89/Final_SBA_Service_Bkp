using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Text;
using System.Web;

namespace SkillTracker.WebApi
{
    public static class Helper
    {
        public static T CastObject<T>(object source)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, NullValueHandling = NullValueHandling.Include }));
        }
    }
}