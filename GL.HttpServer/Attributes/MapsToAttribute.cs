using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
