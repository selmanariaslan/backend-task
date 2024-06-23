using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Task.Core.Utils.Data
{
    public static class JsonUtils
    {
        public static string ToJson(this object obj)
        {
            return JsonSerializer.Serialize(obj);
        }

        public static string ToJson(this object obj, JsonSerializerOptions options)
        {
            return JsonSerializer.Serialize(obj, options);
        }

        //public static string ToJson(this object obj, JsonSerializerSettings settings)
        //{
        //    return JsonSerializer.Serialize(obj, settings);
        //}

        public static T FromJson<T>(this object obj)
        {
            return JsonSerializer.Deserialize<T>(obj as string);
        }

        public static T FromJson<T>(this object obj, JsonSerializerOptions options)
        {
            return JsonSerializer.Deserialize<T>(obj as string, options);
        }
    }
}
