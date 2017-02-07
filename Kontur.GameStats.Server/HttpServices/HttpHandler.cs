using Kontur.GameStats.Server.Context;
using Kontur.GameStats.Server.Enums;
using Kontur.GameStats.Server.Extensions;
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
        protected ServicesContainer _servicesContainer;
        public MethodType MethodType { get; set; }

        public void Subscribe(StatServer server)
        {
            server.Where(a => a.Request.HttpMethod == MethodType).Subscribe(async => ProcessRequest(async));
        }

        public virtual IObservable<MethodInfoItem> GetMethod(RequestContext requestContext)
        {
            var methodName = requestContext.Request.RawUrl.GetMethodName();
            return Observable.FromAsync(() => 
            {
                return Task.Run(() => 
                {
                    return _servicesContainer.GetMethod(methodName, MethodType, requestContext.Request.RawUrl);
                });
            });
        }

        public virtual void ProcessRequest(RequestContext requestContext)
        {
            var method = GetMethod(requestContext).Subscribe(m => m.Invoke(requestContext));
        }

        public void SetContainer(ServicesContainer servicesContainer)
        {
            _servicesContainer = servicesContainer;
        }
    }
}
