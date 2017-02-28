using System;

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
