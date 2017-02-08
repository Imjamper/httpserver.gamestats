using Kontur.GameStats.Server.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server.Types
{
    public class IntegerParser : KnownTypeParser<int?>
    {
        private readonly Regex regex = new Regex("^[0-9]+$", RegexOptions.Compiled);

        public override bool CanParse(string input)
        {
            int value = 0;
            return int.TryParse(input, out value);
        }

        public override int? Parse(string input)
        {
            int value = 0;
            if(int.TryParse(input, out value))
            {
                return new int?(value);
            }
            return null;
        }
    }
}
