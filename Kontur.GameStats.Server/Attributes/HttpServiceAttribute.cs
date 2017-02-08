using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server.Attributes
{
    /// <summary>
    /// Атрибут для указания пути к http сервису
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
