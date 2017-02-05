using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kontur.GameStats.Server.Enums;

namespace Kontur.GameStats.Server.Attributes
{
    /// <summary>
    /// Атрибут для PUT метода
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class PutOperationAttribute : HttpOperationAttribute
    {
        public PutOperationAttribute(string url) : base(url)
        {
            MethodType = MethodType.PUT;
        }
    }
}
