using System;

namespace GL.HttpServer.Types
{
    public class UrlParameter
    {
        public UrlParameter()
        {
        }

        public UrlParameter(object value, Type type)
        {
            Value = value;
            Type = type;
        }

        public object Value { get; set; }
        public Type Type { get; set; }
    }
}