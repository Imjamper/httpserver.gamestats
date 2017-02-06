using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kontur.GameStats.Server.Enums;
using Kontur.GameStats.Server.Context;

namespace Kontur.GameStats.Server.HttpServices
{
    public class GetHttpHandler : HttpHandler
    {
        public GetHttpHandler()
        {
            MethodType = MethodType.GET;
        }
    }
}
