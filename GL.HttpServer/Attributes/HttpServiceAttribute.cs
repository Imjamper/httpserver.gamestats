using System;

namespace GL.HttpServer.Attributes
{
    /// <summary>
    ///     Атрибут для указания пути к http сервису
    /// </summary>
    public class HttpServiceAttribute : Attribute
    {
        public HttpServiceAttribute()
        {
        }

        public HttpServiceAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}