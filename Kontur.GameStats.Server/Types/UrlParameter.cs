using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server.Types
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
