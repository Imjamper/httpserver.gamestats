using Kontur.GameStats.Server.Attributes;
using Kontur.GameStats.Server.Enums;
using Kontur.GameStats.Server.Extensions;
using Kontur.GameStats.Server.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server.HttpServices
{
    public class HttpServiceInfo
    {
        private List<HttpMethodInfo> _methods = new List<HttpMethodInfo>();
        private List<string> _methodNames = new List<string>();

        public HttpServiceInfo(string name, List<MethodInfo> methods)
        {
            Name = name;
            foreach (var method in methods)
            {
                var attribute = method.GetAttribute<HttpOperationAttribute>();
                _methods.Add(new HttpMethodInfo(attribute.Name, attribute.Url, method, attribute.MethodType));
                _methodNames.Add(attribute.Name);
            }
        }

        public HttpServiceInfo()
        {
        }
        public string Name { get; set; }

        public List<string> MethodNames
        {
            get
            {
                return _methodNames;
            }
        }

        public HttpMethodInfo GetMethod(MethodType methodType, List<UrlParameter> urlParameters)
        {
            return _methods.FirstOrDefault(a => a.MethodInfo.CompareByParams(urlParameters));
        }
    }
}
