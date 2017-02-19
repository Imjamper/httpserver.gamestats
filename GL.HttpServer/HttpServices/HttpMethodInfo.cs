using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System;
using System.Reactive.Linq;
using GL.HttpServer.Context;
using GL.HttpServer.Enums;
using GL.HttpServer.Extensions;
using Newtonsoft.Json;
using GL.HttpServer.Types;

namespace GL.HttpServer.HttpServices
{
    public class HttpMethodInfo
    {
        public HttpMethodInfo()
        {
            BindInfos = new List<ParameterBindInfo>();
        }

        public HttpMethodInfo(string name, string url, MethodInfo methodInfo, MethodType methodType)
        {
            Url = url;
            MethodInfo = methodInfo;
            MethodType = methodType;
            Name = name;
            BindInfos = new List<ParameterBindInfo>();
        }

        public string Url { get; set; }

        public string Name { get; set; }

        public MethodType MethodType { get; set; }

        public MethodInfo MethodInfo { get; set; }

        public List<ParameterBindInfo> BindInfos { get; set; }

        public void Invoke(RequestContext requestContext)
        {
            var bytes = requestContext.Request.InputStream.ReadAll(requestContext.Request.ContentLength);
            var parametersValues = new object[MethodInfo.GetParameters().Length];
            var index = 0;
            foreach (var parameter in MethodInfo.GetParameters().ToList())
            {
                object value = null;
                if (typeof(JsonResponse).IsAssignableFrom(parameter.ParameterType))
                {
                    var json = Encoding.UTF8.GetString(bytes);
                    value = JsonConvert.DeserializeObject(json, parameter.ParameterType);
                }
                else
                {
                    var urlParameter = requestContext.Request.Parameters.FirstOrDefault(p => p.Type == parameter.ParameterType);
                    if (urlParameter?.Value != null)
                        value = urlParameter.Value;
                }
                parametersValues[index] = value;
                index++;
            }
            var response = MethodInfo.Invoke(parametersValues);
            requestContext.Respond(response);
        }
    }
}