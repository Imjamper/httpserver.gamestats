using Kontur.GameStats.Server.Context;
using Kontur.GameStats.Server.Enums;
using Kontur.GameStats.Server.Extensions;
using Kontur.GameStats.Server.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server.HttpServices
{
    public abstract class HttpHandler : IHttpHandler
    {
        protected StatServer _server;
        protected ComponentContainer _servicesContainer;
        public MethodType MethodType { get; set; }

        public void Subscribe(IObservable<RequestContext> observableContext)
        {
            observableContext.Where(a => a.Request.HttpMethod == MethodType).Subscribe(async => ProcessRequest(async));
        }

        public virtual IObservable<HttpMethodInfo> GetMethod(RequestContext requestContext)
        {
            return Observable.FromAsync(() => 
            {
                return Task.Run(() => 
                {
                    var serviceName = requestContext.Request.RawUrl.GetServiceName();
                    var service = _servicesContainer.GetService(serviceName);
                    if (service != null)
                    {
                        var url = Uri.UnescapeDataString(requestContext.Request.RawUrl);
                        var methodName = service.MethodNames.FirstOrDefault(a => url.Exclude(serviceName).Contains(a));
                        if (!String.IsNullOrEmpty(methodName))
                        {
                            var urlParameters = UrlParser.Parse(url, methodName, serviceName);
                            requestContext.Request.Parameters.AddRange(urlParameters);
                            return service.GetMethod(MethodType, urlParameters);
                        }
                        return null;
                    }
                    else return null;
                });
            });
        }

        public virtual void ProcessRequest(RequestContext requestContext)
        {
            GetMethod(requestContext).Where(method => method != null).Subscribe(m => m.Invoke(requestContext));
        }

        public void SetContainer(ComponentContainer servicesContainer)
        {
            _servicesContainer = servicesContainer;
        }
    }
}
