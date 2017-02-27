using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.HttpServer.Types
{
    public class DateOffset
    {
        public DateOffset (DateTimeOffset dateTimeOffset)
        {
            Value = dateTimeOffset;
        }
        public DateTimeOffset Value { get; set; }
    }
}
