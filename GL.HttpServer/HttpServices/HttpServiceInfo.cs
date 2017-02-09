using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GL.HttpServer.Attributes;
using GL.HttpServer.Enums;
using GL.HttpServer.Extensions;
using GL.HttpServer.Types;

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
                var attribute = method.GetAttribute<HttpOperationAttribute>();
                _methods.Add(new HttpMethodInfo(attribute.Name, attribute.Url, method, attribute.MethodType));
                MethodNames.Add(attribute.Name);
            }
        }

        public HttpServiceInfo()
        {
        }

        public string Name { get; set; }

        public List<string> MethodNames { get; } = new List<string>();

        public HttpMethodInfo GetMethod(MethodType methodType, List<UrlParameter> urlParameters)
        {
            return _methods.FirstOrDefault(a => a.MethodInfo.CompareByParams(urlParameters));
        }
    }
}