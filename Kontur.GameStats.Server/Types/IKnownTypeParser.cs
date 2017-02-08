using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server.Types
{
    public interface IKnownTypeParser<T>
    {
        T Parse(string input);
    }

    public interface IKnownTypeParser
    {
        bool CanParse(string input);
        Type Type { get; }
        object ParseObject(string input);
    }
}
