using System;

namespace GL.HttpServer.Attributes
{
    public class MapsToAttribute : Attribute
    {
        public MapsToAttribute(Type mapsToType)
        {
            MapsToType = mapsToType;
        }
        public Type MapsToType { get; set; }
    }
}
