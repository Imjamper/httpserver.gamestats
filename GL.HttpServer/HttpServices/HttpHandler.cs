using System;
using System.Linq;
using System.Threading.Tasks;
using GL.HttpServer.Context;
using GL.HttpServer.Enums;
using GL.HttpServer.Extensions;
using GL.HttpServer.Types;
using System.Reactive.Linq;
using GL.HttpServer.Logging;

namespace GL.HttpServer.HttpServices
{
    public abstract class HttpHandler : IHttpHandler
    {
        public MethodType MethodType { get; set; }

        public ComponentContainer ComponentContainer => ComponentContainer.Current;

        public void Subscribe(IObservable<RequestContext> observableContext)
        {
            observableContext.Where(a => a.Request.HttpMethod == MethodType).Subscribe(ProcessRequest);
        }

        public virtual IObservable<HttpMethodInfo> GetMethod(RequestContext requestContext)
        {
            return Observable.FromAsync(() =>
            {
                return Task.Run(() =>
                {
                    var serviceName = requestContext.Request.RawUrl.GetServiceName();
                    var service = ComponentContainer.GetService(serviceName);
                    if (service != null)
                    {
                        var url = Uri.UnescapeDataString(requestContext.Request.RawUrl);
                        var methodName = service.MethodNames.FirstOrDefault(a => url.Exclude(serviceName).Contains(a));
                        if (!string.IsNullOrEmpty(methodName))
                        {
                            var matchMethods = service.GetMethods(MethodType, methodName);
                            var urlParameters = UrlParser.Parse(url, methodName, matchMethods, serviceName);
                            requestContext.Request.Parameters.AddRange(urlParameters);
                            return service.GetMethod(MethodType, methodName, urlParameters);
                        }
                    }
                    Logger.Info($"The method is not found. Invalid request. Request: {requestContext.Request.HttpMethod:G} {requestContext.Request.RawUrl}");
                    requestContext.Respond(new EmptyResponse());
                    return null;
                });
            });
        }

        public virtual void ProcessRequest(RequestContext requestContext)
        {
            GetMethod(requestContext)
                .Subscribe(m =>
                {
                    if (m == null)
                    {
                        Logger.Info($"The method is not found. Invalid request. Request: {requestContext.Request.HttpMethod:G} {requestContext.Request.RawUrl}");
                        requestContext.Respond(new EmptyResponse());
                        return;
                    }
                    m.Invoke(requestContext);
                });
        }
    }
}