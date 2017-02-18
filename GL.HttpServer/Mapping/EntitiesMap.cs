using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GL.HttpServer.Attributes;
using GL.HttpServer.Extensions;

namespace GL.HttpServer.Mapping
{
    public class EntitiesMap : Profile
    {
        public EntitiesMap()
        {
            var types =
                AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(t => t.GetTypes())
                    .Where(a => a.IsClass && !a.IsAbstract);
            foreach (var type in types)
            {
                var mapsToAttribute = type.GetAttribute<MapsToAttribute>();
                if (mapsToAttribute != null)
                {
                    this.CreateMap(type, mapsToAttribute.MapsToType);
                    this.CreateMap(mapsToAttribute.MapsToType, type);
                }
            }
        }
    }
}
