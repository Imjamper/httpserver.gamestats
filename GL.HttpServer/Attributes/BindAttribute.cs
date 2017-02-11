using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.HttpServer.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class BindAttribute : Attribute
    {
        public BindAttribute(string bindMask)
        {
            BindMask = bindMask;
        }

        public string BindMask { get; set; }
    }
}
