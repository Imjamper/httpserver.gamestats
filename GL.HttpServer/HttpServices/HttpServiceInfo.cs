using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GL.HttpServer.Attributes;
using GL.HttpServer.Enums;
using GL.HttpServer.Extensions;
using GL.HttpServer.Types;
using System;
using GL.HttpServer.Server;

namespace GL.HttpServer.HttpServices
{
    public class HttpServiceInfo
    {
        private readonly List<HttpMethodInfo> _methods = new List<HttpMethodInfo>();

        public HttpServiceInfo(string name, List<MethodInfo> methods)
        {
            Name = name;
            foreach (var method in methods)
            {
                var httpAttribute = method.GetAttribute<HttpOperationAttribute>();
                var methodInfo = new HttpMethodInfo(httpAttribute.Name, httpAttribute.Url, method, httpAttribute.MethodType);
                Console.WriteLine($"        {httpAttribute.MethodType.ToString("G")} {ServerEnviroment.Host}{name}{httpAttribute.Url}");
                foreach (var parameter in method.GetParameters())
                {
                    var bindAttribute = parameter.GetAttribute<BindAttribute>();
                    if (bindAttribute != null)
                    {
                        var parameterBindInfo = new ParameterBindInfo(bindAttribute.BindMask, parameter.Name, parameter.ParameterType);
                        methodInfo.BindInfos.Add(parameterBindInfo);
                    }
                }
                _methods.Add(methodInfo);
                MethodNames.Add(httpAttribute.Name);
            }
        }

        public HttpServiceInfo()
        {
        }

        public string Name { get; set; }

        public List<string> MethodNames { get; } = new List<string>();

        public HttpMethodInfo GetMethod(MethodType methodType, string methodName, List<UrlParameter> urlParameters)
        {
            return _methods.FirstOrDefault(a => a.MethodInfo.CompareByParams(urlParameters) && a.MethodType == methodType && a.Name == methodName);
        }

        public List<HttpMethodInfo> GetMethods(MethodType methodType, string methodName)
        {
            return _methods.Where(a => a.Name == methodName && a.MethodType == methodType).ToList();
        }
    }
}