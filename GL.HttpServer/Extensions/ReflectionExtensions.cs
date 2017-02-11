using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GL.HttpServer.Context;
using GL.HttpServer.Types;
using System.Collections;

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

        public static T GetAttribute<T>(this MethodInfo methodInfo) where T : Attribute
        {
            return methodInfo.GetCustomAttribute(typeof(T), false) as T;
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
            return type.GetMethods().Where(m => HasAttribute<T>(m)).ToList();
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

        public static Response Invoke(this MethodInfo methodInfo, object[] parameters, object obj = null)
        {
            object instance;
            if (obj == null)
                instance = Activator.CreateInstance(methodInfo.DeclaringType);
            else instance = obj;
            Response response = null;
            try
            {
                response = methodInfo.Invoke(instance, parameters) as Response;
            }
            catch (Exception ex)
            {
                response = new ErrorResponse(ex.Message);
            }

            return response;
        }

        public static bool CompareByParams(this MethodInfo method, List<UrlParameter> urlParameters)
        {
            var methodParameters =
                method.GetParameters().Where(p => !typeof(JsonResponse).IsAssignableFrom(p.ParameterType)).ToList();
            if (urlParameters.Count != methodParameters.Count)
                return false;

            for (var index = 0; index <= methodParameters.Count - 1; index++)
            {
                var methodParameter = methodParameters.ElementAt(index);
                var urlParameter = urlParameters.ElementAtOrDefault(index);
                if (methodParameter == null || urlParameter == null ||
                    methodParameter.ParameterType != urlParameter.Type)
                    return false;
            }

            return true;
        }
    }
}