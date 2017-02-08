using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server.Types
{
    public class DateTimeParser : KnownTypeParser<DateTime?>
    {
        public override bool CanParse(string input)
        {
            DateTime date;
            if (DateTime.TryParse(input, out date))
            {
                return true;
            }
            return false;
        }

        public override DateTime? Parse(string input)
        {
            DateTime date;
            if (DateTime.TryParse(input, out date))
            {
                return new DateTime?(date);
            }
            return null;
        }
    }
}
