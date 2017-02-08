using Kontur.GameStats.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server.Types
{
    public class EndpointParser : KnownTypeParser<Endpoint>
    {
        public override bool CanParse(string input)
        {
            Endpoint endpoint;
            if(Endpoint.TryParse(input, out endpoint))
            {
                return true;
            }
            return false;
        }

        public override Endpoint Parse(string input)
        {
            Endpoint endpoint;
            if (Endpoint.TryParse(input, out endpoint))
            {
                return endpoint;
            }
            return null;
        }
    }
}
