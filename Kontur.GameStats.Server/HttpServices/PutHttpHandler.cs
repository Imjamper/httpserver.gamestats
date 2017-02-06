using Kontur.GameStats.Server.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kontur.GameStats.Server.Context;
using Kontur.GameStats.Server.Extensions;
using System.Reactive.Linq;

namespace Kontur.GameStats.Server.HttpServices
{
    public class PutHttpHandler : HttpHandler
    {
        public PutHttpHandler()
        {
            MethodType = MethodType.PUT;
        }
    }
}
