using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server.Types
{
    public abstract class KnownTypeParser<T> : KnownTypeParser, IKnownTypeParser<T>
    {
        public abstract T Parse(string input);

        public override Type Type
        {
            get { return typeof(T); }
        }

        public override object ParseObject(string input)
        {
            return Parse(input);
        }
    }

    public abstract class KnownTypeParser : IKnownTypeParser
    {
        public abstract Type Type { get; }

        public abstract bool CanParse(string input);

        public abstract object ParseObject(string input);
    }
}
