using Kontur.GameStats.Server.Context;
using Kontur.GameStats.Server.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server.HttpServices
{
    public abstract class HttpHandler : IHttpHandler
    {
        protected StatServer _server;
        public MethodType MethodType { get; set; }

        public void Subscribe(StatServer server)
        {
            server.Where(a => a.Request.HttpMethod == MethodType).Subscribe(async => ProcessRequest(async));
        }

        public abstract void ProcessRequest(RequestContext requestContext);
    }
}
