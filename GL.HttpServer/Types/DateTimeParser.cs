using System;
using System.Globalization;

namespace GL.HttpServer.Types
{
    public class DateTimeParser : KnownTypeParser<DateTimeOffset?>
    {
        public override bool CanParse(string input)
        {
            DateTimeOffset date;
            if (DateTimeOffset.TryParseExact(input, "o", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out date))
                return true;
            return false;
        }

        public override DateTimeOffset? Parse(string input)
        {
            DateTimeOffset date;
            if (DateTimeOffset.TryParseExact(input, "o", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out date))
                return date;
            return null;
        }
    }
}