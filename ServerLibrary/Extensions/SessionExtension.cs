using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text.Json;

namespace ServerLibrary.Extensions
{
    public static class SessionExtension
    {
        public static void Set<T>(this ISession session,string key,T value)
        {
            var json = JsonConvert.SerializeObject(value, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Include,
                DefaultValueHandling = DefaultValueHandling.Include
            });
            session.SetString(key, json);
        }
        public static T? Get<T>(this ISession session,string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
