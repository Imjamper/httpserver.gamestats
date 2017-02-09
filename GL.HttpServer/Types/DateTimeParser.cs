using System;

namespace GL.HttpServer.Types
{
    public class DateTimeParser : KnownTypeParser<DateTime?>
    {
        public override bool CanParse(string input)
        {
            DateTime date;
            if (DateTime.TryParse(input, out date))
                return true;
            return false;
        }

        public override DateTime? Parse(string input)
        {
            DateTime date;
            if (DateTime.TryParse(input, out date))
                return date;
            return null;
        }
    }
}