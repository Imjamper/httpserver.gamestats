using Kontur.GameStats.Server.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server.HttpServices
{
    public class MethodStoreItem
    {
        public MethodStoreItem()
        {

        }

        public MethodStoreItem(string url, MethodInfo methodInfo, MethodType methodType)
        {
            Url = url;
            MethodInfo = methodInfo;
            MethodType = methodType;
        }

        public string Url { get; set; }

        public MethodType MethodType { get; set; }

        public MethodInfo MethodInfo { get; set; }

        public object Invoke(object obj, object[] parameters)
        {
           return MethodInfo.Invoke(obj, parameters);
        }
    }
}
