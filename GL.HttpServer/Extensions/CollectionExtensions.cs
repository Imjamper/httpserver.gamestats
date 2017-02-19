using System.Collections;
using GL.HttpServer.Context;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using GL.HttpServer.Dto;
using GL.HttpServer.Entities;

namespace GL.HttpServer.Extensions
{
    public static class CollectionExtensions
    {
        public static IDictionary<string, IEnumerable<string>> ToDictionary(this NameValueCollection source)
        {
            return source.AllKeys.ToDictionary<string, string, IEnumerable<string>>(key => key, source.GetValues);
        }

        public static NameValueCollection ToNameValueCollection(this IDictionary<string, IEnumerable<string>> source)
        {
            var collection = new NameValueCollection();

            foreach (var key in source.Keys)
            foreach (var value in source[key])
                collection.Add(key, value);

            return collection;
        }

        public static JsonList<T> ToJsonList<T>(this IEnumerable list) where T : IDto, new()
        {
            var jsonList = new JsonList<T>();
            foreach (var item in list)
            {
                var entity = item as IEntity;
                if (entity != null)
                    jsonList.Add(entity.ToDto<T>());
            }

            return jsonList;
        }
    }
}