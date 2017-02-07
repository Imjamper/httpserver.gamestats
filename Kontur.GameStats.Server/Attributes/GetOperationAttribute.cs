using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kontur.GameStats.Server.Enums;

namespace Kontur.GameStats.Server.Attributes
{
    /// <summary>
    /// Атрибут для GET метода
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class GetOperationAttribute : HttpOperationAttribute
    {
        public GetOperationAttribute(string name, string url) : base(name, url)
        {
            MethodType = MethodType.GET;
        }
    }
}
