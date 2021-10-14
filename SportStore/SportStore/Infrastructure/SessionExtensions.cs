using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace SportStore.Infrastructure
{
    public static class SessionExtensions
    {
        public static void SetJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetJSon<T>(this ISession session, string key)
        {
            var sessionData = session.GetString(key);
            return sessionData is null 
                ? default 
                : JsonConvert.DeserializeObject<T>(sessionData);
        }
    }
}