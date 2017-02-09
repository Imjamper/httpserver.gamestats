using System;
using System.Linq;
using System.Threading.Tasks;
using GL.HttpServer.Context;
using GL.HttpServer.Enums;
using GL.HttpServer.Extensions;
using GL.HttpServer.Server;
using GL.HttpServer.Types;

namespace GL.HttpServer.HttpServices
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
                        if (!string.IsNullOrEmpty(methodName))
                        {
                            var urlParameters = UrlParser.Parse(url, methodName, serviceName);
                            requestContext.Request.Parameters.AddRange(urlParameters);
                            return service.GetMethod(MethodType, urlParameters);
                        }
                        return null;
                    }
                    return null;
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