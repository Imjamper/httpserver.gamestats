using System;
using GL.HttpServer.Enums;

namespace GL.HttpServer.Attributes
{
    /// <summary>
    ///     Атрибут для GET метода
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