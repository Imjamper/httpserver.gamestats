using System;
using System.Globalization;

namespace GL.HttpServer.Types
{
    public class DateTimeParser : KnownTypeParser<DateTimeOffset?>
    {
        public const string UtcFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ff'Z'";
        public override bool CanParse(string input)
        {
            DateTime date;
            if (DateTime.TryParse(input, out date))
                return true;
            return false;
        }

        public override DateTimeOffset? Parse(string input)
        {
            DateTimeOffset date;
            if (DateTimeOffset.TryParse(input, out date))
                return date;
            return null;
        }
    }
}