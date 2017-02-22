using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GL.HttpServer.Context;
using GL.HttpServer.Types;
using System.Diagnostics;
using GL.HttpServer.Logging;

namespace GL.HttpServer.Extensions
{
    public static class ReflectionExtensions
    {
        public static bool TryGetAtribute<T>(this MethodInfo methodInfo, out T attribute) where T : Attribute
        {
            var attr = GetAttribute<T>(methodInfo);
            if (attr != null)
            {
                attribute = attr;
                return true;
            }
            attribute = null;
            return false;
        }

        public static bool HasAttribute<T>(this MethodInfo methodInfo) where T : Attribute
        {
            return GetAttribute<T>(methodInfo) != null;
        }

        public static bool HasAttribute<T>(this PropertyInfo propertyInfo) where T : Attribute
        {
            return GetAttribute<T>(propertyInfo) != null;
        }

        public static T GetAttribute<T>(this MethodInfo methodInfo) where T : Attribute
        {
            return methodInfo.GetCustomAttribute(typeof(T), false) as T;
        }

        public static T GetAttribute<T>(this PropertyInfo propertyInfo) where T : Attribute
        {
            return propertyInfo.GetCustomAttribute(typeof(T), false) as T;
        }

        public static bool HasAttribute<T>(this Type type) where T : Attribute
        {
            return GetAttribute<T>(type) != null;
        }

        public static T GetAttribute<T>(this Type type) where T : Attribute
        {
            return type.GetCustomAttribute(typeof(T), false) as T;
        }

        public static bool TryGetAtribute<T>(this Type type, out T attribute) where T : Attribute
        {
            var attr = GetAttribute<T>(type);
            if (attr != null)
            {
                attribute = attr;
                return true;
            }
            attribute = null;
            return false;
        }

        public static List<MethodInfo> GetMethodsWithAttribute<T>(this Type type) where T : Attribute
        {
            return type.GetMethods().Where(HasAttribute<T>).ToList();
        }

        public static List<PropertyInfo> GetPropertiesWithAttribute<T>(this Type type) where T : Attribute
        {
            return type.GetProperties().Where(HasAttribute<T>).ToList();
        }

        public static bool HasAttribute<T>(this ParameterInfo parameter) where T : Attribute
        {
            return GetAttribute<T>(parameter) != null;
        }

        public static T GetAttribute<T>(this ParameterInfo parameter) where T : Attribute
        {
            return parameter.GetCustomAttribute(typeof(T), false) as T;
        }

        public static bool TryGetAtribute<T>(this ParameterInfo parameter, out T attribute) where T : Attribute
        {
            var attr = GetAttribute<T>(parameter);
            if (attr != null)
            {
                attribute = attr;
                return true;
            }
            attribute = null;
            return false;
        }

        

        

        
    }
}