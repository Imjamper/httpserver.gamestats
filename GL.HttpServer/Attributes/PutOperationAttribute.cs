using System;
using GL.HttpServer.Enums;

namespace GL.HttpServer.Attributes
{
    /// <summary>
    ///     Атрибут для PUT метода
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class PutOperationAttribute : HttpOperationAttribute
    {
        public PutOperationAttribute(string name, string url) : base(name, url)
        {
            MethodType = MethodType.PUT;
        }
    }
}