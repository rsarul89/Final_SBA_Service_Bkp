using Newtonsoft.Json;

namespace SkillTracker.WebApi
{
    public static class Helper
    {
        public static T CastObject<T>(object source)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
        }
    }
}