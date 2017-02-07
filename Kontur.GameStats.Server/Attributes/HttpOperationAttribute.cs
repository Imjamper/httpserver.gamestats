using Kontur.GameStats.Server.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server.Attributes
{
    /// <summary>
    /// Атрибут для метода который доступен по http
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class HttpOperationAttribute : Attribute
    {
        public HttpOperationAttribute(string name, string url)
        {
            Url = url;
            Name = name;
        }

        /// <summary>
        /// Адрес для обращения к методу
        /// </summary>
        public string Url { get; set; }

        public string Name { get; set; }

        public MethodType MethodType { get; set; } 
    }
}
