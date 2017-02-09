using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using GL.HttpServer.Context;
using GL.HttpServer.Enums;
using GL.HttpServer.Extensions;

namespace GL.HttpServer.HttpServices
{
    public class HttpMethodInfo
    {
        public HttpMethodInfo()
        {
        }

        public HttpMethodInfo(string name, string url, MethodInfo methodInfo, MethodType methodType)
        {
            Url = url;
            MethodInfo = methodInfo;
            MethodType = methodType;
            ParametersNames = new Dictionary<string, int>();
            Name = name;
        }

        public string Url { get; set; }

        public string Name { get; set; }

        public MethodType MethodType { get; set; }

        public Dictionary<string, int> ParametersNames { get; set; }

        public MethodInfo MethodInfo { get; set; }

        public void Invoke(RequestContext requestContext)
        {
            requestContext.Request.InputStream.ReadBytes(requestContext.Request.ContentLength)
                .Subscribe(a =>
                {
                    var values = new object[MethodInfo.GetParameters().Length];
                    var index = 0;
                    foreach (var parameter in MethodInfo.GetParameters().ToList())
                    {
                        object value = null;
                        if (typeof(JsonResponse).IsAssignableFrom(parameter.ParameterType))
                        {
                            var json = Encoding.UTF8.GetString(a);
                            value = JsonConvert.DeserializeObject(json, parameter.ParameterType);
                        }
                        else
                        {
                            var urlParameter =
                                requestContext.Request.Parameters.FirstOrDefault(p => p.Type == parameter.ParameterType);
                            if (urlParameter != null && urlParameter.Value != null)
                                value = urlParameter.Value;
                        }
                        values[index] = value;
                        index++;
                    }
                    var response = MethodInfo.Invoke<Response>(values);
                    requestContext.Respond(response);
                });
        }
    }
}