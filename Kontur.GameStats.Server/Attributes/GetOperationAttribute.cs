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
        public GetOperationAttribute(string url) : base(url)
        {
            MethodType = MethodType.GET;
        }
    }
}
