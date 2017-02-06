using Kontur.GameStats.Server.Context;
using Kontur.GameStats.Server.Enums;
using Kontur.GameStats.Server.Extensions;
using Kontur.GameStats.Server.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server.HttpServices
{
    public class MethodInfoItem
    {
        public MethodInfoItem()
        {

        }

        public MethodInfoItem(string url, MethodInfo methodInfo, MethodType methodType)
        {
            Url = url;
            MethodInfo = methodInfo;
            MethodType = methodType;
            ParametersNames = new Dictionary<string, int>();
            MapParametersName();
            Name = Url.GetMethodName();
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
                    int index = 0;
                    foreach (var parameter in MethodInfo.GetParameters().ToList())
                    {
                        object value = null;
                        if (parameter.Name == "body")
                        {
                            var json = Encoding.UTF8.GetString(a);
                            value = JsonConvert.DeserializeObject(json, parameter.ParameterType);
                            values[index] = value;
                        }
                        else
                        {
                            int indexInPath;
                            if (ParametersNames.TryGetValue(parameter.Name, out indexInPath))
                            {
                                value = requestContext.Request.RawUrl.GetValueByIndex(indexInPath);
                                values[index] = value;
                            }
                        }
                        
                        index++;
                    }
                    var response = MethodInfo.Invoke<Response>(values);
                    requestContext.Respond(response);
                });
        }

        private void MapParametersName()
        {
            var regex = new Regex(@"(?<=\<)(.*?)(?=\>)");
            var groups = regex.Match(Url).Groups;
            foreach (Group group in groups)
            {
                if (!ParametersNames.ContainsKey(group.Value))
                {
                    ParametersNames.Add(group.Value, Url.GetIndexInUrl(group.Value));
                }
            }
        }
    }
}
