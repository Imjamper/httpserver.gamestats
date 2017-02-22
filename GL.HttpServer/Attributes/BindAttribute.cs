using System;

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
