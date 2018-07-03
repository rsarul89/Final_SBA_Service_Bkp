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
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, NullValueHandling = NullValueHandling.Include, ContractResolver = new CustomResolver() }));
        }
        public static string Serialize(object value)
        {
            Type type = value.GetType();

            Newtonsoft.Json.JsonSerializer json = new Newtonsoft.Json.JsonSerializer();

            json.NullValueHandling = NullValueHandling.Include;

            json.ObjectCreationHandling = Newtonsoft.Json.ObjectCreationHandling.Replace;
            json.MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore;
            json.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            StringWriter sw = new StringWriter();
            Newtonsoft.Json.JsonTextWriter writer = new JsonTextWriter(sw);
            writer.Formatting = Formatting.Indented;

            writer.QuoteChar = '"';
            json.Serialize(writer, value);

            string output = sw.ToString();
            writer.Close();
            sw.Close();

            return output;
        }

        public static object Deserialize(string jsonText, Type valueType)
        {
            Newtonsoft.Json.JsonSerializer json = new Newtonsoft.Json.JsonSerializer();

            json.NullValueHandling = Newtonsoft.Json.NullValueHandling.Include;
            json.ObjectCreationHandling = Newtonsoft.Json.ObjectCreationHandling.Replace;
            json.MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore;
            json.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            StringReader sr = new StringReader(jsonText);
            Newtonsoft.Json.JsonTextReader reader = new JsonTextReader(sr);
            object result = json.Deserialize(reader, valueType);
            reader.Close();

            return result;
        }
    }
    
    public static class HttpResponseBaseExtensions
    {
        public static void WriteJson<T>(this HttpResponseBase response, T obj, bool useResponseBuffering = true, bool includeNull = true)
        {
            var contentEncoding = response.ContentEncoding ?? Encoding.UTF8;
            if (!useResponseBuffering)
            {
                response.Buffer = false;

                // use a BufferedStream as suggested in //https://stackoverflow.com/questions/26010915/unbuffered-output-very-slow
                var bufferedStream = new BufferedStream(response.OutputStream, 256 * 1024);
                bufferedStream.WriteJson(obj, contentEncoding, includeNull);
                bufferedStream.Flush();
            }
            else
            {
                response.OutputStream.WriteJson(obj, contentEncoding, includeNull);
            }
        }

        static void WriteJson<T>(this Stream stream, T obj, Encoding contentEncoding, bool includeNull)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new JsonConverter[] { new StringEnumConverter() },
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,//newly added
                                                                                     //PreserveReferencesHandling =Newtonsoft.Json.PreserveReferencesHandling.Objects,
                NullValueHandling = includeNull ? NullValueHandling.Include : NullValueHandling.Ignore
            };
            var serializer = JsonSerializer.CreateDefault(settings);
            var textWriter = new StreamWriter(stream, contentEncoding);
            serializer.Serialize(textWriter, obj);
            textWriter.Flush();
        }
    }
}